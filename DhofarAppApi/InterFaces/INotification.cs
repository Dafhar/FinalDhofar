using Microsoft.AspNetCore.Mvc;

namespace DhofarAppApi.InterFaces
{
    public interface INotification
    {
        public Task<IActionResult> SentToUser(string title, string body, string deviceToken);
        public Task<IActionResult> SentToAllUserInDepartment(string title, string body, int departmentId);
        public Task<IActionResult> SentToTopic(string title, string body, string topic);

    }
}
