using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Data.Databases.SqlServer.Context;
using Eefa.Accounting.Data.Events.Abstraction;
using Library.Common;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Services.EventManager
{
    public class ApplicationEventsManager : IApplicationEventsManager
    {
        private readonly IAccountingUnitOfWork unitOfWork;
        private readonly ICurrentUserAccessor currentUser;
        private readonly IConfigurationProvider configurationProvider;

        private List<ApplicationEvent> Events { get; set; } = new List<ApplicationEvent>();
        public ApplicationEventsManager(IAccountingUnitOfWork unitOfWork, ICurrentUserAccessor currentUser, IConfigurationProvider configurationProvider)
        {
            this.unitOfWork = unitOfWork;
            this.currentUser = currentUser;
            this.configurationProvider = configurationProvider;
        }


        public async Task CommitEvents(CancellationToken cancellationToken = default)
        {
          if(Events.Any())
            {
                foreach (var item in Events)
                {
                    item.EntityId = item.Entity.Id;
                    item.State = EventStates.Committed;
                }
                unitOfWork.DbContext().AddRange(Events);

                await unitOfWork.DbContext().SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<ApplicationEvent>> MapEventsToTheirRespectiveType(List<ApplicationEvent> events)
        {
            // TODO
            return events;
        }


        public ApplicationEvent AddEvent<T>(BaseEntity entity, T payload, string descriptions, Guid? originId = null)
        {
            var applicationEvent = new ApplicationEvent
            {
                Entity = entity,
                EntityType = entity.GetType().Name,
                Id = Guid.NewGuid(),
                OriginId = originId,
                Payload = payload,
                PayloadJSON = JsonConvert.SerializeObject(payload),
                CreatedAt = DateTime.UtcNow,
                CreatedById = currentUser.GetId(),
                PayloadType = payload.GetType().Name,
                Descriptions = descriptions,
                State = EventStates.Initialized,
            };

            this.Events.Add(applicationEvent);

            return applicationEvent;
        }



        public async Task<List<ApplicationEventViewModel>> GetEvents(int entityId,string entityType)
        {
            var query = unitOfWork.ApplicationEvents.AsQueryable().Where(x => x.EntityId == entityId && x.EntityType == entityType);

            var events = await query.Include(x => x.SubEvents).OrderByDescending(x => x.CreatedAt).ProjectTo<ApplicationEventViewModel>(configurationProvider).ToListAsync();

            foreach (var item in events)
            {
                item.SubEvents = item.SubEvents.OrderBy(x => x.CreatedAt).ToList();
            }
            return events;
        }
    }
}
