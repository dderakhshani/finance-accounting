using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.BaseValue.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.BaseValue.Command.Update
{
    public class UpdateBaseValueCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.BaseValue>, ICommand
    {
        public int Id { get; set; }
        /// <summary>
        /// کد نوع مقدار
        /// </summary>
        public int BaseValueTypeId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }


        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// مقدار
        /// </summary>
        public string Value { get; set; } = default!;

        /// <summary>
        /// ترتیب آرتیکل سند حسابداری
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;
        public string Code { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBaseValueCommand, Data.Databases.Entities.BaseValue>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateBaseValueCommandHandler : IRequestHandler<UpdateBaseValueCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateBaseValueCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateBaseValueCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.BaseValue>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.Value = request.Value;
            entity.Title = request.Title;
            entity.ParentId = request.ParentId;
            entity.UniqueName = request.UniqueName;
            entity.BaseValueTypeId = request.BaseValueTypeId;
            entity.OrderIndex = request.OrderIndex;
            entity.IsReadOnly = request.IsReadOnly;
            entity.Code = request.Code;
            
            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.BaseValue,BaseValueModel>(_repository, _mapper, updateEntity.Entity, cancellationToken);
        }
    }
}
