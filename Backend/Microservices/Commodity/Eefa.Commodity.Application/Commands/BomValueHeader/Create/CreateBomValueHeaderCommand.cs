using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Eefa.Common.Exceptions;

namespace Eefa.Commodity.Application.Commands.BomValueHeader.Create
{
    public class CreateBomValueHeaderCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateBomValueHeaderCommand>, ICommand
    {

        /// <summary>
        /// کد فرمول ساخت
        /// </summary>
        public int BomId { get; set; } = default!;

        /// <summary>
        /// کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;

        /// <summary>
        /// تاریخ فرمول ساخت
        /// </summary>
        public DateTime BomDate { get; set; } = default!;
        public string Name { get; set; } = default!;
        public  List<BomValueCreate> Values { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBomValueHeaderCommand, Data.Entities.BomValueHeader>()
                .IgnoreAllNonExisting();
        }
    }
    public  class BomValueCreate : IMapFrom<Eefa.Commodity.Data.Entities.BomValue>
    {


       

        /// <summary>
        /// کد کالای مصرفی
        /// </summary>
        public int UsedCommodityId { get; set; } = default!;
        public int BomWarehouseId { get; set; } = default!;


        /// <summary>
        /// مقدار
        /// </summary>
        public double Value { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<BomValueCreate, Eefa.Commodity.Data.Entities.BomValue>()
                .IgnoreAllNonExisting();
        }

    }

    public class CreateBomValueHeaderCommandHandler : IRequestHandler<CreateBomValueHeaderCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.BomValueHeader> _repository;
        private readonly IMapper _mapper;

        public CreateBomValueHeaderCommandHandler(IRepository<Data.Entities.BomValueHeader> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateBomValueHeaderCommand request, CancellationToken cancellationToken)
        {
            var bomValueHeader = _mapper.Map<Data.Entities.BomValueHeader>(request);
            if (!request.Values.Any())
            {
                throw new ValidationError("آیتم های فرمول ساخت وارد نشده است");
            }
            if (request.Values.Where(a=>a.BomWarehouseId==0).Any())
            {
                throw new ValidationError("انبارهای خروجی انتخاب نشده است");
            }
            if (request.Values.Where(a => a.UsedCommodityId == 0).Any())
            {
                throw new ValidationError("کالاهای خروجی انتخاب نشده است");
            }
            foreach (var items in request.Values)
            {

                var bomItem = new Eefa.Commodity.Data.Entities.BomValue();


                bomItem.Value = items.Value;
                bomItem.UsedCommodityId = items.UsedCommodityId;
                bomItem.BomWarehouseId = items.BomWarehouseId;
                //bomValueHeader.AddItem(bomItem);
            }

            var entity = _repository.Insert(bomValueHeader);

            if(await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();

        }
    }
}
