using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Interfaces;
using Library.Models;
using Library.Resources;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Library.Common
{
    public static class CommandBaseExtention{
        public static async Task<ServiceResult> Save(this ICommand command, IRepository repository, CancellationToken cancellationToken)
        {
            if (command.SaveChanges)
            {
                if (await repository.SaveChangesAsync(command.MenueId, cancellationToken) > 0)
                {
                    return ServiceResult.Success();
                }
            }
            else
            {
                return ServiceResult.Success();
            }

            return ServiceResult.Failure();
        }


        public static async Task<ServiceResult> Save<T>(this ICommand command, IRepository repository,T entity, CancellationToken cancellationToken)
        { 
            if (command.SaveChanges)
            {
                if (await repository.SaveChangesAsync(command.MenueId, cancellationToken) > 0)
                {
                    return ServiceResult.Success(entity);
                }
            }
            else
            {
                return ServiceResult.Success(entity);
            }

            return ServiceResult.Failure();
        }


        public static async Task<ServiceResult> Save<TEntity,TModel>(this ICommand command, IRepository repository,IMapper mapper ,TEntity entity, CancellationToken cancellationToken)
        {
            if (command.SaveChanges)
            {
                if (await repository.SaveChangesAsync(command.MenueId, cancellationToken) > 0)
                {
                    return ServiceResult.Success(mapper.Map<TModel>(entity));
                }
            }
            else
            {
                return ServiceResult.Success(entity);
            }

            return ServiceResult.Failure();
        }

        public static async Task<ServiceResult> Save<TEntity, TModel>(this ICommand command, IRepository repository,IMediator mediator, IMapper mapper, TEntity entity, CancellationToken cancellationToken)
        {
            if (command.SaveChanges)
            {
                if (await repository.SaveChangesAsync(command.MenueId, cancellationToken) > 0)
                {
                    return ServiceResult.Success(mapper.Map<TModel>(entity));
                }
            }
            else
            {
                return ServiceResult.Success(entity);
            }

            return ServiceResult.Failure();
        }

    }

    public class CommandBase : ICommand
    {
        public bool SaveChanges { get; set; } = true;
        public int MenueId { get; set; }


        public async Task<Dictionary<string, List<string>>> Validate<T>(T command,
            IRepository repository,
            ICurrentUserAccessor currentUserAccessor,
            IMediator mediator,
            IWebHostEnvironment hostingEnvironment,
            IResourceFactory validationFactory,
            IMapper mapper
        ) where T : ICommand
        {
            var actionValidator = JsonConvert.DeserializeObject<Validator>(GetJsonValidator(command, hostingEnvironment));

            var exceptions = new Dictionary<string, List<string>>();
            if (actionValidator.Properties is null)
            {
                return exceptions;
            }

            foreach (var actionConfigProperty in actionValidator.Properties!)
            {
                var messages = new List<string>();

                if (actionConfigProperty.Title.Trim() is null or "") continue;
                if (actionConfigProperty.Expressions is not null)
                {
                    foreach (var expression in actionConfigProperty.Expressions)
                    {
                        if (expression?.Query?.Trim() is null or "") continue;
                        if (DynamicExpressionParser.ParseLambda<T, bool>(default, true, expression.Query).Compile()
                            .Invoke(command) is false)
                        {
                            messages.Add(ValidationMessageBuilder(validationFactory, expression.Message));
                        }
                    }
                }


                if (actionConfigProperty.Functions is not null)
                {
                    foreach (var function in actionConfigProperty.Functions ?? new List<Function>())
                    {
                        if (function.Name.Trim() is null or "") continue;
                        var functionResult = await ValidationFunctionsLocator
                            .ValidatorFunctions[function.Name]
                            .Invoke(command, repository, mapper);

                        if (functionResult)
                        {
                            messages.Add(ValidationMessageBuilder(validationFactory,
                                function.Message));
                        }
                    }
                }

                if (actionConfigProperty.NestedValidations is not null)
                {
                    foreach (var nestedValidation in actionConfigProperty.NestedValidations)
                    {
                        var prop = command.GetType().GetProperty(actionConfigProperty.Title)?.PropertyType;
     
                        var propvalue = command.GetType().GetProperty(actionConfigProperty.Title)?.GetValue(command);

                        if (typeof(IEnumerable<ICommand>).IsAssignableFrom(prop))
                        {
                            var commands = propvalue as IEnumerable;
                            foreach (var c in commands)
                            {
                                var o = new object[]{
                                        c as ICommand,
                                        repository,
                                        currentUserAccessor,
                                        mediator,
                                        hostingEnvironment,
                                        validationFactory,
                                        mapper
                                    };

                                var method = GetType().GetMethod("Validate")
                                    ?.MakeGenericMethod(new Type[] { c.GetType() });
                                var res = ((Task<Dictionary<string, List<string>>>)method.Invoke(this, o)).GetAwaiter().GetResult();

                                if (res.Count > 0)
                                {
                                    return res;
                                }

                            }

                        }
                        else
                        {
                            var c = propvalue as ICommand;
                            var o = new object[]{
                                c,
                                repository,
                                currentUserAccessor,
                                mediator,
                                hostingEnvironment,
                                validationFactory,
                                mapper
                            };
                            var method = GetType().GetMethod("Validate")
                                .MakeGenericMethod(new Type[] { c.GetType() });
                            var res = ((Task<Dictionary<string, List<string>>>)method.Invoke(this, o)).GetAwaiter().GetResult();
                        }
                    }
                }

                if (messages.Count > 0)
                {
                    exceptions.Add(TranslatedKey(actionConfigProperty.Title, typeof(T).Name, validationFactory), messages);
                }
            }

            return exceptions;
        }

        public static string GetJsonValidator<T>(T command, IWebHostEnvironment hostingEnvironment) where T : ICommand
        {
            var assemblyName = command.GetType()?.Assembly?.FullName?.Split(',')[0] ?? throw new Exception("Couldn't find assembly");
            // ReSharper disable once RedundantAssignment
            var jsonPath = string.Empty;
            var a = Directory.GetCurrentDirectory();

#if DEBUG
            var currentDir = hostingEnvironment.ContentRootPath ?? Directory.GetCurrentDirectory();

            if (currentDir.Contains("Tests"))
            {
                currentDir = currentDir.Remove(currentDir.IndexOf("Test", StringComparison.Ordinal),
                    currentDir.Length - currentDir.IndexOf("Test", StringComparison.Ordinal));
              //  currentDir = currentDir + "Microservices\\" + assemblyName.Split('.')[1] + "\\";
            }

            jsonPath =
                $@"{currentDir.Remove(currentDir.LastIndexOf('\\'), currentDir.Length - currentDir.LastIndexOf('\\'))}\{assemblyName}{command.GetType().Namespace?.Replace(assemblyName, "").Replace('.', '\\')}\{command.GetType().Name}.json";

#else
            jsonPath = @$"{Directory.GetCurrentDirectory()}{command.GetType().Namespace?.Replace(assemblyName, "").Replace('.', '\\')}\{command.GetType().Name}.json";
#endif

            var json = File.ReadAllText(jsonPath);
            return json;
        }


        public static string TranslatedKey(string key, string commandOrTabelName, IResourceFactory validationFactory)
        {
            var transalted = validationFactory.GetTranslatedMetaData($"{commandOrTabelName}-{key}");
            if (string.IsNullOrEmpty(transalted))
            {
                transalted = validationFactory.GetTranslatedMetaData($"{key}");
            }

            if (string.IsNullOrEmpty(transalted))
            {
                transalted = key.ToLower();
            }
            return transalted;
        }  
        

        public static string ValidationMessageBuilder(IResourceFactory validationFactory, Message message)
        {
            var validationMessage = validationFactory.GetValidationMessage(message.Key);
            if (string.IsNullOrEmpty(validationMessage))
            {
                return message.Key;
            }
            var messageBuilder = new StringBuilder();
            var i = 1;
            foreach (var s in validationMessage.Split(' '))
            {
                if (s == $"[{i}]")
                {
                    messageBuilder.Append(message.Values[i - 1]);
                    i++;
                }
                else
                {
                    messageBuilder.Append(s);
                }

                messageBuilder.Append(" ");
            }

            return messageBuilder.ToString();
        }
    }
}