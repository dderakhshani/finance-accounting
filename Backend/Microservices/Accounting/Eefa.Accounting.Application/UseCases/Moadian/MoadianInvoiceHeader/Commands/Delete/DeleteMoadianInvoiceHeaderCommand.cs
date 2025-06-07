using AutoMapper;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.Delete
{
    public class DeleteMoadianInvoiceHeaderCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<DeleteMoadianInvoiceHeaderCommand>, ICommand
    {
        public List<int> InvoiceIds { get; set; }

        public bool IsProduction { get; set; } = false;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteMoadianInvoiceHeaderCommand, Data.Entities.MoadianInvoiceHeader>()
                .IgnoreAllNonExisting();
        }
    }


    public class DeleteMoadianInvoiceHeaderCommandHandler : IRequestHandler<DeleteMoadianInvoiceHeaderCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public DeleteMoadianInvoiceHeaderCommandHandler(IMapper mapper, IRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<ServiceResult> Handle(DeleteMoadianInvoiceHeaderCommand request, CancellationToken cancellationToken)
        {
            var entities = await _repository
                .GetAll<Data.Entities.MoadianInvoiceHeader>()
                .Where(x => x.IsSandbox == true & (x.TaxId == null || x.TaxId.StartsWith(MoadianConstants.SandboxProtectorId)))
                .Include(x => x.MoadianInvoiceDetails)
             .ToListAsync(cancellationToken);

            foreach (var entity in entities)
            {
                _repository.Delete(entity);
                foreach (var detail in entity.MoadianInvoiceDetails)
                {
                    _repository.Delete(detail);
                }
            }

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success(_mapper.Map<List<MoadianInvoiceHeaderDetailedModel>>(entities));
        }
    }
}
