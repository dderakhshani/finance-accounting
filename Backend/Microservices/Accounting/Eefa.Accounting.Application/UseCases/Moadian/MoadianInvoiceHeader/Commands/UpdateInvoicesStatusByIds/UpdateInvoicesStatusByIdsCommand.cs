using AutoMapper;
using Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceDetail.Commands.Create;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Moadian.MoadianInvoiceHeader.Commands.UpdateInvoicesStatusByIds
{
    public class UpdateInvoicesStatusByIdsCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<UpdateInvoicesStatusByIdsCommand>, ICommand
    {
        public List<int> Ids { get; set; }
        public string Status { get; set; } = default!;

        public List<UpdateMoadianInvoiceDetailCommand> MoadianInvoiceDetails { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateInvoicesStatusByIdsCommand, Data.Entities.MoadianInvoiceHeader>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateInvoicesStatusByIdsCommandHandler : IRequestHandler<UpdateInvoicesStatusByIdsCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public UpdateInvoicesStatusByIdsCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(UpdateInvoicesStatusByIdsCommand request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAll<Data.Entities.MoadianInvoiceHeader>()
                                          .Where(x => request.Ids.Contains(x.Id))
                                          .ToListAsync(cancellationToken);
            foreach (var entity in entities)
            {
                entity.Status = request.Status;
            }

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success(true);
        }
    }
}
