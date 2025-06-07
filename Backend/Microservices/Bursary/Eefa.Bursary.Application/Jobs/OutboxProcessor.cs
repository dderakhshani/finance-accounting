using Eefa.Bursary.Application.Events;
using Eefa.Bursary.Application.Interfaces;
using Eefa.Common;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Jobs
{
    public class OutboxProcessor: BackgroundService
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRequestClient<FinancialEvent> _requestClient;

        public OutboxProcessor(IServiceScopeFactory serviceScopeFactory, IRequestClient<FinancialEvent> requestClient)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _requestClient = requestClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var unprocessedMessages = await outboxRepository.GetUnprocessedMessagesAsync(stoppingToken);

                foreach (var message in unprocessedMessages)
                {
                    try
                    {
                        var financialEvent = new FinancialEvent
                        {
                            EventType = message.EventType,
                            Payload = message.Payload
                        };

                        var response = await _requestClient.GetResponse<FinancialResponse>(financialEvent, stoppingToken);

                        await outboxRepository.MarkAsProcessedAsync(message.Id,  3, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message {message.Id}: {ex.Message}");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(10),, stoppingToken);  
            }
        }

    }
}
