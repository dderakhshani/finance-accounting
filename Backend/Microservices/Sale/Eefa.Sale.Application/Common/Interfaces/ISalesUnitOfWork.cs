using Eefa.Common.Data;
using Eefa.Sale.Domain.Aggregates;
using Eefa.Sale.Domain.Aggregates.CustomerAggregate;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Sale.Application.Common.Interfaces
{
    public interface ISalesUnitOfWork : IUnitOfWork
    {
        DbSet<AccountReferences> AccountReferences { get; set; }
        DbSet<AccountReferencesGroups> AccountReferencesGroups { get; set; }
        DbSet<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; }
        DbSet<BaseValueTypes> BaseValueTypes { get; set; }
        DbSet<BaseValues> BaseValues { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<PersonPhones> PersonPhones { get; set; }
        DbSet<Person> Persons { get; set; }
        DbSet<SalesAgents> SalesAgents { get; set; }


    }
}
