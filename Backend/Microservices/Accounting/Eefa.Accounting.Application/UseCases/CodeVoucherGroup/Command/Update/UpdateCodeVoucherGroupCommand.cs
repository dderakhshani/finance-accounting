using System;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.CodeVoucherGroup.Command.Update
{
    public class UpdateCodeVoucherGroupCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.CodeVoucherGroup>, ICommand
    {
        public int Id { get; set; }
        /// <summary>
        /// شناسه
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// کد شرکت
        /// </summary>


        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; }
        /// <summary>
        /// تاریخ قفل شدن اطلاعات
        /// </summary>
        public DateTime? LastEditableDate { get; set; }

        /// <summary>
        /// اتوماتیک است؟
        /// </summary>
        public bool IsAuto { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool IsEditable { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;

        /// <summary>
        /// گروه سند مکانیزه
        /// </summary>
        public bool AutoVoucherEnterGroup { get; set; } = default!;

        /// <summary>
        /// فرمول جایگزین خالی بودن
        /// </summary>
        public string? BlankDateFormula { get; set; }

        /// <summary>
        /// کد گزارش
        /// </summary>
        public int? ViewId { get; set; }
        public int? ExtendTypeId { get; set; }
        public string? TableName { get; set; }
        public string? SchemaName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCodeVoucherGroupCommand, Data.Entities.CodeVoucherGroup>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCodeVoucherGroupCommandHandler : IRequestHandler<UpdateCodeVoucherGroupCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public UpdateCodeVoucherGroupCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateCodeVoucherGroupCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.CodeVoucherGroup>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);


            entity.Code = request.Code;
            entity.CompanyId = _currentUserAccessor.GetCompanyId();
            entity.Title = request.Title;
            entity.LastEditableDate = request.LastEditableDate;
            entity.IsAuto = request.IsAuto;
            entity.IsEditable = request.IsEditable;
            entity.IsActive = request.IsActive;
            entity.AutoVoucherEnterGroup = request.AutoVoucherEnterGroup;
            entity.BlankDateFormula = request.BlankDateFormula;
            entity.ViewId = request.ViewId;
            entity.ExtendTypeId = request.ExtendTypeId;
            entity.SchemaName = request.SchemaName;
            entity.TableName = request.TableName;

            _repository.Update(entity);

            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<CodeVoucherGroupModel>(entity));
            }
            return ServiceResult.Failure();
        }
    }
}
