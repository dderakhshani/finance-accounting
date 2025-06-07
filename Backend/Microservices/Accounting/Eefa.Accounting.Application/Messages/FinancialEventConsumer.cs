using CacheManager.Core.Logging;
using DocumentFormat.OpenXml.Presentation;
using Eefa.Accounting.Application.UseCases.VouchersHead.Command.AutoVoucher;
using MassTransit;
using MassTransit.Internals.GraphValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedCode.Contracts.BursaryAccounting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Messages
{
    public class FinancialEventConsumer : IConsumer<FinancialEvent>
    {

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FinancialEventConsumer> _logger;
        public FinancialEventConsumer(IMediator mediator, IHttpContextAccessor httpContextAccessor, ILogger<FinancialEventConsumer> logger)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<FinancialEvent> context)
        {
            try
            {
                
                ProvideToken(); // inject token to httpcontext

                var request = JsonConvert.DeserializeObject<AutoVoucherRefactoredCommand>(context.Message.Payload);
                var deserializedList = new List<dynamic>();   
                foreach (var item in request.DataList)
                {
                    var deserializedItem = JsonConvert.DeserializeObject<JObject>(item.ToString());
                    deserializedList.Add(deserializedItem);
                }

                var command = new AutoVoucherRefactoredCommand
                {
                    DataList = deserializedList.Select(item => (dynamic)item).ToList(),
                   // VoucherHeadId = request.VoucherHeadId,
                };

                var result = await _mediator.Send(command);

                if (result.Succeed)
                {
                    await context.RespondAsync(new FinancialResponse
                    {
                        ObjResult = result.ObjResult,
                        Succeed = result.Succeed,
                        Message = result.Message,
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "مشکل در ثبت سند حسابداری", DateTime.Now);
               Console.WriteLine(ex.Message.ToString());
            }

        }

        private void ProvideToken()
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMzMSIsInVzZXJuYW1lIjoiZmFyaGFkIiwiZnVsbE5hbWUiOiIg2YHYsdmH2KfYryDYtdin2YTYrduMIiwiZ2VuZGVyIjoi2YXYsdivICIsInVzZXJBdmF0YXJSZWxldGl2ZUFkZHJlc3MiOiIiLCJicmFuY2hJZCI6IjEiLCJhY2NvdW50UmVmZXJlbmNlSWQiOiIyNzg0MCIsInJvbGVJZCI6IjIxODkiLCJ1c2VyUm9sZU5hbWUiOiLZhtix2YUg2KfZgdiy2KfYsSIsImxldmVsQ29kZSI6IjAwMTQiLCJjb21wYW55SWQiOiIxIiwieWVhcklkIjoiNSIsImxhbmd1YWdlSWQiOiIxIiwiY3VsdHVyZVR3b0lzb05hbWUiOiJlbiIsInJlZnJlc2hUb2tlbiI6IjRlQkVFU1NaY3NYYXQ3bWNiWm55YmlleFhub0N1N3g3Y1BjdFhKcTZzSDQ9IiwiZXhwIjoxNzcyNzE3MDIwLCJpc3MiOiJlZWZhY2VyYW1PYXV0aFNlcnZlciIsImF1ZCI6ImVlZmFjZXJhbU9hdXRoU2VydmVyIn0.V29Bnfw22L7LoTguCyxxyJmRWUoG5R_7Jrksz8LyvyA");
            var claims = token.Claims;
            var identity = new ClaimsIdentity(claims, "Bearer");
            var principal = new ClaimsPrincipal(identity);

            // شبیه‌سازی HttpContext
            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            _httpContextAccessor.HttpContext = httpContext;

        }

        public class BursaryDocumentModel
        {
            public string DocumentNo { get; set; }
            public int DocumentId { get; set; }
            public DateTime DocumentDate { get; set; }
            public int CodeVoucherGroupId { get; set; }
            public int DebitAccountHeadId { get; set; }
            public int? DebitAccountReferenceGroupId { get; set; }
            public int? DebitAccountReferenceId { get; set; }
            public int CreditAccountHeadId { get; set; }
            public int? CreditAccountReferenceGroupId { get; set; }
            public int? CreditAccountReferenceId { get; set; }
            public decimal Amount { get; set; }
            public int DocumentTypeBaseId { get; set; }
            public string SheetUniqueNumber { get; set; }
            public decimal? CurrencyFee { get; set; }
            public decimal? CurrencyAmount { get; set; }
            public int? CurrencyTypeBaseId { get; set; }
            public int NonRialStatus { get; set; }
            public int? ChequeSheetId { get; set; }
            public bool IsRial { get; set; }
            public string ReferenceName { get; set; }
            public string ReferenceCode { get; set; }
            public string Description { get; set; }
            public decimal TotalAmount { get; set; } = 0;
            public int GroupCode { get; set; } = 0;
            public int VoucherHeadId { get; set; } = default;

        }

    }


}

 
