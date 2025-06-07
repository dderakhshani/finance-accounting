using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Library.Interfaces;
using Library.Models;
using Library.Mappings;
using AutoMapper;
using Eefa.Accounting.Data.Databases.Sp;

namespace Eefa.Accounting.Application.UseCases.Report
{
    public class AccountReferenceCodeBookRemainQuery : IMapFrom<AccountReferenceCodeBookRemainQuery>, IRequest<ServiceResult>, IQuery
    {
        public string AccountHeadCode { get; set; }
        public int AccountReferenceId { get; set; }
        public string? ReportTitle { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountReferenceCodeBookRemainQuery, StpAccountReferenceBookInput>()
                .IgnoreAllNonExisting();
        }
    }


    public class AccountReferenceCodeBookRemainQueryHandler : IRequestHandler<AccountReferenceCodeBookRemainQuery,ServiceResult>
    {
        private readonly IRepository _repository;
        public AccountReferenceCodeBookRemainQueryHandler(IRepository _repository)
        {
            this._repository = _repository;
        }

        public async Task<ServiceResult> Handle(AccountReferenceCodeBookRemainQuery request, CancellationToken cancellationToken)
        {
            var currentYear = await _repository.Find<Data.Entities.Year>(x => x.ConditionExpression(x => x.IsCurrentYear == true)).FirstOrDefaultAsync();

            var accountHead = await _repository.Find<Data.Entities.AccountHead>(x => x.ConditionExpression(x  => x.Code == request.AccountHeadCode)).FirstOrDefaultAsync();

            var sum  = await _repository.GetAll<Data.Entities.VouchersDetail>(x => x.ConditionExpression(x => x.AccountHeadId == accountHead.Id && x.ReferenceId1 == request.AccountReferenceId)).SumAsync(x => x.Credit - x.Debit);

            return ServiceResult.Success(sum);
        }
    }
}
