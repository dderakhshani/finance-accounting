using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Data.Attributes;
using Infrastructure.Data.Models;
using Infrastructure.Data.SqlServer;
using Infrastructure.Interfaces;
using MediatR;

namespace Eefa.Identity.Application.Behaviors
{
    public class RepositoryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IRepository _repository;

        public RepositoryBehavior(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (typeof(TRequest).IsSubclassOf(typeof(CommandBase)))
            {
                if ((request is CommandBase result))
                {
                    if (result.SaveChanges)
                    {
                        await _repository.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            if (typeof(TRequest).IsSubclassOf(typeof(ISearchableRequest)))
            {
                if (response is ServiceResult result)
                {
                    var searachableProps = new Dictionary<string, string>();
                    PropertyInfo[] properties = null;

                    if (result.ObjResult is IEnumerable temp)
                    {
                        foreach (var t in temp)
                        {
                            properties = t.GetType().GetProperties();
                            break;
                        }
                    }

                    if (properties != null)
                    {
                        foreach (var prop in properties)
                        {
                            foreach (var attribute in prop.GetCustomAttributes(true))
                            {
                                if (attribute is Searchable searchableAttr)
                                    searachableProps.Add($"{searchableAttr.Schema}.{searchableAttr.TableName}.{prop.Name}", $"{properties.First(x=>x.Name == prop.Name).GetCustomAttribute(typeof(Display))},"+searchableAttr.ToString());
                            }
                        }

                        result.Searchables = searachableProps;
                    }
                }
            }

            return response;
        }
    }
}
