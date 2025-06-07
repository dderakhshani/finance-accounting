using AutoMapper;
using Eefa.Bursary.Application.Models;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data.Query;
using MediatR;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Create
{
    public class CreateDocumentForBursaryArticlesCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult>, IMapFrom<CreateDocumentForBursaryArticlesCommand>, ICommand
    {
        public List<JObject> DataList { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateDocumentForBursaryArticlesCommand, Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>()
                .IgnoreAllNonExisting();
        }

        public class CreateDocumentForBursaryArticlesCommandHandler : IRequestHandler<CreateDocumentForBursaryArticlesCommand, ServiceResult>
        {
            private readonly IMapper _mapper;
            private readonly IBursaryUnitOfWork _bursaryUnitOfWork;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IApplicationLogs _applicationLogs;
            public CreateDocumentForBursaryArticlesCommandHandler(IMapper mapper, IBursaryUnitOfWork bursaryUnitOfWork, ICurrentUserAccessor currentUserAccessor, IApplicationLogs applicationLogs)
            {
                _mapper = mapper;
                _bursaryUnitOfWork = bursaryUnitOfWork;
                _currentUserAccessor = currentUserAccessor;
                _applicationLogs = applicationLogs;
            }

            public async Task<ServiceResult> Handle(CreateDocumentForBursaryArticlesCommand request, CancellationToken cancellationToken)
            {

                await _applicationLogs.CommitLog(request);


                string financialRequestIds = "";
                foreach (var item in request.DataList)
                {
                    var docModel = JsonSerializer.Deserialize<AccountingDocument>(item.ToString());
                    financialRequestIds += "," + docModel.DocumentId;
                }
                 
                financialRequestIds = financialRequestIds.Substring(0);
                
                var model = new SpCheckVoucherIdInFinancialRequestAndUpdateVoucherIdInFinancialRequestParam() { FinancialRequestIds=financialRequestIds };
                var parameters = model.EntityToSqlParameters();

                await _bursaryUnitOfWork.ExecuteSqlQueryAsync<SpCheckVoucherIdInFinancialRequestAndUpdateVoucherIdInFinancialRequestParam>($"EXEC [bursary].[CheckVoucherIdInFinancialRequestAndUpdateVoucherIdInFinancialRequest] {Eefa.Common.Data.Query.QueryUtility.SqlParametersToQuey(parameters)}", parameters, new CancellationToken());



                return ServiceResult.Success();



            }
        }
    }
}
