using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector.Queries.DynamicQueryConfig
{
    /// <summary>
    /// Representación de una consulta como cadena de texto.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 06/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    [XmlType(TypeName = "DbType")]
    public class QueryTag
    {
        /// <summary>
        /// Obtiene o establece el tipo de la base de datos que responde a esta query.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 06/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute("DbType")]
        public string TipoBaseDatos { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de consulta.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 06/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute("Type")]
        public string Tipo { get; set; }

        /// <summary>
        /// Obtiene o establece la consulta como cadena de texto.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 06/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlText]
        public string Consulta { get; set; }
    }
}
