using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class DeleteWarehouseLayoutCategoriesCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteWarehouseLayoutCategoriesCommandHandler : IRequestHandler<DeleteWarehouseLayoutCategoriesCommand, ServiceResult>
    {
        private readonly IRepository<WarehouseLayoutCategories> _WarehouseLayoutCategoriesRepository;
        private readonly IRepository<Domain.WarehouseLayoutProperty>  _WarehouseLayoutPropertyRepository;
        private readonly IWarehouseLayoutRepository _warehouseLayoutRepository;
        private readonly IMapper _mapper;

        public DeleteWarehouseLayoutCategoriesCommandHandler(

            IRepository<WarehouseLayoutCategories> WarehouseLayoutCategoriesRepository,
            IRepository<Domain.WarehouseLayoutProperty> WarehouseLayoutPropertyRepository,
            IWarehouseLayoutRepository warehouseLayoutRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _WarehouseLayoutCategoriesRepository = WarehouseLayoutCategoriesRepository;
            _WarehouseLayoutPropertyRepository = WarehouseLayoutPropertyRepository;
            _warehouseLayoutRepository = warehouseLayoutRepository;
        }

        public async Task<ServiceResult> Handle(DeleteWarehouseLayoutCategoriesCommand request, CancellationToken cancellationToken)
        {
            var entity = await _WarehouseLayoutCategoriesRepository.Find(request.Id);
            var WarehouseloyatId=entity.WarehouseLayoutId;
            var categoryLevelCode = _warehouseLayoutRepository.GetAll().Where(a => a.Id == WarehouseloyatId).Select(a => a.LevelCode).FirstOrDefault();
            var warehouseLayout = _warehouseLayoutRepository.GetAll().Where(a => !a.IsDeleted &&
                                                                          (categoryLevelCode == null ||
                                                                          (a.LevelCode.Length >= categoryLevelCode.Length &&
                                                                          (a.LevelCode.Substring(0, categoryLevelCode.Length) == categoryLevelCode)))).ToList();


           var Categories = _WarehouseLayoutCategoriesRepository.GetAll().ToList().Where(x =>  warehouseLayout.Any(par => x.WarehouseLayoutId == par.Id)).ToList();
           var Property =_WarehouseLayoutPropertyRepository.GetAll().ToList().Where(x => Categories.Any(par => x.WarehouseLayoutCategoryId== par.Id)).ToList();
            Property.ForEach(a =>
            {
                _WarehouseLayoutPropertyRepository.Delete(a);
               
            });
            Categories.ForEach(a =>
            {

                _WarehouseLayoutCategoriesRepository.Delete(a);
                

            });

           

            if (await _WarehouseLayoutCategoriesRepository.SaveChangesAsync(cancellationToken) > 0)
            {   
                
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
    }
}
