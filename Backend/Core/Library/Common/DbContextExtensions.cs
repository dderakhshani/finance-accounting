using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Common
{
    public static class DbContextExtensions
    {
        public static async Task SqlQueryAsync(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default)
        {
            if (parameters is null)
            {
                parameters = new object[] { };
            }


            await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

        }

        public static async Task<List<T>> SqlQueryAsync<T>(this DbContext db, string sql, object[] parameters = null, CancellationToken cancellationToken = default) where T : class
        {
            if (parameters is null)
            {
                parameters = new object[] { };
            }

            if (typeof(T).GetProperties().Any())
            {
                using (var db2 = new ContextForQueryType<T>(db.Database.GetDbConnection(),
                           db.Database.CurrentTransaction))
                {
                    db2.Database.SetCommandTimeout(db.Database.GetCommandTimeout());
                    var a = db2.Set<T>().FromSqlRaw(sql, parameters).ToQueryString();
                    return await db2.Set<T>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
                }
            }
            else
            {
                await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
                return default;
            }

        }

        private class ContextForQueryType<T> : DbContext where T : class
        {
            private readonly DbConnection connection;
            private readonly IDbContextTransaction transaction;

            public ContextForQueryType(DbConnection connection, IDbContextTransaction tran)
            {
                this.connection = connection;
                this.transaction = tran;

                if (tran != null)
                {
                    this.Database.UseTransaction((tran as IInfrastructure<DbTransaction>).Instance);
                }
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (transaction != null)
                {
                    optionsBuilder.UseSqlServer(connection);
                }
                else
                {
                    optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());
                }
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>().HasNoKey().ToView(null);
            }
        }
    }

    public class OutputParameter<TValue>
    {
        private bool _valueSet = false;

        public TValue _value;

        public TValue Value
        {
            get
            {
                if (!_valueSet)
                    throw new InvalidOperationException("Value not set.");

                return _value;
            }
        }

        internal void SetValue(object value)
        {
            _valueSet = true;

            _value = null == value || Convert.IsDBNull(value) ? default(TValue) : (TValue)value;
        }
    }
}
