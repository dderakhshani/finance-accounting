using FileTransfer.WebApi.Services.Models;
using Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileTransfer.WebApi.Services
{
    public interface IArchiveServices
    {
        Task<ArchiveModel> CreateArchive(CreateArchiveModel model);
        Task<bool> DeleteArchive(int id);
        Task<ArchiveModel> GetArchive(int id);
        Task<List<ArchiveModel>> GetArchives(int typeId);
        Task<ServiceResult> SearchAttachment(string searchQuery, int pageSize, int pageIndex);
        Task<ArchiveModel> UpdateArchive(UpdateArchiveModel model);
    }
}