
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Threading.Tasks;

using System.Linq;
using System.Linq.Dynamic.Core;
using Eefa.Common.Validation.Resources;

namespace Eefa.Common.Validation
{
    public delegate Task<bool> ValidatorFunctionDelegate(object model);

    public class Utility
    {
        public static string GetJsonValidator<T>(T model, IWebHostEnvironment hostingEnvironment)
        {
            var assembly = model.GetType()?.Assembly;
            var fileName = @$"{model.GetType().Name}.json";

            //Find Fully Qualified Name of the resouce in Embeded Resources of the Assembly
            var resouceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(fileName));
            if(resouceName!=null)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resouceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    return json;
                }
            }
            return null;
           

        }

        internal static string ValidationMessageBuilder(IValidationFactory validationFactory, ValidationMessage message)
        {
            var validationMessage = validationFactory.Get(message.Key);
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

        public static async Task<Dictionary<string, List<string>>> Validate<T>(T model,
           ICurrentUserAccessor currentUserAccessor,
           IWebHostEnvironment hostingEnvironment,
           IValidationFactory validationFactory)
        {

            var result = Utility.GetJsonValidator(model, hostingEnvironment);
            if(result!=null)
            {
                var validationSchema = JsonConvert.DeserializeObject<List<ValidationCriteria>>(result);

                var exceptions = new Dictionary<string, List<string>>();


                foreach (var criteria in validationSchema)
                {
                    var messages = new List<string>();

                    if (criteria.FieldName is null || criteria.FieldName.Trim() == "") continue;
                    if (criteria.Conditions is not null)
                    {
                        foreach (var expression in criteria.Conditions)
                        {
                            if (expression.Condition.Trim() is null or "") continue;

                            var expressionString = "p=>";

                            expressionString += expression.Condition.Replace("@", "p.");

                            if (DynamicExpressionParser.ParseLambda<T, bool>(default, true, expressionString)
                                .Compile().Invoke(model) is false)
                            {
                                messages.Add(Utility.ValidationMessageBuilder(validationFactory, expression.Message));
                            }
                        }
                    }


                    if (criteria.Functions is not null)
                    {
                        foreach (var function in criteria.Functions)
                        {
                            if (function.Name.Trim() is null or "") continue;
                            var functionResult = await ValidationFunctionsLocator
                                                        .ValidatorFunctions[function.Name].Invoke(model);

                            if (functionResult)
                            {
                                messages.Add(Utility.ValidationMessageBuilder(validationFactory,
                                    function.Message));
                            }
                        }
                    }

                    if (criteria.NestedValidations is not null)
                    {
                        foreach (var nestedValidation in criteria.NestedValidations)
                        {
                            var prop = model.GetType().GetProperty(criteria.FieldName)?.PropertyType;
                            var nestedObject = model.GetType().GetProperty(criteria.FieldName)?.GetValue(model);
                            var res = Validate<object>(nestedObject, currentUserAccessor, hostingEnvironment, validationFactory);

                        }
                    }

                    if (messages.Count > 0)
                    {
                        exceptions.Add(criteria.FieldName, messages);
                    }
                }

                return exceptions;
            }
            else
            {
                return null;
            }
           
        }

    }
}
