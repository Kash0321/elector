using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data.CosmosDb
{
    /// <summary>
    /// Configuración del almacenamiento de datos en Azure CosmosDb
    /// </summary>
    public class CosmosDbConfiguration
    {
        /// <summary>
        /// Uri de acceso a la instancia de CosmosDb
        /// </summary>
        public string ServiceEndpoint { get; set; }

        /// <summary>
        /// Nombre de la base de datos
        /// </summary>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Nombre de la colección
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Clave que proporciona acceso a CosmosDb
        /// </summary>
        public string AuthKeyOrResourceToken { get; set; }
    }
}
