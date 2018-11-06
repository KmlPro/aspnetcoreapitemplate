using APITemplate.CQRS.Queries.Interfaces;

namespace APITemplate.Logic.TestQuery
{
    public class TestQueryResult : IQueryResult
    {
        public string TestQueryResultValue { get; set; }
    }
}
