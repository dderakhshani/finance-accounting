using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Library.Models
{
    public class FormulasModel
    {
       
        public class FormulaCondition
        {
            public string Expression { get; set; }
            public List<Property> Properties { get; set; }
        }


        public class Formula
        {
            public string Property { get; set; }
            public Value Value { get; set; }
        }

        public class Property
        {
            public string Name { get; set; }
            public string? AggregateFunction { get; set; } = null;
        }

        public class Value
        {
            public string Text { get; set; }
            public List<Property> Properties { get; set; }
        }
    }

    public static class Extention
    {
        public static LinkedList<object> FetchValueFromDataRow(this LinkedList<object> calculatedValues, IEnumerable<JObject> dataList, JObject row, FormulasModel.Property property)
        {
            switch (property.AggregateFunction)
            {
                case "SUM":
                    var sum = dataList.Sum(dataRow => dataRow.TryGetValue<long>(property.Name));
                    calculatedValues.AddLast(sum.ToString());
                    break;
                default:
                    try
                    {
                        calculatedValues.AddLast(row.TryGetValue<object>(property.Name));
                    }
                    catch (Exception ex)
                    {

                        throw new Exception($"Property {property.Name} not provided");
                    }
                    break;
            }

            return calculatedValues;
        }


        public static T TryGetValue<T>(this JObject jObject, string key)
        {
            try
            {
                var res = jObject.GetValue(key).Value<T>();
                if (res is null)
                {
                    throw new Exception();
                }
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

}