using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using MD.Bidding.BAL.Interfaces;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MD.Bidding.Web.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        private string _mediaFolder = ConfigurationManager.AppSettings["MediaFolder"];
        private string _tempFolder = ConfigurationManager.AppSettings["TempFolder"];
        private IMediaService _mediaService;

        private IUserService _userService;       

        private string userId = string.Empty;
       

        public UploadController(IMediaService mediaService, IUserService userService)
        {
            _mediaService = mediaService;           
            _userService = userService;            
        }

        [HttpPost]
        public ActionResult Index(int parentId)
        {
            if (parentId > 0)
            {
                if (Request.Files.Count > 0)
                {
                    //Get all file types
                    var extentions = _mediaService.GetAllExtentionTypes();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase hpf = Request.Files[i];
                        if (hpf.ContentLength > 0)
                        {
                            byte[] fileContent = new byte[hpf.ContentLength];
                            hpf.InputStream.Read(fileContent, 0, hpf.ContentLength);

                            string ext = hpf.FileName.Substring(hpf.FileName.LastIndexOf('.'));
                            long extensionTypeId = 0;
                            var extention = (from n in extentions
                                             where n.Ext.ToLower() == ext.ToLower()
                                             select n).FirstOrDefault();

                            if (extention != null)
                            {
                                extensionTypeId = extention.ExtensionTypeId;
                                //Save entry in media table
                                var mediaId = _mediaService.SaveFile(parentId, hpf.FileName, hpf.ContentLength, extensionTypeId, extention.MediaTypeId);

                                if (!System.IO.Directory.Exists(_tempFolder + "\\" + mediaId.ToString()))
                                    System.IO.Directory.CreateDirectory(_tempFolder + "\\" + mediaId.ToString());

                                var _folder = _tempFolder + "\\" + mediaId.ToString() + "\\";
                                var _file = _folder + hpf.FileName;


                                hpf.SaveAs(_file);


                                //Copy file to shared folder
                                var filePath = _mediaFolder + "\\" + mediaId.ToString();
                                if (!Directory.Exists(filePath))
                                    System.IO.Directory.CreateDirectory(filePath);

                                System.IO.File.Copy(_file, filePath + "\\" + hpf.FileName, true);
                            }
                        }
                    }
                }
            }
            return View();
        }
    }
}