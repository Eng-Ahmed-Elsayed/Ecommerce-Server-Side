using Microsoft.AspNetCore.Http;

namespace Utility.ManageFiles
{
    public interface IManageFiles
    {
        void DeleteImage(string imgPath);
        string UploadFile(IFormFile file, string folderName);
        List<string> UploadFiles(IFormFileCollection files, string folderName);
    }
}
