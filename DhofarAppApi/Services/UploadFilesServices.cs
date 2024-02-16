using DhofarAppApi.Data;
using DhofarAppApi.InterFaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DhofarAppApi.Services
{
    public class UploadFilesServices : IUploadFile
    {

       
        private readonly FileServices _fileServices;

        public UploadFilesServices(FileServices fileServices)
        {
            _fileServices = fileServices;
           
        }

       

        public async Task<string> uploadfile(IFormFile file)
        {
            string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = await _fileServices.SaveFileToServerAndDatabase(file, fileName);
            return filePath;
        }
    }
}
