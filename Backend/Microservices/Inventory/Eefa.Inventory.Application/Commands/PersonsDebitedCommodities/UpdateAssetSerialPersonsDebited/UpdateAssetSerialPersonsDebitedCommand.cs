using System;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Eefa.Common.Exceptions;
using System.Collections.Generic;
using Eefa.Invertory.Infrastructure.Repositories;

namespace Eefa.Inventory.Application
{


    public class UpdateAssetSerialPersonsDebitedCommand : CommandBase, IRequest<ServiceResult<PersonsDebitedCommoditiesModel>>, IMapFrom<Domain.PersonsDebitedCommodities>, ICommand
    {
        public int Id { get; set; } = default!;
        public int AssetId { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAssetSerialPersonsDebitedCommand, Domain.PersonsDebitedCommodities>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateAssetSerialPersonsDebitedCommandHandler : IRequestHandler<UpdateAssetSerialPersonsDebitedCommand, ServiceResult<PersonsDebitedCommoditiesModel>>
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IAssetsRepository _assetsRepository;
        private readonly IPersonsDebitedCommoditiesRepository _PersonsDebitedCommoditiesRepository;

        public UpdateAssetSerialPersonsDebitedCommandHandler(
            IMapper mapper,
            IInvertoryUnitOfWork context,
            
            IAssetsRepository assetsRepository,
            IPersonsDebitedCommoditiesRepository PersonsDebitedCommoditiesRepository

            )
        {
            _mapper = mapper;
            _context = context;
            _assetsRepository = assetsRepository;
            _PersonsDebitedCommoditiesRepository = PersonsDebitedCommoditiesRepository;

        }

        public async Task<ServiceResult<PersonsDebitedCommoditiesModel>> Handle(UpdateAssetSerialPersonsDebitedCommand request, CancellationToken cancellationToken)
        {
            
            var entity = await _PersonsDebitedCommoditiesRepository.Find(request.Id);

            var Asset_old = await _assetsRepository.Find(Convert.ToInt32(entity.AssetId));
            Asset_old.IsActive = true;
            
            entity.AssetId = request.AssetId;


            _PersonsDebitedCommoditiesRepository.Update(entity);

            //-----------------------------------------------------------------------
            var Asset_new = await _assetsRepository.Find(request.AssetId);

            Asset_new.IsActive = false;

           
            _assetsRepository.Update(Asset_new);
            _assetsRepository.Update(Asset_old);

            //-------------------------------------------------------------------------
            await _PersonsDebitedCommoditiesRepository.SaveChangesAsync(cancellationToken);

            return ServiceResult<PersonsDebitedCommoditiesModel>.Success(_mapper.Map<PersonsDebitedCommoditiesModel>(entity));
        }
        
    }
}
