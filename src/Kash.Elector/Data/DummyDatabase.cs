using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector.Data
{
    internal class DummyDatabase
    {
        const string ZARAGOZA = "Zaragoza";
        const string HUESCA = "Huesca";
        const string TERUEL = "Teruel";
        const string MORDOR = "Mordor";

        /// <summary>
        /// Almacenamiento de entidades de tipo <see cref="District"/>
        /// </summary>
        internal Dictionary<int, Dictionary<int, District>> Districts { get; set; } = null;

        internal DummyDatabase(Election election)
        {
            Districts = new Dictionary<int, Dictionary<int, District>>() 
            {
                {
                    1, //Identificador de la elección
                    new Dictionary<int,District>()
                    {
                        { 1, new District(election, 1, ZARAGOZA, 7) },
                        { 2, new District(election, 2, HUESCA, 3) },
                        { 3, new District(election, 3, TERUEL, 3) },
                        { 4, new District(election, 4, MORDOR, 337) }
                    }
                }
            };
        }
    }
}
