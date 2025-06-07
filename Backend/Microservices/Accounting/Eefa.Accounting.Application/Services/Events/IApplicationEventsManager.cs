using Eefa.Accounting.Data.Events.Abstraction;
using Library.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.Services.EventManager
{
    public interface IApplicationEventsManager
    {
        ApplicationEvent AddEvent<T>(BaseEntity entity, T payload, string description, Guid? originId = null);
        Task CommitEvents(CancellationToken cancellationToken = default);
        Task<List<ApplicationEvent>> MapEventsToTheirRespectiveType(List<ApplicationEvent> events);
        Task<List<ApplicationEventViewModel>> GetEvents(int entityId, string entityType);
    }
}