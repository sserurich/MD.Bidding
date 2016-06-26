using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD.Bidding.DTO;
using MD.Bidding.EF.Models;

namespace MD.Bidding.DAL.Interfaces
{
    public interface IMediaDataService
    {
        IEnumerable<Media> GetChildren(long parentId);
        Media GetMedia(long mediaId);
        IEnumerable<Media> GetChildFolders(long parentId);
        IEnumerable<Media> GetFilesInFolder(long folderId);
        long SaveFolder(long mediaId, long? parentId, string name);
        long SaveFile(long parentId, string fileName, int contentLength, long extensionTypeId, long mediaTypeId);
        IEnumerable<MediaDTO> GetDescendants(long mediaId);
        void MarkAsDeleted(long mediaId);
        void Delete(long mediaId);
        IEnumerable<ExtensionType> GetAllExtentionTypes();
        long AddExtention(string ext);
        bool HasFolderOrFiles(long folderId);
        string GetPathCsvMediaId(long mediaId);      
        long CreateFolder(long parentId, string name);
    }
}
