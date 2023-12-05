using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility.ManageFiles;

namespace ecommerce_server_side.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IManageFiles _manageFiles;

        public AdminController(IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            IManageFiles manageFiles)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _manageFiles = manageFiles;
        }

        //[HttpPost, DisableRequestSizeLimit]
        //[Route("upload-file")]
        //public async Task<IActionResult> UploadFile()
        //{
        //    try
        //    {
        //        var formCollection = await Request.ReadFormAsync();
        //        var file = formCollection.Files.First();
        //        if (file.Length > 0)
        //        {
        //            var dbPath = _manageFiles.UploadFile(file, "Category");
        //            return Ok(new { dbPath });
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
        //    }
        //}

        //[HttpPost, DisableRequestSizeLimit]
        //[Route("upload-files")]
        //public async Task<IActionResult> UploadFiles()
        //{
        //    try
        //    {
        //        var formCollection = await Request.ReadFormAsync();
        //        var files = formCollection.Files;
        //        if (files.Any(f => f.Length == 0))
        //        {
        //            return BadRequest();
        //        }
        //        var dbPathList = _manageFiles.UploadFiles(files, "Product");
        //        return Ok(new { dbPathList });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "The server encountered an unexpected condition. Please try again later.");
        //    }
        //}

    }
}

