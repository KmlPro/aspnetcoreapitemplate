using APITemplate.CQRS.Queries.Interfaces;
using APITemplate.CQRS.Validator;
using APITemplate.CQRS.Validator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace APITemplate.CQRS.Queries
{
    public class QueryBus : IQueryBus 
    {
        private readonly IServiceProvider _serviceProvider;
        public QueryBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        public TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery where TResult : IQueryResult
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var commandValidator = (IValidator<TQuery>)scope.ServiceProvider
                      .GetRequiredService(typeof(IValidator<TQuery>));

                if (commandValidator != null)
                {
                    var res = commandValidator.Validate(query);
                    if (res.IsValid == false)
                    {
                        throw new ValidationException(res);
                    }
                }

                var queryHandler = (IHandleQuery<TQuery, TResult>)scope.ServiceProvider
                       .GetRequiredService(typeof(IHandleQuery<TQuery, TResult>));

                if (queryHandler == null)
                {
                    throw new Exception(string.Format("No handler found for query '{0}'", query.GetType().FullName));
                }
                return queryHandler.Execute(query);
            }
        }
    }
}
