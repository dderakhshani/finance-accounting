using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Queries.Commodity;
using Eefa.Commodity.Application.Commands.CommodityCategoryPropertyItem.Create;
using System.Collections.Generic;

namespace Eefa.Commodity.Application.Commands.CommodityCategoryProperty.Create
{
    public class CreateCommodityCategoryPropertyCommand : CommandBase, IRequest<ServiceResult<CommodityCategoryPropertyModel>>, IMapFrom<CreateCommodityCategoryPropertyCommand>, ICommand
    {

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد گروه
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// واحد اندازه گیری
        /// </summary>
        public int? MeasureId { get; set; }

        /// <summary>
        /// قوانین حاکم بر مولفه
        /// </summary>
        public int? PropertyTypeBaseId { get; set; }

        /// <summary>
        /// ترتیب نمایش
        /// </summary>
        public int OrderIndex { get; set; } = default!;


        public List<CreateCommodityCategoryPropertyItemCommand> Items { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCommodityCategoryPropertyCommand, Data.Entities.CommodityCategoryProperty>()
                .ForMember(x => x.CommodityCategoryPropertyItems, opt => opt.MapFrom(x => x.Items));
        }
    }

    public class CreateCommodityCategoryPropertyCommandHandler : IRequestHandler<CreateCommodityCategoryPropertyCommand, ServiceResult<CommodityCategoryPropertyModel>>
    {
        private readonly IRepository<Data.Entities.CommodityCategoryProperty> _repository;
        private readonly IMapper _mapper;

        public CreateCommodityCategoryPropertyCommandHandler(IRepository<Data.Entities.CommodityCategoryProperty> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult<CommodityCategoryPropertyModel>> Handle(CreateCommodityCategoryPropertyCommand request, CancellationToken cancellationToken)
        {
            var commodityCategoryProperty = _mapper.Map<Data.Entities.CommodityCategoryProperty>(request);

            var entity = _repository.Insert(commodityCategoryProperty);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult<CommodityCategoryPropertyModel>.Success(
                _mapper.Map<CommodityCategoryPropertyModel>(entity));
        }
    }
}
