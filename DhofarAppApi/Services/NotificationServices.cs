using DhofarAppApi.Data;
using DhofarAppApi.InterFaces;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DhofarAppApi.Services
{
    public class NotificationServices : INotification
    {
        private readonly AppDbContext _db;

        public NotificationServices(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> SentToAllUserInDepartment(string title, string body, int departmentId)
        {
            try
            {
                var message = new MulticastMessage()
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Tokens = await GetDeviceTokensAsync(departmentId) 
                };

                await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);

                return new OkObjectResult("Notification sent successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Failed to send notification: {ex.Message}");
            }
        }

        public async Task<IActionResult> SentToUser(string title, string body, string deviceToken)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Token = deviceToken
                };

                await FirebaseMessaging.DefaultInstance.SendAsync(message);

                return new OkObjectResult("Notification sent successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Failed to send notification: {ex.Message}");
            }
        }

        public async Task<IActionResult> SentToTopic(string title, string body, string topic)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Topic = topic
                };

                await FirebaseMessaging.DefaultInstance.SendAsync(message);

                return new OkObjectResult("Notification sent successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Failed to send notification to topic: {ex.Message}");
            }
        }

        private async Task<List<string>> GetDeviceTokensAsync(int departmentId)
        {
            var deviceTokens = await _db.DepartmentAdmins
                                    .Where(d => d.DepartmentTypeId == departmentId)
                                    .SelectMany(d => d.User.DeviceTokens.Select(t => t.Token))
                                    .ToListAsync();

            return deviceTokens;
        }

    }
}
