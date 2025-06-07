using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.BomValueHeader.Update
{
    public class UpdateBomValueHeaderCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Eefa.Commodity.Data.Entities.BomValueHeader>, ICommand
    {
        public int Id { get; set; }

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
        public List<BomValueUpdate> Values { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBomValueHeaderCommand, Data.Entities.BomValueHeader>()
                .IgnoreAllNonExisting();
        }
    }
    public class BomValueUpdate : IMapFrom<Eefa.Commodity.Data.Entities.BomValue>
    {

        public int Id { get; set; } = default!;
        /// <summary>
        /// کد سند فرمول ساخت
        /// </summary>
        public int BomValueHeaderId { get; set; } = default!;

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
            profile.CreateMap<BomValueUpdate, Eefa.Commodity.Data.Entities.BomValue>()
                .IgnoreAllNonExisting();
        }

    }

    public class UpdateBomValueHeaderCommandHandler : IRequestHandler<UpdateBomValueHeaderCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.BomValueHeader> _repository;
        private readonly IRepository<Data.Entities.BomValue> _bomValueRepository;
        private readonly IMapper _mapper;

        public UpdateBomValueHeaderCommandHandler(
            IRepository<Data.Entities.BomValueHeader> repository,
            IRepository<Data.Entities.BomValue> bomValueRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _bomValueRepository = bomValueRepository;
        }

        public async Task<ServiceResult> Handle(UpdateBomValueHeaderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);
            var BomItemsList = await _bomValueRepository.GetAll().Where(a => a.BomValueHeaderId == request.Id).ToListAsync();
            DeleteBomValueItems(request, BomItemsList);
            foreach (var items in request.Values)
            {
                //---------جدید
                if (items.Id == 0)
                {
                    var bomValue = new Eefa.Commodity.Data.Entities.BomValue();
                    bomValue.Value = items.Value;
                    bomValue.UsedCommodityId = items.UsedCommodityId;
                    bomValue.BomWarehouseId = items.BomWarehouseId;
                    entity.AddItem(bomValue);
                }
                //-----------ویرایش شده
                if (items.Id > 0)
                {
                    var model = BomItemsList.Where(a => a.Id == items.Id).FirstOrDefault();
                    _mapper.Map<BomValueUpdate, Data.Entities.BomValue>(items, model);
                    _bomValueRepository.Update(model);
                }
            }
            if (await _bomValueRepository.SaveChangesAsync() > 0)
            {
                _mapper.Map<UpdateBomValueHeaderCommand, Data.Entities.BomValueHeader>(request, entity);
                _repository.Update(entity);
                await _repository.SaveChangesAsync(cancellationToken);
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }

        private void DeleteBomValueItems(UpdateBomValueHeaderCommand request, List<Data.Entities.BomValue> BomItemsList)
        {
            var ListId = request.Values.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            var DeleteList = BomItemsList.Where(a => !ListId.Contains(a.Id)).ToList();
            //---------حذف شده
            foreach (var item in DeleteList)
            {

                _bomValueRepository.Delete(item);

            }
        }
    }
}
