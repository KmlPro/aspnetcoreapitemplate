using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITemplate._Infrastructure.Commands.Interfaces;
using APITemplate._Infrastructure.ICQRS;
using APITemplate._Infrastructure.Queries.Interfaces;
using APITemplate.BusinessLogic.TestCommand;
using APITemplate.BusinessLogic.TestQuery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APITemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICQRS _cqrs;
        public TestController(ICQRS cQRS)
        {
            this._cqrs = cQRS;
        }

        [HttpGet]
        [Route("get")]
        public ActionResult<string> Get(string inputValue)
        {
            TestQuery query = new TestQuery(inputValue);
            var result = _cqrs.ExecuteQuery<TestQuery, TestQueryResult>(query);
          
            return result.TestQueryResultValue;
        }

        [HttpPost]
        [Route("trycommand")]
        public ActionResult TryCommand(string inputValue)
        {
            TestCommand query = new TestCommand(inputValue);
            _cqrs.ExecuteCommand<TestCommand>(query);

            return Ok();
        }
    }
}