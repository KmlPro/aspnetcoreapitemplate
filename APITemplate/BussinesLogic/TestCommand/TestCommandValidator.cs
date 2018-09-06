using APITemplate._Infrastructure.Validator;
using APITemplate._Infrastructure.Validator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.BussinesLogic.TestCommand
{
    public class TestCommandValidator : IValidator<TestCommand>
    {
        public ValidationResult Validate(TestCommand command)
        {
            ValidationResult vr = new ValidationResult();
            if (string.IsNullOrEmpty(command.TestCommandValue))
            {
                vr.Messages.Add("TestCommandValue cannot be null");
            }

            return vr;
        }
    }
}

