using APITemplate.CQRS.Validator;
using APITemplate.CQRS.Validator.Interfaces;

namespace APITemplate.Logic.TestQuery
{
    public class TestQueryValidator : IValidator<TestQuery>
    {
        public ValidationResult Validate(TestQuery query)
        {
            ValidationResult vr = new ValidationResult();
            if (string.IsNullOrEmpty(query.TestValue))
            {
                vr.Messages.Add("TestValue cannnot be empty");
            }

            return vr;
        }
    }
}

