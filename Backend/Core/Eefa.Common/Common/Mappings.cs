using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Eefa.Common
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).ReverseMap();
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .SelectMany(a => a.GetExportedTypes());

            ApplyMappingsFromAssembly(types);
        }

        private void ApplyMappingsFromAssembly(IEnumerable<Type> types)
        {

            var mappertypes = (from t in types
                               where t.GetInterfaces().Any(i => i.IsGenericType 
                               && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)) 
                               && !t.IsAbstract
                               select t).ToList();

            foreach (var type in mappertypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(type);

                    var methodInfo = type.GetMethod("Mapping")
                        ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                    methodInfo?.Invoke(instance, new object[] { this });
                }
                catch (Exception)
                {

                }

               

            }
        }
    }

    public static class MappingExtentions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
        (this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                    // expression.ForAllMembers(x=>x.DoNotAllowNull());
                }
            }
            return expression;
        }

      
    }

   
}
