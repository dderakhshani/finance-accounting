using Eefa.Admin.Data.Databases.Entities;
using Eefa.Admin.Data.Databases.SqlServer.Context;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.UseCases.Person.Command.Update
{
    public class UpdateDepositPersonCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public List<string> AccountReferences { get; set; }

        public class UpdateDepositPersonCommandHandler : IRequestHandler<UpdateDepositPersonCommand, ServiceResult>
        {
            private readonly IRepository _repository;
            private readonly ITejaratBankServices _tejaratBankServices;
            private readonly IAdminUnitOfWork _adminUnitOfWork;
            public UpdateDepositPersonCommandHandler(IRepository repository, ITejaratBankServices tejaratBankServices, IAdminUnitOfWork adminUnitOfWork)
            {
                _repository = repository;
                _tejaratBankServices = tejaratBankServices;
                _adminUnitOfWork = adminUnitOfWork;
            }

            public async Task<ServiceResult> Handle(UpdateDepositPersonCommand request, CancellationToken cancellationToken)
            {
 
                var references = await (from a in _repository.GetQuery<AccountReference>()
                                where request.AccountReferences.Contains(a.Code)
                                select a).ToListAsync(cancellationToken);


                foreach (var reference in references)
                {
                    var code = reference.Code;
                    if (reference.Code.Length == 6)
                        code = reference.Code + "0";

                    reference.DepositId = _tejaratBankServices.SetDepositId(code);
                    _repository.Update(reference);
                }
                await _repository.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success(true);

            }

        }

    }
}
