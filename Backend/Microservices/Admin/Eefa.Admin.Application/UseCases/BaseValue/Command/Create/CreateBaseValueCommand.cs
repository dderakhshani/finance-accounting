using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


namespace Eefa.Admin.Application.CommandQueries.BaseValue.Command.Create
{
    public class CreateBaseValueCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateBaseValueCommand>,
        ICommand
    {
        /// <summary>
        /// کد نوع مقدار
        /// </summary>
        public int BaseValueTypeId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; }

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
            profile.CreateMap<CreateBaseValueCommand, Data.Databases.Entities.BaseValue>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateBaseValueCommandHandler : IRequestHandler<CreateBaseValueCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public CreateBaseValueCommandHandler(IRepository repository, IMapper mapper,
            ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateBaseValueCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.BaseValue>(request));
            return await request.Save(_repository, entity.Entity, cancellationToken);
        }
    }
}