using Ada.Framework.Data.DBConnector.Connections.Entities;
using Ada.Framework.Data.DBConnector.Entities.DataBase;
using System.Collections.Generic;

namespace Ada.Framework.Data.DBConnector.Connections
{
    /// <summary>
    /// Contrato de la implementación encargada de leer la configuración.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public interface IConfiguracionBaseDatos
    {
        /// <summary>
        /// Obtiene o establece los conectores a bases de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        IList<ConectorTO> Conectores { get; set; }

        /// <summary>
        /// Obtiene o establece la lista de conexiones del archivo de conexiones (DataBaseConnections.xml).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        IList<ConexionTO> Conexiones { get; set; }

        /// <summary>
        /// Obtiene o establece la ruta del archivo de conexiones a base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        string DataBaseConnectionsPath { get; set; }

        /// <summary>
        /// Obtiene o establece la ruta del archivo validador (Xml Schema) de conexiones a base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        string DataBaseConnectionsValidatorPath { get; set; }

        /// <summary>
        /// Obtiene la conexión a la base de datos. Sus ConnectionString, tipo de base de datos, etc.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="nombre">Nombre de la conexión especificada en el archivo DataBaseConnections.xml.</param>
        /// <returns>Representación de la configuración de la base de datos.</returns>
        /// <exception cref="Ada.Framework.Util.FileMonitor.Exceptions.ArchivoNoEncontradoException">Lanzado al no encontrar el archivo XML o su esquema.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">Lanzado al encontrar al archivo de configuración inválido.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">Lanzada de no existir la conexión solicitada.</exception>
        ConexionTO ObtenerConexion(string nombre);

        /// <summary>
        /// Obtiene la conexión a la base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 17/08/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="nombre">Nombre de la conexión especificada en el archivo DataBaseConnections.xml.</param>
        /// <returns></returns>
        /// <exception cref="Ada.Framework.Util.FileMonitor.Exceptions.ArchivoNoEncontradoException">Lanzado al no encontrar el archivo XML o su esquema.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">Lanzado al encontrar al archivo de configuración inválido.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">Lanzada de no existir la conexión solicitada.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">¡No se puede crear la instancia de base de datos  + db.Instancia + !</exception>
        ConexionBaseDatos ObtenerConexionBaseDatos(string nombre);

        /// <summary>
        /// Carga la configuración desde el archivo XML a memoria. De paso lo valida.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <exception cref="Ada.Framework.Util.FileMonitor.Exceptions.ArchivoNoEncontradoException">Lanzado al no encontrar el archivo XML o su esquema.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">Lanzado al encontrar al archivo de configuración inválido.</exception>
        void CargarConfiguracion(bool comprobarFechaModificacion = false);

        /// <summary>
        /// Guarda la configuración de Conectores y Conexiones en el archivo XML de configuración.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <exception cref="Ada.Framework.Util.FileMonitor.Exceptions.ArchivoNoEncontradoException">Lanzado al no encontrar el archivo XML o su esquema.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">Lanzado al encontrar al archivo de configuración inválido.</exception>
        void GuardarConfiguracion();

        /// <summary>
        /// 
        /// </summary>
        void Serializar();
    }
}
