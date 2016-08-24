using Ada.Framework.Configuration;
using Ada.Framework.Data.DBConnector.Exceptions;
using Ada.Framework.Util.FileMonitor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector.Queries.DynamicQueryConfig
{
    /// <summary>
    /// Administrador de Dynamic Query.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public static class DynamicQueryManager
    {
        /// <summary>
        /// Obtiene la ruta de la carpeta donde estan las queries dinámicas(DAO.dq.xml).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private static string CARPETA_DYNAMIC_QUERIES { get { return "DynamicQueriesFolder"; } }
        
        /// <summary>
        /// Obtiene o establece la fecha de última modificación del archivo con las consultas dinámicas.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private static IDictionary<string, DynamicQueryRoot> ArchivosConsultas = new Dictionary<string, DynamicQueryRoot>();
        
        /// <summary>
        /// Obtiene o establece la configuración leida del archivo de consultas XML. 
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private static DynamicQueryRoot Configuracion { get; set; }
        
        /// <summary>
        /// Carga una consulta dinámica con los datos obtenidos del archivo de configuración.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="dynamicQuery">Consulta dinámica.</param>
        public static void Cargar(ref DynamicQuery dynamicQuery)
        {
            MethodBase metodo = new StackTrace().GetFrame(1).GetMethod();
            QueryTag consulta = ObtenerConsulta(dynamicQuery.Nombre, dynamicQuery.TipoBaseDatos, metodo.DeclaringType);
            dynamicQuery.Consulta = consulta.Consulta;
            dynamicQuery.Tipo = consulta.Tipo;
        }

        /// <summary>
        /// Obtiene una consulta desde el archivo de configuración, según el nombre de la consulta y el tipo de la base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="nombre">Nombre de la consulta dinámica.</param>
        /// <param name="tipoBaseDatos">Tipo de base de datos de la conexión.</param>
        /// <param name="tipoDAO">Clase mediante reflexión que desea acceder a la base de datos.</param>
        /// <returns></returns>
        public static QueryTag ObtenerConsulta(string nombre, string tipoBaseDatos, Type tipoDAO)
        {
            DynamicQueryRoot root = ObtenerConfiguracion(tipoDAO);
            if (root != null)
            {
                int cantidad = root.Consultas.Count(c => c.Nombre == nombre);

                if (cantidad == 1)
                {
                     DynamicQueryTag tag = root.Consultas.First(c => c.Nombre == nombre);
                     cantidad = tag.Consultas.Count(c => c.TipoBaseDatos == tipoBaseDatos);

                     if (cantidad == 1)
                     {
                         return tag.Consultas.First(c => c.TipoBaseDatos == tipoBaseDatos);
                     }
                     else if (cantidad == 0)
                     {
                         throw new PreparacionException(string.Format("¡No se ha encontrado la consulta dinámica {0} para {1}!", nombre, tipoBaseDatos));
                     }
                     else
                     {
                         throw new PreparacionException(string.Format("¡Exite más de una consulta dinámica con el nombre {0} y para {1}!", nombre, tipoBaseDatos));
                     }
                     
                }
                else if (cantidad == 0)
                {
                    throw new PreparacionException(string.Format("¡No se ha encontrado la consulta dinámica {0}!", nombre));
                }
                else
                {
                    throw new PreparacionException(string.Format("¡Exite más de una consulta dinámica con el nombre {0}!", nombre));
                }
            }
            throw new PreparacionException("¡La configuración de Dynamic Query no posee definiciones!");
        }

        /// <summary>
        /// Obtiene la configuración desde el archivo XML.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="claseDAO">Tipo de la clase de un DAO.</param>
        /// <returns></returns>
        public static DynamicQueryRoot ObtenerConfiguracion(Type claseDAO)
        {
            IMonitorArchivo monitor = MonitorArchivoFactory.ObtenerArchivo();
            string DynamicQueryConfigPath = FrameworkConfigurationManager.ObtenerRutaArchivoConfiguracion(CARPETA_DYNAMIC_QUERIES);

            string ruta = monitor.ObtenerRutaArchivo(DynamicQueryConfigPath, claseDAO.FullName + ".dbq.xml");
            string clave = null;
            
            if (monitor.Existe(ruta))
            {
                DynamicQueryConfigPath = ruta;
                clave = claseDAO.FullName;
            }
            else
            {
                DynamicQueryConfigPath = monitor.ObtenerRutaArchivoExistente(DynamicQueryConfigPath, claseDAO.Name + ".dbq.xml");
                clave = claseDAO.Name;
            }
            
            if ( (!ArchivosConsultas.ContainsKey(clave)) || (monitor.FueModificado(ArchivosConsultas[clave].FechaUltimaModificacion, DynamicQueryConfigPath)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DynamicQueryRoot));
                FileStream file = null;
                DynamicQueryRoot retorno = null;

                try
                {
                    file = new FileStream(DynamicQueryConfigPath, FileMode.Open);
                    retorno = serializer.Deserialize(file) as DynamicQueryRoot;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (file != null)
                    {
                        file.Close();
                    }
                }
                
                if (retorno != null)
                {
                    Configuracion = retorno;
                }

                if (!ArchivosConsultas.ContainsKey(clave))
                {
                    ArchivosConsultas.Add(clave, retorno);
                }

                ArchivosConsultas[clave].FechaUltimaModificacion = monitor.ObtenerFechaUltimaModificacion(DynamicQueryConfigPath);
            }

            return Configuracion;
        }
    }
}
