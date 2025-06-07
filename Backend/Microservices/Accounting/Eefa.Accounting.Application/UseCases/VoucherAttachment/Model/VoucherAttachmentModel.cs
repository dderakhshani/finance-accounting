using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.VoucherAttachment.Model
{
    public class VoucherAttachmentModel : IMapFrom<Data.Entities.VoucherAttachment>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد فایل راهنما
        /// </summary>
        public int VoucherHeadId { get; set; } = default!;

        /// <summary>
        /// کد پیوست
        /// </summary>

        public int AttachmentId { get; set; } = default!;

        public int LanguageId { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string Extension { get; set; } = default!;
        public bool IsUsed { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// کلمات کلیدی
        /// </summary>
        public string? KeyWords { get; set; }

        /// <summary>
        /// لینک
        /// </summary>
        public string AttachmentUrl { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.VoucherAttachment, VoucherAttachmentModel>()
                .ForMember(x => x.AttachmentId, opt => opt.MapFrom(x => x.Attachment.Id))
                .ForMember(x => x.LanguageId, opt => opt.MapFrom(x => x.Attachment.LanguageId))
                .ForMember(x => x.TypeBaseId, opt => opt.MapFrom(x => x.Attachment.TypeBaseId))
                .ForMember(x => x.Extension, opt => opt.MapFrom(x => x.Attachment.Extention))
                .ForMember(x => x.IsUsed, opt => opt.MapFrom(x => x.Attachment.IsUsed))
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Attachment.Title))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Attachment.Description))
                .ForMember(x => x.KeyWords, opt => opt.MapFrom(x => x.Attachment.KeyWords))
                .ForMember(x => x.AttachmentUrl, opt => opt.MapFrom(x => x.Attachment.Url))
                ;
        }
    }

}
