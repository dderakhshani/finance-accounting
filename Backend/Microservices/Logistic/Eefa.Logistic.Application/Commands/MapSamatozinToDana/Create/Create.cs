using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Exceptions;
using Eefa.Logistic.Domain;
using Eefa.Logistic.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Logistic.Application
{
    public class CreateMapSamatozinToDanaCommand : CommandBase, IRequest<ServiceResult<MapSamatozinToDana>>, IMapFrom<CreateMapSamatozinToDanaCommand>, ICommand
    {

        public int  AccountReferenceId  { get; set; } = default!;
        public string AccountReferenceCode { get; set; } = default!;
       
        public string SamaTozinCode { get; set; } = default!;
        public string SamaTozinTitle { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMapSamatozinToDanaCommand, MapSamatozinToDana>()
                .IgnoreAllNonExisting();
        }

    }
   
    public class CreateMapSamatozinToDanaCommandHandler : IRequestHandler<CreateMapSamatozinToDanaCommand, ServiceResult<MapSamatozinToDana>>
    {
        private readonly IMapper _mapper;
        private readonly LogisticContext _context;
        private readonly IMapSamatozinToDanaRepository _mapSamatozinToDanaRepository;

        public CreateMapSamatozinToDanaCommandHandler(
              IMapper mapper
            , LogisticContext context
            , IMapSamatozinToDanaRepository mapSamatozinToDanaRepository

            )
        {
            _mapper = mapper;
            _context = context;
            _mapSamatozinToDanaRepository = mapSamatozinToDanaRepository;

        }

        public async Task<ServiceResult<MapSamatozinToDana>> Handle(CreateMapSamatozinToDanaCommand request, CancellationToken cancellationToken)
        {
            var dana = _mapper.Map<MapSamatozinToDana>(request);

            var entity = _mapSamatozinToDanaRepository.Insert(dana);

            await _mapSamatozinToDanaRepository.SaveChangesAsync(cancellationToken);

            return ServiceResult<MapSamatozinToDana>.Success(_mapper.Map<MapSamatozinToDana>(dana));
        }


        

    }
}