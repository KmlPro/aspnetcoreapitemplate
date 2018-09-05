using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Validator.Interfaces
{
    public interface IValidator<TAction> where TAction : IValidatable
    {
        ValidationResult Validate(TAction command);
    }
}
