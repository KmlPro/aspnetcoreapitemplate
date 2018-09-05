using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Queries.Interfaces
{
    interface IHandleQuery<in TQuery, TResult> where TQuery : IQuery where TResult : IQueryResult
    {
        TResult Execute(TQuery query);
    }
}
