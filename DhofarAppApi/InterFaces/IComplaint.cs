﻿using DhofarAppApi.Dtos.Complaint;
using DhofarAppApi.Model;
using DhofarAppApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface IComplaint
    {
        public Task<ComplaintDTO> CreateComplaint(PostComplaintDTO complaint);

        public  Task<List<GetMyComplintDTO>> GetMyComplaints();

        Task<GetCompliantsDtoForId> GetComplaintById(int id);

    }
}
