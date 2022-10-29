using lab1_pws.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab1_pws.Services.Interfaces.Services
{
    public interface IFileService
    {
        public List<FileModel> GetFilesList(string path);
        public byte[] GetFileDownload(string path);
        public Task<bool> UploadFile(IFormFile file, string savepath);  
    }
}
