using AutoMapper;
using Library.Mappings;
using Microsoft.AspNetCore.Http;

namespace FileTransfer.WebApi.Models
{
    public class AttachmentModel : IMapFrom<FileTransfer.WebApi.Persistance.Entities.Attachment>, IMapFrom<AttachmentModel>
    {
        public int? LanguageId { get; set; } = default!;
        public int? TypeBaseId { get; set; } = default!;
        public string Extention { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Description { get; set; }
        public string KeyWords { get; set; }
        public bool? IsUsed { get; set; }
        public string Url { get; set; } = default!;
        public IFormFile File { get; set; } = default!;
        public string TypeBaseTitle { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<FileTransfer.WebApi.Persistance.Entities.Attachment, AttachmentModel>()
                .ForMember(x => x.TypeBaseTitle, opt => opt.MapFrom(x => x.TypeBase.Title))
                ;

            profile.CreateMap<AttachmentModel, FileTransfer.WebApi.Persistance.Entities.Attachment>()
                .IgnoreAllNonExisting()
                ;
        }
    }
    public class UploadFileData :  IMapFrom<FileTransfer.WebApi.Persistance.Entities.Attachment>
    {
        public string Extention { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public int Id { get; set; } = default!;
        public string ProgressStatus { get { return "uploaded"; } }
        public string FullFilePath { get { return FilePath; } }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<FileTransfer.WebApi.Persistance.Entities.Attachment, UploadFileData>()
                .ForMember(x => x.FilePath, opt => opt.MapFrom(x => x.Url));
        }

    }
   
   





}
