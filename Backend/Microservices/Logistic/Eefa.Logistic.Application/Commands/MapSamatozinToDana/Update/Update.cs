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
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Logistic.Application
{
    public class UpdateMapSamatozinToDanaCommand : CommandBase, IRequest<ServiceResult<MapSamatozinToDana>>, IMapFrom<UpdateMapSamatozinToDanaCommand>, ICommand
    {
        public int Id { get; set; } = default!;
        public int AccountReferenceId { get; set; } = default!;
        public string AccountReferenceCode { get; set; } = default!;
        public string DanaCode { get; set; } = default!;
        public string SamaTozinCode { get; set; } = default!;
        public string SamaTozinTitle { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateMapSamatozinToDanaCommand, MapSamatozinToDana>()
                .IgnoreAllNonExisting();
        }
    }

    public class UpdateMapSamatozinToDanaCommandHandler : IRequestHandler<UpdateMapSamatozinToDanaCommand, ServiceResult<MapSamatozinToDana>>
    {
        private readonly IRepository<MapSamatozinToDana> _repository;
        private readonly IMapper _mapper;

        public UpdateMapSamatozinToDanaCommandHandler(IRepository<MapSamatozinToDana> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult<MapSamatozinToDana>> Handle(UpdateMapSamatozinToDanaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            _mapper.Map<UpdateMapSamatozinToDanaCommand, MapSamatozinToDana>(request, entity);

            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            var model = _mapper.Map<MapSamatozinToDana>(entity);
            return ServiceResult<MapSamatozinToDana>.Success(model);
        }
    }
}
