using Ada.Framework.Util.FileMonitor;
using Ada.Framework.Util.FileMonitor.Exceptions;
using Ada.Framework.Data.DBConnector.Connections.Exceptions;
using Ada.Framework.Data.DBConnector.Entities.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using Ada.Framework.Data.DBConnector.Connections.Entities;
using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector.Connections
{
    /// <summary>
    /// Clase cuya funcionalidad es manejar las configuraciones de conexión a base de datos. Esta clase no puede heredarse.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    internal sealed class ConfiguracionBaseDatos : IConfiguracionBaseDatos
    {
        /// <summary>
        /// Contiene la fecha de última modificación del archivo de conexiones (DataBaseConnections.xml).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private static Nullable<DateTime> fechaUltimaModificacionArchivo;

        /// <summary>
        /// Contiene la lista de conexiones en memoria que han sido extraídas del archivo de conexiones (DataBaseConnections.xml).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private static IList<ConexionTO> conexiones;

        /// <summary>
        /// Obtiene o establece la lista de conexiones del archivo de conexiones (DataBaseConnections.xml).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public IList<ConexionTO> Conexiones { get { return conexiones; } set { conexiones = value; } }

        /// <summary>
        /// Contiene la lista de conectores a base de datos que han sido extraídas del archivo de conexiones (DataBaseConnections.xml).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private static IList<ConectorTO> conectores { get; set; }

        /// <summary>
        /// Obtiene o establece los conectores a bases de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public IList<ConectorTO> Conectores { get { return conectores; } set { conectores = value; } }
        
        /// <summary>
        /// Obtiene o establece la ruta del archivo de conexiones a base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string DataBaseConnectionsPath { get; set; }

        /// <summary>
        /// Obtiene o establece la ruta del archivo validador (Xml Schema) de conexiones a base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string DataBaseConnectionsValidatorPath { get; set; }

        /// <summary>
        /// Constructor que inicializa la instancia con las rutas del archivo de configuración y validación.
        /// </summary>
        /// <param name="dataBaseConnectionsPath">Ruta del archivo XML de configuración.</param>
        /// <param name="dataBaseConnectionsValidatorPath">Ruta del archivo XML que contiene el esquema del archivo de configuración. Xml Schema.</param>
        public ConfiguracionBaseDatos(string dataBaseConnectionsPath, string dataBaseConnectionsValidatorPath)
        {
            DataBaseConnectionsPath = dataBaseConnectionsPath;
            DataBaseConnectionsValidatorPath = dataBaseConnectionsValidatorPath;
        }

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
        public ConexionTO ObtenerConexion(string nombre)
        {
            CargarConfiguracion(true);

            IList<ConexionTO> retorno = Conexiones.Where(c => c.Name == nombre).ToList();
            
            if (retorno.Count == 0)
            {
                throw new ConfiguracionBaseDatosException("¡La conexión " + nombre + " no existe!");
            }

            return retorno.First();
        }

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
        /// <exception cref="ConfiguracionBaseDatosException">¡No se puede crear la instancia de base de datos  + db.Instancia + !</exception>
        public ConexionBaseDatos ObtenerConexionBaseDatos(string nombre)
        {
            return ConexionBaseDatosFactory.ObtenerConexionBaseDatos(ObtenerConexion(nombre));
        }

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
        public void CargarConfiguracion(bool comprobarFechaModificacion = false)
        {
            IMonitorArchivo monitor = MonitorArchivoFactory.ObtenerArchivo();

            if (monitor.Existe(DataBaseConnectionsPath))
            {
                if (!fechaUltimaModificacionArchivo.HasValue)
                {
                    fechaUltimaModificacionArchivo = new DateTime(1, 1, 1);
                }

                if (!comprobarFechaModificacion || (Conexiones == null || Conexiones.Count == 0 || monitor.FueModificado(fechaUltimaModificacionArchivo.Value, DataBaseConnectionsPath)))
                {
                    Conexiones = new List<ConexionTO>();
                    Conectores = new List<ConectorTO>();

                    XmlDocument doc;
                    XmlReader reader = null;
                    try
                    {
                        CargarXML(out doc, out reader);

                        foreach (XmlNode appender in doc.DocumentElement.ChildNodes[1].ChildNodes)
                        {
                            if (appender is XmlElement)
                            {
                                XmlElement element = (appender as XmlElement);

                                Conectores.Add(new ConectorTO()
                                {
                                    Name = element.GetAttribute("Name"),
                                    Instance = element.GetAttribute("Instance"),
                                    AssemblyName = element.GetAttribute("AssemblyName"),
                                    AssemblyPath = element.GetAttribute("AssemblyPath")
                                });
                            }
                        }

                        foreach (XmlNode elemento in doc.DocumentElement.ChildNodes[0].ChildNodes)
                        {
                            if (elemento is XmlElement)
                            {
                                XmlElement element = (elemento as XmlElement);

                                ConectorTO conector = Conectores.Where(c => c.Name == element.GetAttribute("Type")).First();

                                Conexiones.Add(new ConexionTO()
                                {
                                    ConnectionString = element.GetAttribute("ConnectionString"),
                                    Name = element.GetAttribute("Name"),
                                    Instance = conector.Instance,
                                    Type = element.GetAttribute("Type"),
                                    AssemblyName = conector.AssemblyName,
                                    AssemblyPath = conector.AssemblyPath
                                });
                            }
                        }
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                }

                fechaUltimaModificacionArchivo = monitor.ObtenerFechaUltimaModificacion(DataBaseConnectionsPath);
            }
            else
            {
                throw new ArchivoNoEncontradoException("¡No se ha encontrado el archivo de conexiones a bases de datos!", DataBaseConnectionsPath);
            }
        }

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
        public void GuardarConfiguracion()
        {
            XmlDocument doc;
            XmlReader reader;
            CargarXML(out doc, out reader);
            
            try
            {
                doc.DocumentElement.ChildNodes[0].RemoveAll();
                foreach (ConexionTO conexion in Conexiones)
                {
                    XmlElement elemento = doc.CreateElement("Connection");
                    elemento.SetAttribute("Name", conexion.Name);
                    elemento.SetAttribute("ConnectionString", conexion.ConnectionString);
                    elemento.SetAttribute("Type", conexion.Type);
                    doc.DocumentElement.ChildNodes[0].AppendChild(elemento);
                }

                doc.DocumentElement.ChildNodes[1].RemoveAll();
                foreach (ConectorTO conector in Conectores)
                {
                    XmlElement elemento = doc.CreateElement("Connector");
                    elemento.SetAttribute("Name", conector.Name);
                    elemento.SetAttribute("Instance", conector.Instance);
                    elemento.SetAttribute("AssemblyName", conector.AssemblyName);
                    elemento.SetAttribute("AssemblyPath", conector.AssemblyPath);
                    doc.DocumentElement.ChildNodes[1].AppendChild(elemento);
                }
                reader.Close();
                doc.Save(DataBaseConnectionsPath);
            }
            catch (Exception e)
            {
                throw new ConfiguracionBaseDatosException("¡Error al guardar la configuración en " + DataBaseConnectionsPath + "!", e);
            }
        }

        /// <summary>
        /// Carga y valida el archivo Xml a los parámetros de salida correspondientes.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="doc">Instancia del documento XML.</param>
        /// <param name="reader">Instancia del lector XML.</param>
        /// <exception cref="Ada.Framework.Util.FileMonitor.Exceptions.ArchivoNoEncontradoException">Lanzado al no encontrar el archivo XML o su esquema.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Connections.Exceptions.ConfiguracionBaseDatosException">Lanzado al encontrar al archivo de configuración inválido.</exception>
        private void CargarXML(out XmlDocument doc, out XmlReader reader)
        {
            doc = new XmlDocument();
            reader = null;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(string.Empty, DataBaseConnectionsValidatorPath);
            settings.ValidationType = ValidationType.Schema;

            try
            {
                reader = XmlReader.Create(DataBaseConnectionsPath, settings);
                doc.Load(reader);
                doc.Validate(ErrorAlValidar);
            }
            catch (XmlSchemaValidationException xmlValException)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                throw new ConfiguracionBaseDatosException("!Archivo de configuración inválido (DataBaseConnections.xml)! " + xmlValException.Message);
            }
            catch (IOException ioE)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                throw new ArchivoNoEncontradoException("¡No se ha encontrado el archivo de conexiones a bases de datos o su esquema!", DataBaseConnectionsPath, ioE);
            }
        }

        /// <summary>
        /// Evento lanzado al fallar la validación del archivo Xml.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="sender">Origen del evento.
        /// Nota: Determine el tipo de un remitente antes de usarlo
        ///     en el código.No puede suponer que el remitente es una instancia de un tipo
        ///     determinado.Tampoco se garantiza que el remitente no sea nulo.Rodee siempre
        ///     las conversiones de tipos con lógica de control de errores.</param>
        /// <param name="e">Devuelve información detallada relacionada con ValidationEventHandler.</param>
        private void ErrorAlValidar(object sender, ValidationEventArgs e)
        {
            throw new ConfiguracionBaseDatosException("!Archivo de configuración erróneo (DataBaseConnections.xml)! " + e.Message);
        }

        public void Serializar()
        {
            CargarConfiguracion();

            ConfiguracionTO config = new ConfiguracionTO()
            {
                Conexiones = Conexiones as List<ConexionTO>,
                Conectores = Conectores as List<ConectorTO>
            };

            XmlSerializer ser = new XmlSerializer(typeof(ConfiguracionTO));
            FileStream stream = new FileStream("E:\\Prueba_Serializacion.xml",FileMode.OpenOrCreate);            
            ser.Serialize(stream, config);
            stream.Flush();
            stream.Close();
        }
    }
}
