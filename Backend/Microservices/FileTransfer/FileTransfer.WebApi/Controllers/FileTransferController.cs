using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FileTransfer.WebApi.Models;
using FileTransfer.WebApi.Persistance.Entities;
using FileTransfer.WebApi.Persistance.Sql;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Archives;

namespace FileTransfer.WebApi.Controllers
{
    public class FileTransferController : FileTransferBaseController
    { 
        private readonly IMapper _mapper;
        private readonly IUpLoader _upLoader;
        private readonly IFileTransferUnitOfWork _fileTransferUnitOfWork;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public FileTransferController(
          
            IMapper mapper,
            IUpLoader upLoader,
            ICurrentUserAccessor currentUserAccessor,
            IFileTransferUnitOfWork fileTransferUnitOfWork)
        {
            _mapper = mapper;
            _upLoader = upLoader;
            _fileTransferUnitOfWork = fileTransferUnitOfWork;
            _currentUserAccessor = currentUserAccessor;

       
        }

        [HttpPost]
        public async Task<IActionResult> Upload( IFormFile file)
        {
            var uploadedFile = await _upLoader.UpLoadAsync(file, Guid.NewGuid().ToString(), CustomPath.Temp, CancellationToken.None);

            return Ok(ServiceResult.Success(uploadedFile.ReletivePath));
        }


        [HttpPost]
        public async Task<IActionResult> UploadAttachment()
        {
            var attachmentEntity = new Attachment();
            var httpRequest = HttpContext.Request.Form;
            var file = httpRequest.Files[0];
            var uploadedFile = await _upLoader.UpLoadAsync(file, Guid.NewGuid().ToString(), CustomPath.Attachment, CancellationToken.None);
            attachmentEntity.Url = uploadedFile.ReletivePath;
            attachmentEntity.Extention = Path.GetExtension(file.FileName);
            attachmentEntity.LanguageId = _currentUserAccessor.GetLanguageId();
            attachmentEntity.IsUsed = false;
            attachmentEntity.Title = httpRequest["title"];
            attachmentEntity.Description = httpRequest["description"];
            attachmentEntity.KeyWords = httpRequest["keyWords"];
            attachmentEntity.TypeBaseId = Convert.ToInt32(httpRequest["typeBaseId"]);
            attachmentEntity.FileNumber = httpRequest["fileNumber"];
            _fileTransferUnitOfWork.Attachments.Add(attachmentEntity);

            var archiveId = Convert.ToInt32(httpRequest["archiveId"]);
            if ( archiveId != default)
            {
                var archiveAttachment = new ArchiveAttachments
                {
                    ArchiveId = archiveId,
                    Attachment = attachmentEntity,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = _currentUserAccessor.GetId(),
                    OwnerRoleId = _currentUserAccessor.GetRoleId()
                };
                _fileTransferUnitOfWork.ArchiveAttachments.Add(archiveAttachment);
            }
            await _fileTransferUnitOfWork.SaveAsync(CancellationToken.None);
            
            return Ok(ServiceResult.Success(attachmentEntity));
        }
        
        
        [HttpDelete]
        public async Task<IActionResult> DeleteAttachment([FromQuery]int Id, [FromQuery] int archiveId)
        {
            var item= _fileTransferUnitOfWork.Attachments.Include(x => x.ArchiveAttachments).Where(a => a.Id == Id).FirstOrDefault();
            item.IsDeleted = true;
            item.IsUsed = false;
            item.ModifiedById = _currentUserAccessor.GetId();
            item.ModifiedAt = DateTime.UtcNow;
            _fileTransferUnitOfWork.Attachments.Update(item);

            if (archiveId != default && item.ArchiveAttachments.Any(x => x.ArchiveId == archiveId))
            {
                var archiveAttachment = item.ArchiveAttachments.FirstOrDefault(x => x.ArchiveId == archiveId);
                archiveAttachment.IsDeleted = true;
                archiveAttachment.ModifiedById = _currentUserAccessor.GetId();
                archiveAttachment.ModifiedAt = DateTime.UtcNow;
            }
            await _fileTransferUnitOfWork.SaveAsync(CancellationToken.None);
            return Ok(ServiceResult.Success(item));
        }
        [HttpPost]
        public async Task<IActionResult> GetAttachments(int[] Ids)
        {
            var result = await _fileTransferUnitOfWork.Attachments.Where(a=> Ids.Contains(a.Id)).Select(a=> new UploadFileData()
            {
                Id = a.Id,
                Extention=a.Extention,
                FilePath=a.Url

            }).ToListAsync();

            return Ok(result);
        }



    }
}
