using APITemplate._Infrastructure.Queries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.BusinessLogic.TestQuery
{
    public class TestQueryResult : IQueryResult
    {
        public string TestQueryResultValue { get; set; }
    }
}
