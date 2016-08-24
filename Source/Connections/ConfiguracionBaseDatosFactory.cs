using Ada.Framework.Configuration;
using Ada.Framework.Data.DBConnector.Connections.Exceptions;
using Ada.Framework.Util.FileMonitor;
using System.Configuration;

namespace Ada.Framework.Data.DBConnector.Connections
{
    /// <summary>
    /// Factoría de la configuración de la base de datos.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public static class ConfiguracionBaseDatosFactory
    {
        /// <summary>
        /// Contiene en sólo lectura, el nombre del archivo de conexiones a bases de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private static readonly string NOMBRE_ARCHIVO_CONEXION = "DataBaseConnections.dbc";

        /// <summary>
        /// Contiene en sólo lectura, el nombre del archivo que valida el xml de conexiones a bases de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private static readonly string NOMBRE_ARCHIVO_VALIDADOR_XML_CONEXION = "DataBaseConnections.xsd";

        /// <summary>
        /// Obtiene la implementación para la configuración de la base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Una implementación de <see cref="Ada.Framework.Data.DBConnector.Connections.IConfiguracionBaseDatos"></see>.</returns>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">Lanzada al no encontrar la declaración de sección en el archivo de configuración.</exception>
        /// <exception cref="Ada.Framework.Util.FileMonitor.Exceptions.ArchivoNoEncontradoException">Lanzada al no encontrar el archivo en la ruta especificada por DataBaseConnectionsPath.</exception>
        /// <exception cref="Ada.Framework.Util.FileMonitor.Exceptions.ArchivoNoEncontradoException">Lanzada al no encontrar el archivo en la ruta especificada por DataBaseConnectionsValidatorPath.</exception>
        public static IConfiguracionBaseDatos ObtenerConfiguracionDeBaseDatos()
        {
            IMonitorArchivo monitor = MonitorArchivoFactory.ObtenerArchivo();

            string DataBaseConnectionsPath = FrameworkConfigurationManager.ObtenerRutaArchivoConfiguracion("DataBaseConnections");
            string DataBaseConnectionsValidatorPath = FrameworkConfigurationManager.ObtenerRutaArchivoConfiguracion("DataBaseConnectionsValidator");
            
            DataBaseConnectionsPath = monitor.ObtenerRutaArchivoExistente(DataBaseConnectionsPath, NOMBRE_ARCHIVO_CONEXION);
            DataBaseConnectionsValidatorPath = monitor.ObtenerRutaArchivoExistente(DataBaseConnectionsValidatorPath, NOMBRE_ARCHIVO_VALIDADOR_XML_CONEXION);

            return new ConfiguracionBaseDatos(DataBaseConnectionsPath, DataBaseConnectionsValidatorPath);
        }

        /// <summary>
        /// Obtiene la implementación para la configuración de la base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Una implementación de <see cref="Ada.Framework.Data.DBConnector.Connections.IConfiguracionBaseDatos"></see>.</returns>
        public static IConfiguracionBaseDatos ObtenerConfiguracionDeBaseDatos(string dataBaseConnectionsPath, string dataBaseConnectionsValidatorPath)
        {
            return new ConfiguracionBaseDatos(dataBaseConnectionsPath, dataBaseConnectionsValidatorPath);
        }
    }
}
