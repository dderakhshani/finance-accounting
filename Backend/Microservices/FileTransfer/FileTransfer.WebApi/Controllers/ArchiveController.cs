using FileTransfer.WebApi.Services;
using FileTransfer.WebApi.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileTransfer.WebApi.Controllers
{
    public class ArchiveController : FileTransferBaseController
    {
        private readonly IArchiveServices archiveServices;

        public ArchiveController(IArchiveServices archiveServices)
        {
            this.archiveServices = archiveServices;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArchiveModel model)
        {
            var result = await archiveServices.CreateArchive(model);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateArchiveModel model)
        {
            var result = await archiveServices.UpdateArchive(model);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await archiveServices.DeleteArchive(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var result = await archiveServices.GetArchive(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int typeId)
        {
            var result = await archiveServices.GetArchives(typeId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> SearchAttachment([FromQuery] string searchQuery, [FromQuery] int pageSize = 20, [FromQuery] int pageIndex = 1)
        {
            var result = await archiveServices.SearchAttachment(searchQuery, pageSize, pageIndex);
            return Ok(result);
        }
    }
}
