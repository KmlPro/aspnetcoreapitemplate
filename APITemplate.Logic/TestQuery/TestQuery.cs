using APITemplate.CQRS.Queries.Interfaces;
using System.ComponentModel;

namespace APITemplate.Logic.TestQuery
{
    [Description("Test query description")]
    public class TestQuery : IQuery
    {
        public TestQuery(string testValue)
        {
            this.TestValue = testValue;
        }
        public string TestValue { get; set; }
    }
}
