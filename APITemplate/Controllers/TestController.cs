using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITemplate._Infrastructure.Commands.Interfaces;
using APITemplate._Infrastructure.Queries.Interfaces;
using APITemplate.BussinesLogic.TestCommand;
using APITemplate.BussinesLogic.TestQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        public TestController(IQueryBus queryBus, ICommandBus commandBus) : base(queryBus, commandBus)
        {
        }

        [HttpGet]
        [Route("get")]
        public ActionResult<string> Get(string inputValue)
        {
            TestQuery query = new TestQuery(inputValue);
            var result = ExecuteQuery<TestQuery, TestQueryResult>(query);
          
            return result.TestQueryResultValue;
        }

        [HttpPost]
        [Route("trycommand")]
        public ActionResult TryCommand(string inputValue)
        {
            TestCommand query = new TestCommand(inputValue);
            ExecuteCommand<TestCommand>(query);

            return Ok();
        }
    }
}