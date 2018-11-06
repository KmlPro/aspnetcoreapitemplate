using APITemplate.CQRS.Queries.Interfaces;

namespace APITemplate.Logic.TestQuery
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
