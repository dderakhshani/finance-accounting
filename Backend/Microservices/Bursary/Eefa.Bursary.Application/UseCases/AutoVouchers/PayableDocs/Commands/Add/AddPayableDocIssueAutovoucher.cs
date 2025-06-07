using Dapper;
using Eefa.Bursary.Application.UseCases.AutoVouchers.GeneralAV.Commands.Add;
using Eefa.Bursary.Application.UseCases.AutoVouchers.GeneralAV.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Web;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.AutoVouchers.PayableDocs.Commands.Add
{
    public class AddPayableDocIssueAutovoucher : CommandBase, IRequest<ServiceResult<VouchersHead>>, ICommand
    {
        public int DocumentId { get; set; }
        public int OperationId { get; set; }
    }

    public class AddPayableDocIssueAutovoucherHandler : IRequestHandler<AddPayableDocIssueAutovoucher, ServiceResult<VouchersHead>>
    {
        private readonly IConfigurationAccessor _config;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMediator _mediator;
        private readonly IBursaryUnitOfWork _uow;

        public AddPayableDocIssueAutovoucherHandler(IConfigurationAccessor configuration, ICurrentUserAccessor currentUserAccessor, IMediator mediator, IConfigurationAccessor config, IBursaryUnitOfWork uow)
        {
            _config = configuration;
            _currentUserAccessor = currentUserAccessor;
            _mediator = mediator;
            _config = config;
            _uow = uow;
        }

        public async Task<ServiceResult<VouchersHead>> Handle(AddPayableDocIssueAutovoucher request, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("DocumentId", request.DocumentId);
            parameters.Add("OperationId", request.OperationId);

            using (var db = new SqlConnection(_config.GetConnectionString().DefaultString))
            {
                var reslist = await db.QueryAsync<dynamic>(sql: "av.Payable_Autovoucher_dataList_Get", commandType: CommandType.StoredProcedure, param: parameters, commandTimeout: 100);
                if (reslist != null && reslist.Count() > 0)
                {
                    var vchcmd = new AddAutoVoucherCommand
                    {
                        dataList = reslist.ToList()
                    };

                    var resvchcmd = await _mediator.Send(vchcmd, cancellationToken);
                    if (resvchcmd == null || !resvchcmd.Succeed)
                    {
                        return ServiceResult<VouchersHead>.Failed();
                    }
                }
            }

            var docop = await _uow.Payables_DocumentsOperations.FirstOrDefaultAsync(x => x.DocumentId == request.DocumentId && x.OperationId == request.OperationId);
            if (docop == null)
            {
                return ServiceResult<VouchersHead>.Failed();
            }
            var row = await _uow.VouchersDetail.Include(x => x.Voucher).FirstOrDefaultAsync(x => x.Voucher.CodeVoucherGroupId == 2372 && x.DocumentId == docop.Id);
            if (row == null)
            {
                return ServiceResult<VouchersHead>.Failed();
            }

            return ServiceResult<VouchersHead>.Success(row.Voucher);
        }
    }

}
