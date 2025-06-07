using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Application.UseCases.VouchersHead.Model;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Query.GetNo
{
    public class GetVouchersHeadNoByDateQuery : IRequest<ServiceResult>
    {
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
    }
    public class GetVouchersHeadNoByDateQueryHandler : IRequestHandler<GetVouchersHeadNoByDateQuery, ServiceResult>
    {
        private readonly IAccountingUnitOfWork _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public GetVouchersHeadNoByDateQueryHandler(ICurrentUserAccessor currentUserAccessor, IAccountingUnitOfWork context)
        {
            _currentUserAccessor = currentUserAccessor;
            _context = context;
        }

        public async Task<ServiceResult> Handle(GetVouchersHeadNoByDateQuery request, CancellationToken cancellationToken)
        {
            var minno = await _context.VouchersHeads.Where(a => a.VoucherDate >= request.FromDateTime && a.VoucherDate <= request.ToDateTime).MinAsync(a => a.VoucherNo);
            var maxno = await _context.VouchersHeads.Where(a => a.VoucherDate >= request.FromDateTime && a.VoucherDate <= request.ToDateTime).MaxAsync(a => a.VoucherNo);
            GetNoResultModel resultModel = new(minno, maxno);
            return ServiceResult.Success(resultModel);
        }
    }
}
