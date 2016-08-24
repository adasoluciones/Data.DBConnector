using System;
using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector.Connections.Entities
{
    /// <summary>
    /// Representación de los datos de un conector a base de datos.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    [XmlType(TypeName = "Connector")]
    [Serializable]
    public class ConectorTO
    {
        /// <summary>
        /// Obtiene o establece el nombre del conector.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la instancia principal del conector.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute]
        public string Instance { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del ensamblado que contiene al conector.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute]
        public string AssemblyName { get; set; }

        /// <summary>
        /// Obtiene o establece la ruta completa del ensamblado. Opcional.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute]
        public string AssemblyPath { get; set; }
    }
}
