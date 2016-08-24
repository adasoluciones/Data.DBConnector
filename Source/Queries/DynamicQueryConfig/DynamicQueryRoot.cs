using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector.Queries.DynamicQueryConfig
{
    /// <summary>
    /// Representa la raiz de las consultas dinámicas.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    [XmlRoot("DynamicQueries")]
    public class DynamicQueryRoot
    {
        [XmlIgnore]
        internal DateTime FechaUltimaModificacion { get; set; }
        
        /// <summary>
        /// Obtiene o establece las consultas dinámicas declaradas.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlElement("DynamicQuery")]
        public List<DynamicQueryTag> Consultas { get; set; }
    }
}
