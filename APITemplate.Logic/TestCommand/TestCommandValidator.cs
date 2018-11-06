using APITemplate.CQRS.Validator;
using APITemplate.CQRS.Validator.Interfaces;

namespace APITemplate.Logic.TestCommand
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

