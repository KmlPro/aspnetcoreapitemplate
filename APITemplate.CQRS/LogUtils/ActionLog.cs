using APITemplate.CQRS.Validator.Interfaces;
using APITemplate.Helpers;
using System;

namespace APITemplate.CQRS.LogUtils
{
    public class ActionLog
    {
        public string TypeName { get; set; }
        public string Parameters { get; set; }
        public string ActionDescription { get; set; }
        public Guid ActionId { get; set; }

        public ActionLog(IValidatable validatable)
        {
            TypeName = validatable.GetType().Name;
            Parameters = AssemblyHelper.GetPropertiesWithValues(validatable);
            ActionDescription = AssemblyHelper.GetDescription(validatable.GetType());
            ActionId = Guid.NewGuid();
        }
    }
}
