using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Eefa.Common.Exceptions;

namespace Eefa.Commodity.Application.Commands.Bom.Update
{
    public class UpdateBomCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.Bom>, ICommand
    {
        public int Id { get; set; }

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
        public List<BomItemsUpdate> BomItems { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBomCommand, Data.Entities.Bom>()
                .IgnoreAllNonExisting();
        }
    }

    public class BomItemsUpdate : IMapFrom<Eefa.Commodity.Data.Entities.BomItem>

    {
        public int Id { get; set; } = default!;
        public int BomId { get; set; } = default!;
        public int SubCategoryId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<BomItemsUpdate, Eefa.Commodity.Data.Entities.BomItem>()
                .IgnoreAllNonExisting();
        }


    }
    public class UpdateBomCommandHandler : IRequestHandler<UpdateBomCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.Bom> _repository;
        private readonly IRepository<Data.Entities.BomItem> _bomItemRepository;

        private readonly IMapper _mapper;

        public UpdateBomCommandHandler(
            IRepository<Data.Entities.Bom> repository,
            IRepository<Data.Entities.BomItem> bomItemRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _bomItemRepository = bomItemRepository;
        }

        public async Task<ServiceResult> Handle(UpdateBomCommand request, CancellationToken cancellationToken)
        {
            if (!request.BomItems.Any())
            {
                throw new ValidationError("آیتم های فرمول ساخت وارد نشده است");
            }
            var bom = await _repository.Find(request.Id);

            var BomItemsList = await _bomItemRepository.GetAll().Where(a => a.BomId == request.Id).ToListAsync();
            //-------حذف
            DeleteBomstems(request, BomItemsList);
            foreach (var items in request.BomItems)
            {
                //---------جدید
                if (items.Id == 0)
                {
                    var bomItem = new Eefa.Commodity.Data.Entities.BomItem();

                    bomItem.SubCategoryId = items.SubCategoryId;
                    bomItem.CommodityId = items.CommodityId;
                    //bom.AddItem(bomItem);
                }
                //-----------ویرایش شده
                if (items.Id > 0)
                {
                    var model = BomItemsList.Where(a => a.Id == items.Id).FirstOrDefault();
                    model.CommodityId = items.CommodityId;
                   
                    _bomItemRepository.Update(model);
                    
                }



            }
            if (await _bomItemRepository.SaveChangesAsync() > 0)
            {
                _mapper.Map<UpdateBomCommand, Data.Entities.Bom>(request, bom);
                _repository.Update(bom);
                await _repository.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success();
            }
            return ServiceResult.Failed();


        }

        private void DeleteBomstems(UpdateBomCommand request, List<Data.Entities.BomItem> BomItemsList)
        {
            var ListId = request.BomItems.Where(a => a.Id > 0).Select(a => a.Id).ToList();
            var DeleteList = BomItemsList.Where(a => !ListId.Contains(a.Id)).ToList();
            //---------حذف شده
            foreach (var item in DeleteList)
            {

                _bomItemRepository.Delete(item);

            }
        }
    }
}
