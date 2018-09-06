using APITemplate._Infrastructure.Validator;
using APITemplate._Infrastructure.Validator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.BussinesLogic.TestQuery
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

