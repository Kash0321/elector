using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kash.Elector.Data.CosmosDb.SeedWork
{
    public interface IRepository<TItem> where TItem : class
    {
        Task<TItem> Add(TItem item);

        Task<TItem> Update(TItem item);

        Task Remove(TItem item);

        Task Remove(string id);

        Task Remove(Expression<Func<TItem, bool>> predicate);

        Task<TItem> Find(Expression<Func<TItem, bool>> predicate);

        /// <summary>
        /// Obtener elementos
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderPredicate">Expresión de ordenación a aplicar</param>
        /// <param name="orderDescending">False se ordena ascendentemente y True se ordena descendentemente</param>
        /// <returns></returns>
        Task<IEnumerable<TItem>> Get(
            Expression<Func<TItem, bool>> predicate,
            OrderedQueryableExpression<TItem>[] orderExpressions = null);

        /// <summary>
        /// Obtener elementos con paginación
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageSize"></param>
        /// <param name="continuationToken"></param>
        /// <param name="orderPredicate">Expresión de ordenación a aplicar</param>
        /// <param name="orderDescending">False se ordena ascendentemente y True se ordena descendentemente</param>
        /// <returns></returns>
        Task<KeyValuePair<string, IEnumerable<TItem>>> Get(
            Expression<Func<TItem, bool>> predicate,
            int pageSize,
            string continuationToken,
            OrderedQueryableExpression<TItem>[] orderExpressions = null);

        /// <summary>
        /// Obtiene el número de registros que cumplen la petición
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> Count(Expression<Func<TItem, bool>> predicate);
    }
}
