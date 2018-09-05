using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Validator
{
    public class ValidationException : Exception
    {
        public ValidationResult Result { get; protected set; }

        public ValidationException(ValidationResult result)
        {
            this.Result = result;
        }
    }
}
