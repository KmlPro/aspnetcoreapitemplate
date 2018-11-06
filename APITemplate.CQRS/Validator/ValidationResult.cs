using System.Collections.Generic;
using System.Linq;

namespace APITemplate.CQRS.Validator
{
    public class ValidationResult
    {
        public bool IsValid
        {
            get
            {
                return !Messages.Any();
            }
        }

        public List<string> Messages { get; private set; } = new List<string>();
    }
}
