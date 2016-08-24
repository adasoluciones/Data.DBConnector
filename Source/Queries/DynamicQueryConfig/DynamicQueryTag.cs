using System.Collections.Generic;
using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector.Queries.DynamicQueryConfig
{
    /// <summary>
    /// Representación del tag de una consulta dinámica a base de datos.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 06/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    [XmlType(TypeName = "DynamicQuery")]
    public class DynamicQueryTag
    {
        /// <summary>
        /// Obtiene o establece el nombre identificador de la consulta dinámica.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 06/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute("Name")]
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de consultas que posee la consulta dinámica.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 06/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlElement("Query")]
        public List<QueryTag> Consultas { get; set; }
    }
}
