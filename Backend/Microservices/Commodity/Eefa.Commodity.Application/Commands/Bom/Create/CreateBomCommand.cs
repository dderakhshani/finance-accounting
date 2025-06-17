using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Queries.Bom;
using System.Collections.Generic;
using System.Linq;
using Eefa.Common.Exceptions;

namespace Eefa.Commodity.Application.Commands.Bom.Create
{
    public class CreateBomCommand : CommandBase, IRequest<ServiceResult<BomModel>>, IMapFrom<CreateBomCommand>, ICommand
    {
        /// <summary>
        /// کد والد
        /// </summary>
        public int? RootId { get; set; }
        public string Title { get; set; } = default!;

        public string Description { get; set; } = default!;
       
        /// <summary>
        /// کد گروه کالا
        /// </summary>
        public int CommodityCategoryId { get; set; } = default!;

        public bool IsActive { get; set; }
        public List<BomItemsCreate> BomItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBomCommand, Eefa.Commodity.Data.Entities.Bom>()
                .IgnoreAllNonExisting();
        }
    }
    public class BomItemsCreate : IMapFrom<Eefa.Commodity.Data.Entities.BomItem>

    {
        public int BomId { get; set; } = default!;
        public int SubCategoryId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<BomItemsCreate, Eefa.Commodity.Data.Entities.BomItem>()
                .IgnoreAllNonExisting();
        }


    }

    public class CreateBomCommandHandler : IRequestHandler<CreateBomCommand, ServiceResult<BomModel>>
    {
        private readonly IRepository<Data.Entities.Bom> _repository;
        private readonly IMapper _mapper;

        public CreateBomCommandHandler(
            IRepository<Data.Entities.Bom> repository,
            IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult<BomModel>> Handle(CreateBomCommand request, CancellationToken cancellationToken)
        {
            if (!request.BomItems.Any())
            {
                throw new ValidationError("آیتم های فرمول ساخت وارد نشده است");
            }
            var bom = _mapper.Map<Data.Entities.Bom>(request);
            bom.LevelCode = "0";

            foreach (var items in request.BomItems)
            {

                var bomItem = new Eefa.Commodity.Data.Entities.BomItem();

                bomItem.SubCategoryId = items.SubCategoryId;
                bomItem.CommodityId = items.CommodityId;
                //bom.AddItem(bomItem);


            }
            var entity = _repository.Insert(bom);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult<BomModel>.Success(_mapper.Map<BomModel>(bom));
            }
            else
            {
                return ServiceResult<BomModel>.Failed();
            }



        }
    }
}
