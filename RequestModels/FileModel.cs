using Microsoft.AspNetCore.Http;

namespace backend.RequestModels
{
    public class FileModel
    {
        public string FileName { get; set; }

        public IFormFile FormFile { get; set; }
    }
}