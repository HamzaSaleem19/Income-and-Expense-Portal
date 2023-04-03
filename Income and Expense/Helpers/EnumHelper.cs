using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Income_and_Expense.Helpers
{
    public static class EnumHelper
    {
        public static Dictionary<string, string> GetKeyValuesFromEnum(Type enumType)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            //var type = typeof(T);
            foreach (var field in enumType.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute == null)
                {
                    if (field.Name != "value__")
                    {
                        results.Add(field.Name, field.Name);
                    }
                }
                else
                {
                    results.Add(field.Name, attribute.Name);
                }
            }

            return results;
        }
    }
}