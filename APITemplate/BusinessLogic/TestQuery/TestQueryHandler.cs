using APITemplate._Infrastructure.Queries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.BusinessLogic.TestQuery
{
    public class TestQueryHandler : IHandleQuery<TestQuery, TestQueryResult>
    {
        public TestQueryResult Execute(TestQuery query)
        {
            var result = new TestQueryResult() { TestQueryResultValue = query.TestValue + "Plus rezultat" };

            return result;
        }
    }
}
