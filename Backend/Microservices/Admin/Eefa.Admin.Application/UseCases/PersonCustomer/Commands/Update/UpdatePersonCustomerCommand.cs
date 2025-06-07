using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.PersonCustomer.Models;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonCustomer.Commands.Update
{

    public class UpdatePersonCustomerCommand : CommandBase, IRequest<ServiceResult>,
        IMapFrom<Data.Databases.Entities.Customer>, ICommand
    {
        public int Id { get; set; }
        public int PersonId { get; set; } = default!;

        public int CustomerTypeBaseId { get; set; } = default!;

        public string CustomerCode { get; set; } = default!;

        public int CurentExpertId { get; set; } = default!;
        public string? EconomicCode { get; set; }
        public string? Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePersonCustomerCommand, Data.Databases.Entities.Customer>()
                .IgnoreAllNonExisting();
        }
    }

    public class UpdatePersonCustomerCommandHandler : IRequestHandler<UpdatePersonCustomerCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePersonCustomerCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(UpdatePersonCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Customer>(c =>
                    c.ObjectId(request.Id))
                .FirstOrDefaultAsync(cancellationToken);


            _mapper.Map(request, entity);
            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.Customer, PersonCustomerModel>(_repository, _mapper,
                updateEntity.Entity, cancellationToken);

        }
    }
}
