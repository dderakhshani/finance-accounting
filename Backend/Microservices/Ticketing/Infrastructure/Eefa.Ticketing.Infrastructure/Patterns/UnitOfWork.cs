using Eefa.Common;
using Eefa.Ticketing.Domain.Core;
using Eefa.Ticketing.Domain.Core.Interfaces.BaseInfo;
using Eefa.Ticketing.Domain.Core.Interfaces.Tickets;
using Eefa.Ticketing.Infrastructure.Database.Context;
using Eefa.Ticketing.Infrastructure.Repositories.BaseInfo;
using Eefa.Ticketing.Infrastructure.Repositories.Tickets;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Infrastructure.Patterns
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EefaTicketingContext context;
        private readonly IServiceProvider _serviceProvider;
        protected readonly ICurrentUserAccessor _currentUserAccessor;
        public UnitOfWork(EefaTicketingContext context, IServiceProvider serviceProvider, ICurrentUserAccessor currentUserAccessor)
        {
            this.context = context;
            _serviceProvider = serviceProvider;
            _currentUserAccessor = currentUserAccessor;
        }

        private TicketRepository ticketRepository;
        private TicketDetailRepository ticketDetailRepository;
        private PrivetMessageRepository privetMessageRepository;
        private DetailHistoryRepository detailHistoryRepository;
        private BaseInfoRepository baseInfoRepository;

        public TicketRepository TicketRepository
        {
            get
            {
                if (ticketRepository == null)
                {
                    ticketRepository = (TicketRepository)_serviceProvider.GetService<ITicketRepository>();
                }

                return ticketRepository;
            }
        }
        public TicketDetailRepository TicketDetailRepository
        {
            get
            {
                if (ticketDetailRepository == null)
                {
                    ticketDetailRepository = (TicketDetailRepository)_serviceProvider.GetService<ITicketDetailRepository>();
                }
                //using (var scope = _serviceProvider.CreateScope())
                //{
                //    ticketDetailRepository = (TicketDetailRepository)scope.ServiceProvider.GetRequiredService<ITicketDetailRepository>();
                //}
                return ticketDetailRepository;
            }
        }
        public PrivetMessageRepository PrivetMessageRepository
        {
            get
            {
                if (privetMessageRepository == null)
                {
                    privetMessageRepository = (PrivetMessageRepository)_serviceProvider.GetService<IPrivetMessageRepository>();
                }
                return privetMessageRepository;
            }
        }
        public DetailHistoryRepository DetailHistoryRepository
        {
            get
            {
                if (detailHistoryRepository == null)
                {
                    detailHistoryRepository = (DetailHistoryRepository)_serviceProvider.GetService<IDetailHistoryRepository>();
                }
                return detailHistoryRepository;
            }
        }


        public BaseInfoRepository BaseInfoRepository
        {
            get
            {
                if (baseInfoRepository == null)
                {
                    baseInfoRepository = (BaseInfoRepository)_serviceProvider.GetService<IBaseInfoRepository>();
                }
                return baseInfoRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            var ModifiedEntities = context.ChangeTracker.Entries().Where(a => a.State == Microsoft.EntityFrameworkCore.EntityState.Modified).Select(a => a.Entity as BaseEntity).ToList();
            var CreatedEntities = context.ChangeTracker.Entries().Where(a => a.State == Microsoft.EntityFrameworkCore.EntityState.Added).Select(a => a.Entity as BaseEntity).ToList();
            foreach (var entity in ModifiedEntities)
            {
                entity.ModifiedAt = DateTime.Now;
                entity.ModifiedById = _currentUserAccessor.GetId();
                entity.IsDeleted = false;
            }
            foreach (var entity in CreatedEntities)
            {
                entity.ModifiedAt = DateTime.UtcNow;
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedById = _currentUserAccessor.GetId();
                entity.ModifiedById = _currentUserAccessor.GetId();
                entity.IsDeleted = false;
                entity.OwnerRoleId = _currentUserAccessor.GetRoleId();
            }
            await context.SaveChangesAsync();
        }


        #region disposed

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
