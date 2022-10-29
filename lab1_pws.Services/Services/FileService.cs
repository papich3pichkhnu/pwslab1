using lab1_pws.Services.Helpers.Files;
using lab1_pws.Services.Interfaces.Services;
using lab1_pws.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;

namespace lab1_pws.Services.Services
{
    public class FileService : IFileService
    {
        private readonly FileSettings _fileSettings;
        public FileService(IOptionsMonitor<FileSettings> fileSettings)
        {
            _fileSettings = fileSettings.CurrentValue;
        }

        public byte[] GetFileDownload(string path)
        {
            byte [] content = File.ReadAllBytes(path);

            return content;
        }

        public List<FileModel> GetFilesList(string path)
        {
            string[] filepaths = Directory.GetFiles(path);
            List<FileModel> files = new List<FileModel>();
            foreach (string filepath in filepaths)
            {
                files.Add(new FileModel { FileName = Path.GetFileName(filepath) });
            }
            return files;
        }

        public async Task<bool> UploadFile(IFormFile file, string savepath)
        {
            string path = "";
            bool iscopied = false;
            try
            {
                if (file.Length > 0)
                {
                    string filen =Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddTHH-mm-ssZ") + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), savepath));
                    using (var filestream = new FileStream(Path.Combine(path, filen), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    iscopied = true;
                }
                else
                {
                    iscopied = false;
                }
            }

            catch (Exception ex)
            {
                throw;
            }
            return iscopied;
        }
    }
}
