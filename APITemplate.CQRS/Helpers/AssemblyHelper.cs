using APITemplate.CQRS.Validator.Interfaces;
using System;
using System.ComponentModel;
using System.Reflection;

namespace APITemplate.Helpers
{
    public class AssemblyHelper
    {
        public static string GetDescription(Type type)
        {
            var descriptions = (DescriptionAttribute[])
                type.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }

        public static string GetPropertiesWithValues(IValidatable action)
        {
            Type type = action.GetType();
            PropertyInfo[] props = type.GetProperties();
            string str = "";
            foreach (var prop in props)
            {
                str += (prop.Name + ":" + prop.GetValue(action)) + ",";
            }

            return str;
        }
    }
}
