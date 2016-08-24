using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector.Entities.DataBase
{
    /// <summary>
    /// Representación de la configuración de la base de datos.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    [XmlType(TypeName = "Conexion")]
    [Serializable]
    public sealed class ConexionTO
    {
        /// <summary>
        /// Propiedad que obtiene o establece el nombre de la conexión.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Propiedad que obtiene o establece la cadena de conexión.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la instancia que representa la conexión según el tipo de base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute]
        public string Instance { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de la base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlAttribute]
        public string Type { get; set; }

        /// <summary>
        /// Obtiene o establece el ensamblado que contiene la instancia de conexión a base de datos.
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
