using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DhofarAppWeb.Data;
using DhofarAppWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DhofarAppWeb.Controllers
{
    public class ComblineController : Controller
    {
        private readonly AppDbContext _db;
        public ComblineController(AppDbContext db)
        {
            _db= db;
        }

        //private readonly HttpClient _httpClient;

        //public ComblineController()
        //{
        //    _httpClient = new HttpClient();
        //    // Set your API base URL here
        //    _httpClient.BaseAddress = new Uri("your_api_base_url");
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateComplaint(Complaint complaint)
        //{
        //    try
        //    {

        //        var jsonContent = new StringContent(JsonConvert.SerializeObject(complaint), Encoding.UTF8, "application/json");


        //        var response = await _httpClient.PostAsync("complaints/create", jsonContent);

        //        if (response.IsSuccessStatusCode)
        //        {

        //            return RedirectToAction("Success");
        //        }
        //        else
        //        {

        //            return View("Error");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return View("Error");
        //    }




        //}



        public async Task<IActionResult> ComplaintChart()
        {
            var allComplaints = await _db.Complaints.ToListAsync();
            var closedComplaintsCount = await _db.Complaints.CountAsync(com => com.Status_En == "Closed" || com.Status_Ar == "مغلقة");

            // Prepare data for the chart
            var complaintData = new Dictionary<string, int>
            {
                { "Total Complaints", allComplaints.Count },
                { "Closed Complaints", closedComplaintsCount }
            };

            return View(complaintData);
        }


    }
}
