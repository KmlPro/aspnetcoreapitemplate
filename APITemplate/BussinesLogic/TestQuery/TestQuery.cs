using APITemplate._Infrastructure.Queries.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.BussinesLogic.TestQuery
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
