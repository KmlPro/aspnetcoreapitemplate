using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Validator
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
