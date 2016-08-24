using Ada.Framework.Data.DBConnector.Entities.DataBase;
using Ada.Framework.Data.DBConnector.Exceptions;
using Ada.Framework.Data.DBConnector.Queries;
using Ada.Framework.Data.DBConnector.Queries.DynamicQueryConfig;
using Ada.Framework.Data.DBConnector.Queries.SP;
using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector
{
    /// <summary>
    /// Representa la conexión a una base de datos.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public abstract class ConexionBaseDatos
    {
        /// <summary>
        /// Obtiene o establece la base de datos de la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private ConexionTO conexion;

        /// <summary>
        /// Obtiene o establece la transacción que agrupa las operaciones creadas mediante la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        protected Transaction.Transaccion transaccionInterna;

        /// <summary>
        /// Permite obtener o establecer la representación de una conexión abierta a un origen de datos. La implementan los
        /// proveedores de datos de .NET Framework que tienen acceso a bases de datos relacionales. Es un enlace a la propiedad DBConnection.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlIgnore]
        internal IDbConnection dbConnection { get { return DBConnection; } set { DBConnection = value; } }

        /// <summary>
        /// Permite obtener o establecer la representación de una conexión abierta a un origen de datos. La implementan los
        /// proveedores de datos de .NET Framework que tienen acceso a bases de datos relacionales.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlIgnore]
        protected IDbConnection DBConnection { get; set; }

        /// <summary>
        /// Obtiene la base de datos de la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public ConexionTO Conexion { get { return conexion; } }

        /// <summary>
        /// Obtiene o establece el valor que indica si se debe abrir y cerrar automáticamente la conexión.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public bool AutoConectarse { get; set; }

        /// <summary>
        /// Obtiene la transacción que agrupa las operaciones creadas mediante la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public Transaction.Transaccion Transaccion { get { return transaccionInterna; } }

        /// <summary>
        /// Constructor que instancia el tipo de base de datos que representa la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="conexionBaseDatos">Base de datos.</param>
        public ConexionBaseDatos(ConexionTO conexionBaseDatos)
        {
            conexion = conexionBaseDatos;
        }
        
        /// <summary>
        /// Abre una conexión de base de datos con la configuración indicada por la propiedad <see cref="Ada.Framework.Data.DBConnector.Entities.DataBase.ConexionTO.ConnectionString"/> del la implementación.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        public virtual void Abrir()
        {
            if (DBConnection.State != ConnectionState.Open && !AutoConectarse && Transaccion == null)
            {
                if (string.IsNullOrEmpty(Conexion.ConnectionString))
                {
                    throw new ArgumentException("La cadena de conexión no puede estar vacia.");
                }

                DBConnection.ConnectionString = Conexion.ConnectionString;

                try
                {
                    DBConnection.Open();
                }
                catch (Exception e)
                {
                    throw new ConexionException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Cierra la conexión con la base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual void Cerrar()
        {
            if (DBConnection.State != ConnectionState.Closed && !AutoConectarse && Transaccion == null)
            {
                try
                {
                    DBConnection.Close();
                    DBConnection.Dispose();
                    DBConnection = null;
                }
                catch (Exception e)
                {
                    throw new ConexionException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// Crea una transacción para agrupar las ejecuciones de la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="isolationLevel">Especifica el comportamiento de bloqueo de la transacción para la conexión.Opcional.</param>
        public abstract void CrearTransaccion(IsolationLevel isolationLevel = IsolationLevel.Unspecified);

        /// <summary>
        /// Obtiene una implementación de la representación de una consulta a base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Implementación de una  consulta a base de datos.</returns>
        public abstract Query CrearQuery();

        /// <summary>
        /// Obtiene una implementación de un procedimiento almacenado.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Implementación de un procedimiento almacenado.</returns>
        public abstract ProcedimientoAlmacenado CrearProcedimientoAlmacenado();

        /// <summary>
        /// Obtiene una implementación de una consulta que puede cambiar según la base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 07/08/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="metodoDAO">Método mediante reflexión que desea acceder a la base de datos.</param>
        /// <param name="nombre">Nombre de la consulta en el archivo de configuración.</param>
        public DynamicQuery CrearQueryDinamica(MethodBase metodoDAO, string nombre)
        {
            return CrearQueryDinamica(metodoDAO.DeclaringType, nombre);
        }

        /// <summary>
        /// Obtiene una implementación de una consulta que puede cambiar según la base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 07/08/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="clase">Clase mediante reflexión, que contiene el método que desea acceder a la base de datos.</param>
        /// <param name="nombre">Nombre de la consulta en el archivo de configuración.</param>
        public DynamicQuery CrearQueryDinamica(Type clase, string nombre)
        {
            QueryTag consulta = DynamicQueryManager.ObtenerConsulta(nombre, conexion.Type, clase);

            Query aux = null;

            if (consulta.Tipo == "StoreProcedure")
            {
                aux = CrearProcedimientoAlmacenado();
            }
            else if (consulta.Tipo == "Query")
            {
                aux = CrearQuery();
            }
            else
            {
                throw new PreparacionException(string.Format("¡El tipo de consulta {0} no es válida!", consulta.Tipo));
            }

            DynamicQuery retorno = new DynamicQuery(this, aux.Mapeador, aux.CreadorQuery);
            retorno.TipoBaseDatos = conexion.Type;
            retorno.Nombre = nombre;
            retorno.Tipo = consulta.Tipo;
            retorno.Consulta = consulta.Consulta;
            return retorno;
        }
        
        /// <summary>
        /// Obtiene una implementación de una consulta que puede cambiar según la base de datos.
        /// La consulta es obtenida según la clase y el método que lo invocó.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 07/08/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="nombre">Nombre de la consulta en el archivo de configuración.</param>
        public DynamicQuery CrearQueryDinamica(string nombre)
        {
            MethodBase metodo = new StackTrace().GetFrame(1).GetMethod();
            return CrearQueryDinamica(metodo, nombre);
        }

        /// <summary>
        /// Obtiene una implementación de una consulta que puede cambiar según la base de datos.
        /// El nombre del archivo depende de la clase que lo invoque, y el nombre de la consulta
        /// es igual al nombre del método que lo invocó.
        /// </summary>
        /// <example>
        ///     <code>
        ///         public class UsuarioDAO
        ///         {
        ///             ...
        ///             public void Agregar(UsuarioTO usuario)
        ///             {
        ///                 Conexion.CrearQueryDinamica(); // El archivo será UsuarioDAO.dbq.xml (referencia), y la consulta será "Agregar".
        ///             }
        ///         }
        ///     </code>
        /// </example>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 07/08/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns></returns>
        public DynamicQuery CrearQueryDinamica()
        {
            MethodBase metodo = new StackTrace().GetFrame(1).GetMethod();
            return CrearQueryDinamica(metodo, metodo.Name);
        }
    }
}
