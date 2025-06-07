using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Commodity.Application.Queries.Commodity;

namespace Eefa.Commodity.Application.Commands.Commodity.Update
{
    public class UpdateCommodityNationalIdCommand : CommandBase, IRequest<ServiceResult<CommodityModel>>, IMapFrom<Data.Entities.Commodity>, ICommand
    {
        /// <summary>
        /// عنوان کد ملی کالا
        /// </summary>
        public int[] ids { get; set; }
        public string commodityNationalId { get; set; }
        public string commodityNationalTitle { get; set; }

    }


    public class UpdateCommodityNationalIdCommandHandler : IRequestHandler<UpdateCommodityNationalIdCommand, ServiceResult<CommodityModel>>
    {
        private readonly IRepository<Data.Entities.Commodity> _repository;
        private readonly IMapper _mapper;

        public UpdateCommodityNationalIdCommandHandler(IRepository<Data.Entities.Commodity> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<CommodityModel>> Handle(UpdateCommodityNationalIdCommand request, CancellationToken cancellationToken)
        {
            for (int i = 0; i < request.ids.Length; i++)
            {
                var entity = await _repository.Find(request.ids[i]);
                entity.CommodityNationalId = request.commodityNationalId;
                entity.CommodityNationalTitle = request.commodityNationalTitle;
            }

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult<CommodityModel>.Success(new CommodityModel());


        }
    }
}

