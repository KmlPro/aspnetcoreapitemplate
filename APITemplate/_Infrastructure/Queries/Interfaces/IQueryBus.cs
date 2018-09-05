using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Queries.Interfaces
{
    public interface IQueryBus
    {
        TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery where TResult : IQueryResult;
    }
}
