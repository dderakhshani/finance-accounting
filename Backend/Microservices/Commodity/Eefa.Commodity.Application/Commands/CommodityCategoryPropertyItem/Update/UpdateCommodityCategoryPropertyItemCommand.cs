using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.CommodityCategoryPropertyItem.Update
{
    public class UpdateCommodityCategoryPropertyItemCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.CommodityCategoryPropertyItem>, ICommand
    {
        public int Id { get; set; }


        /// <summary>
        /// کد ویژگی گروه
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;

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
        /// کد
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCommodityCategoryPropertyItemCommand, Eefa.Commodity.Data.Entities.CommodityCategoryPropertyItem>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCommodityCategoryPropertyItemCommandHandler : IRequestHandler<UpdateCommodityCategoryPropertyItemCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.CommodityCategoryPropertyItem> _repository;
        private readonly IMapper _mapper;

        public UpdateCommodityCategoryPropertyItemCommandHandler(IRepository<Data.Entities.CommodityCategoryPropertyItem> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateCommodityCategoryPropertyItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            _mapper.Map<UpdateCommodityCategoryPropertyItemCommand, Data.Entities.CommodityCategoryPropertyItem>(request, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
