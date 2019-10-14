using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Kash.Elector.Data.CosmosDb.SeedWork
{
    public class OrderedQueryableExpression<TItem>
    {
        public Expression<Func<TItem, object>> Predicate { get; set; }

        public bool orderDescending { get; set; }
    }
}
