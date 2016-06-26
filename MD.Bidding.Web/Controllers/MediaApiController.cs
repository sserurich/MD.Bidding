using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using MD.Bidding.Models;
using MD.Bidding.BAL.Interfaces;
using MD.Bidding.BAL.Concrete;
using MD.Bidding.Library;
using log4net;


namespace MD.Bidding.Web.Controllers
{
    [Authorize]
    public class MediaApiController : ApiController
    {

        private string userId = string.Empty;

        static ILog logger = log4net.LogManager.GetLogger(typeof(MediaApiController));
        private IMediaService _mediaService;
        private IUserService _userService;

        public MediaApiController(IMediaService mediaService, IUserService userService)
        {
            this._mediaService = mediaService;
            _userService = userService;
            userId = Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(RequestContext.Principal.Identity);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetMedia")]
        public Media GetMedia(long mediaId)
        {
            return _mediaService.GetMedia(mediaId);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetFolder")]
        public IEnumerable<MediaFolder> GetFolder(long mediaId)
        {
            try
            {
                var media = _mediaService.GetMedia(mediaId);
                var mediaFolder = new MediaFolder();
                if (media != null)
                {
                    mediaFolder = new MediaFolder
                    {
                        MediaId = media.MediaId,
                        ParentId = media.ParentId,
                        MediaGuid = media.MediaGuid,
                        label = media.Name,
                    };
                }
                List<MediaFolder> list = new List<MediaFolder>();
                list.Add(mediaFolder);
                return list;
            }
            catch (Exception ex)
            {
                logger.Debug(ex);
                return null;
            }
        }


        [HttpGet]
        [System.Web.Http.ActionName("GetChildFolders")]
        public IEnumerable<MediaFolder> GetChildFolders(long parentId)
        {
            return _mediaService.GetChildFolders(parentId);
        }
        
        [HttpGet]
        [System.Web.Http.ActionName("GetFolderAndChildFolders")]
        public IEnumerable<MediaFolder> GetFolderAndChildFolders(long parentId)
        {
            var result = _mediaService.GetFolderAndChildFolders(parentId);
            List<MediaFolder> list = new List<MediaFolder>();
            list.Add(result);
            return list;
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetFilesInFolder")]
        public IEnumerable<Media> GetFilesInFolder(long folderId, string mediaTypes)
        {
            if (string.IsNullOrEmpty(mediaTypes))
                return _mediaService.GetFilesInFolder(folderId);
            else
                return _mediaService.GetFilesInFolder(folderId, mediaTypes);
        }

        [HttpPost]
        [System.Web.Http.ActionName("SaveFolder")]
        public long SaveFolder(MediaFolder model)
        {
            return _mediaService.SaveFolder(model);
        }

        [HttpGet]
        [System.Web.Http.ActionName("MarkAsDeleted")]
        public void MarkAsDeleted(long mediaId)
        {
            _mediaService.MarkAsDeleted(mediaId);
        }
        
        [HttpGet]
        [System.Web.Http.ActionName("HasFolderOrFiles")]
        public bool HasFolderOrFiles(long folderId)
        {
            return _mediaService.HasFolderOrFiles(folderId);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetAllExtentionTypes")]
        public IEnumerable<ExtensionType> GetAllExtentionTypes()
        {
            return _mediaService.GetAllExtentionTypes();
        }
       
        [HttpGet]
        [System.Web.Http.ActionName("GetPathCsvMediaId")]
        public string GetPathCsvMediaId(long mediaId)
        {
            return _mediaService.GetPathCsvMediaId(mediaId);
        }

        [HttpGet]
        [System.Web.Http.ActionName("GetPathMediaIds")]
        public long[] GetPathMediaIds(long mediaId)
        {
            return _mediaService.GetPathMediaIds(mediaId);
        }

    }
}
