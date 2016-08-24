using Ada.Framework.Data.DBConnector.Entities.Parameter;
using Ada.Framework.Data.DBConnector.Entities.Query;
using Ada.Framework.Data.DBConnector.Exceptions;
using Ada.Framework.Data.DBConnector.Mapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Ada.Framework.Data.DBConnector.Queries
{
    /// <summary>
    /// Representación de una consulta a base de datos relacional.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public abstract class Query
    {
        /// <summary>
        /// Contiene el diccionario que asocia el campo de la respuesta con una propiedad de un objeto.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        internal IDictionary<string, string> campoPropiedad;

        /// <summary>
        /// Obtiene o establece el creador de queries como string.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public IQueryCreator CreadorQuery { get; set; }

        /// <summary>
        /// Obtiene o establece la representación de la conexión a una base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        protected ConexionBaseDatos Conexion { get; set; }

        /// <summary>
        /// Obtiene o establece la consulta del comando.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public virtual string Consulta { get; set; }

        /// <summary>
        /// Obtine el diccionario que asocia el campo de la respuesta con una propiedad de un objeto.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public IDictionary<string, string> CampoPropiedad
        {
            get
            {
                return campoPropiedad;
            }
        }

        /// <summary>
        /// Obtiene el mapeador de la consulta.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <value>Mapeador encargado de convertir la respuesta de una consulta a un objeto.</value>
        public MapeadorDeObjetos Mapeador { get; set; }

        /// <summary>
        /// Permite obtener o establecer los parámetros de la consulta actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public virtual ColeccionParametroSql Parametros { get; set; }
        
        /// <summary>
        /// Constructor que inicializa las propiedades.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        ///         2.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): Se añade IQueryCreator.
        /// </remarks>
        /// <param name="conexion">Representación de la conexión a una base de datos relacional.</param>
        /// <param name="mapeadorObjetos">Mapeador encargado de convertir la respuesta de una consulta a un objeto.</param>
        /// <param name="creadorQuery">Creador de queries como string.</param>
        public Query(ConexionBaseDatos conexion, MapeadorDeObjetos mapeadorObjetos, IQueryCreator creadorQuery)
        {
            Conexion = conexion;
            campoPropiedad = new Dictionary<string, string>();
            Mapeador = mapeadorObjetos;
            CreadorQuery = creadorQuery;
            Parametros = new ColeccionParametroSql();
        }

        /// <summary>
        /// Crea un comando para la conexión actual. Además de establecer el tipo de comando como SP y
        /// la consulta como la especificada en la propiedad <see cref="Ada.Framework.Data.DBConnector.Queries.Query.Consulta"/>.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Implementación de un comando sql.</returns>
        /// <exception cref="System.ArgumentException">Excepción lanzada al recivir una consulta nula o vacia.</exception>
        public virtual IDbCommand CrearComando()
        {
            IDbCommand retorno = Conexion.dbConnection.CreateCommand();
            retorno.CommandType = CommandType.Text;
            retorno.CommandText = Consulta;
            if (Conexion.Transaccion != null)
            {
                retorno.Transaction = Conexion.Transaccion.DbTransaction;
            }
            return retorno;
        }

        /// <summary>
        /// Obtiene un elemento (1 fila) de la respuesta obtenida al ejecutar la instancia actual (Procedimiento Almacenado).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <typeparam name="T">Tipo de la respuesta (fila).</typeparam>
        /// <returns>Una instancia cargada del tipo especificado, y datos retornados por la ejecución.</returns>
        /// <exception cref="System.ArgumentException">Excepción lanzada al no establecer el nombre del procedimiento almacenado.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.EjecutarException">Excepción lanzada al ocurrir un error al realizar la consulta.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual RespuestaEjecucion<T> Obtener<T>()
        {
            try
            {
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IDbCommand comando = CrearComando();
                IDataReader reader = comando.ExecuteReader();

                RespuestaEjecucion<T> retorno = new RespuestaEjecucion<T>();

                if (reader.Read())
                {
                    retorno = Mapeador.Mapear<T>(reader, CampoPropiedad);
                }

                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Closed) Conexion.dbConnection.Close();

                return retorno;
            }
            catch (Exception e)
            {
                throw new EjecutarException(e.Message, e);
            }
        }

        /// <summary>
        /// Obtiene un elemento (1 fila) de la respuesta obtenida al ejecutar la instancia actual (Procedimiento Almacenado).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Diccionario cuya clave es el nombre del campo.</returns>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.EjecutarException">Excepción lanzada al ocurrir un error al realizar la consulta.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual IDictionary<string, object> Obtener()
        {
            try
            {
                IDictionary<string, object> retorno = new Dictionary<string, object>();
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IDbCommand comando = CrearComando();
                IDataReader reader = comando.ExecuteReader();

                retorno = Mapeador.ObtenerFila(reader);

                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Closed) Conexion.dbConnection.Close();
                return retorno;
            }
            catch (Exception e)
            {
                throw new EjecutarException(e.Message, e);
            }
        }

        /// <summary>
        /// Permite realizar una ejecución sin respuesta a una base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.EjecutarException">Excepción lanzada al ocurrir un error al realizar la consulta.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual void Ejecutar()
        {
            try
            {
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IDbCommand comando = CrearComando();
                comando.ExecuteNonQuery();

                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Closed) Conexion.dbConnection.Close();
            }
            catch (Exception e)
            {
                throw new EjecutarException(e.Message, e);
            }
        }

        /// <summary>
        /// Obtiene una colección de elementos, a partir de la respuesta obtenida al ejecutar la instancia actual (Procedimiento Almacenado).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <typeparam name="T">Tipo de la respuesta (entidad).</typeparam>
        /// <returns>Colección del tipo especificado.</returns>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.EjecutarException">Excepción lanzada al ocurrir un error al realizar la consulta.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual IList<RespuestaEjecucion<T>> Consultar<T>()
        {
            try
            {
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IList<RespuestaEjecucion<T>> retorno = new List<RespuestaEjecucion<T>>();

                IDbCommand comando = CrearComando();
                IDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    retorno.Add(Mapeador.Mapear<T>(reader, CampoPropiedad));
                }

                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Closed) Conexion.dbConnection.Close();

                return retorno;
            }
            catch (Exception e)
            {
                throw new EjecutarException(e.Message, e);
            }
        }

        /// <summary>
        /// Obtiene una colección de elementos, a partir de la respuesta obtenida al ejecutar la instancia actual (Procedimiento Almacenado).
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Colección de diccionarios cuya clave corresponde al nombre de los campos obtenidos.</returns>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.EjecutarException">Excepción lanzada al ocurrir un error al realizar la consulta.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual IList<IDictionary<string, object>> Consultar()
        {
            try
            {
                IList<IDictionary<string, object>> retorno = new List<IDictionary<string, object>>();

                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IDbCommand comando = CrearComando();
                IDataReader reader = comando.ExecuteReader();

                retorno = Mapeador.ObtenerFilas(reader);

                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Closed) Conexion.dbConnection.Close();

                return retorno;
            }
            catch (Exception e)
            {
                throw new EjecutarException(e.Message, e);
            }
        }

        /// <summary>
        /// Carga los parámetros definidos, al comando recivido como párametro.
        /// Se sugiere crear el comando con el método <see cref="Ada.Framework.Data.DBConnector.Queries.SP.ProcedimientoAlmacenado.CrearComando"/>.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="comando">Comando sql.</param>
        /// <returns>Implementación de un comando sql.</returns>
        public virtual IDbCommand CargarParametros(ref IDbCommand comando)
        {
            if (comando == null)
            {
                comando = CrearComando();
            }

            try
            {
                foreach (ParametroSql parametro in Parametros.Parametros)
                {
                    parametro.Nombre = parametro.Nombre.Replace("@", "");
                    Consulta = Consulta.Replace(string.Format("@{0}", parametro.Nombre), parametro.ObtenerValor());
                }
            }
            catch (Exception e)
            {
                throw new EjecutarException("¡Error al cargar los parámetros de entrada del SP!", e);
            }

            return comando;
        }
    }
}
