using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Utility.ManageFiles
{
    public class ManageFiles : IManageFiles
    {
        public void DeleteImage(string imgPath)
        {
            if (!string.IsNullOrEmpty(imgPath))
            {
                // Delete with full path
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), imgPath));
            }
        }

        private string SaveFile(IFormFile? file, string pathToSave, string folderName)
        {
            var fileName = Guid.NewGuid().ToString() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return dbPath;
        }

        public string UploadFile(IFormFile file, string folderName)
        {
            folderName = Path.Combine("StaticFiles", "Images", folderName);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            return SaveFile(file, pathToSave, folderName);
        }

        public List<string> UploadFiles(IFormFileCollection files, string folderName)
        {
            folderName = Path.Combine("StaticFiles", "Images", folderName);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var dbPathList = new List<string>();
            foreach (var file in files)
            {
                var dbPath = SaveFile(file, pathToSave, folderName);
                dbPathList.Add(dbPath);
            }
            return dbPathList;
        }
    }
}
