using Eefa.Bursary.Application.Commands.CustomerReceipt.Create;
using Eefa.Bursary.Application.Commands.CustomerReceipt.Update;
using Eefa.Bursary.Application.Interfaces;
using Eefa.Bursary.Application.Models;
using MassTransit;
using MassTransit.Clients;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Eefa.Bursary.Infrastructure.BackgroundTasks.OutboxProcessorToAccountingJob;

namespace Eefa.Bursary.Infrastructure.BackgroundTasks
{
    public class OutboxProcessorToBursuryJob:IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMediator _mediator;
        private readonly ILogger<OutboxProcessorToBursuryJob> _logger;
        public OutboxProcessorToBursuryJob(IServiceScopeFactory serviceScopeFactory, IMediator mediator, ILogger<OutboxProcessorToBursuryJob> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var unprocessedMessages = await outboxRepository.GetProcessedMessagesByAccountingAsync(CancellationToken.None);

                foreach (var message in unprocessedMessages)
                {
                    try
                    {
                        var document = JsonConvert.DeserializeObject<SendDocument<DataListModel>>(message.Payload);
                        var voucherHeadId = message.VoucherHeadId;

                        var ids = new List<int>();
                        ids.Add(document.dataList[0].DocumentId);

                        var updateCommand = new UpdateVoucherHeadIdCommand()
                        {
                            ReceiveIds = ids,
                            VoucherHeadId = (int)voucherHeadId,
                            TransactionId = message.Id
                        };


                        var result = await _mediator.Send(updateCommand);
                       
                        if(result.Succeed)
                            await outboxRepository.MarkAsProcessedByBursaryAsync(message.Id,CancellationToken.None);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing message {message.Id}: {ex.Message} *****", DateTime.Now);

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} *****", DateTime.Now);

            }



        }
    }
}
