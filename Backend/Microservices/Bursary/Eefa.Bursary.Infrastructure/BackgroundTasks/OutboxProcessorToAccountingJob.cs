using Eefa.Bursary.Application.Interfaces;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Quartz.Logging;
using SharedCode.Contracts.BursaryAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure.BackgroundTasks
{
    public class OutboxProcessorToAccountingJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRequestClient<FinancialEvent> _requestClient;
        private readonly ILogger<OutboxProcessorToAccountingJob> _logger;


        public OutboxProcessorToAccountingJob(IServiceScopeFactory serviceScopeFactory, IRequestClient<FinancialEvent> requestClient, ILogger<OutboxProcessorToAccountingJob> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _requestClient = requestClient;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

            using var scope = _serviceScopeFactory.CreateScope();
            var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
            var unprocessedMessages = await outboxRepository.GetUnprocessedMessagesAsync(CancellationToken.None);

            foreach (var message in unprocessedMessages)
            {
                try
                {
                    var financialEvent = new FinancialEvent
                    {
                        EventType = message.EventType,
                        Payload =  message.Payload 
                    };

                    var response = await _requestClient.GetResponse<FinancialResponse>(financialEvent, CancellationToken.None);
                        if (response.Message.Succeed)
                        {
                            string jsonString = response.Message.ObjResult.ToString();

                            var voucher = JsonConvert.DeserializeObject<List<ResponseVoucherModel>>(jsonString);
                            await outboxRepository.MarkAsProcessedAsync(message.Id, voucher[0].VoucherHeadId, CancellationToken.None);
                        }
                        else
                        {
                            _logger.LogError(response.Message.Message, "ثبت سند به وسیله کیو مشکل در ***** ", DateTime.Now);
                            throw new Exception("error");
                        }
                    }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "***** OutboxProcessorToAccountingJob مشکل در ", DateTime.Now);
                    Console.WriteLine($"Error processing message {message.Id}: {ex.Message}");
                        throw;

                    }
                }
            }catch(Exception ex)
            {
                _logger.LogError(ex, "***** OutboxProcessorToAccountingJob مشکل در ", DateTime.Now);
                Console.WriteLine(ex);
                throw;

            }
        }

        public class ResponseVoucherModel
        {
            public int VoucherHeadId { get; set; }
            public int DocumentId { get; set; }
            public int VoucherNo { get; set; }
        }

    }

}
