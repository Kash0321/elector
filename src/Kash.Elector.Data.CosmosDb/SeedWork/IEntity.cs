using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data.CosmosDb.SeedWork
{
    public interface IEntity
    {
        string Id { get; set; }

        string _etag { get; set; }
    }
}
