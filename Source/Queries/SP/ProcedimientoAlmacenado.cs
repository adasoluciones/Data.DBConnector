using Ada.Framework.Data.DBConnector.Entities.Parameter;
using Ada.Framework.Data.DBConnector.Entities.Query;
using Ada.Framework.Data.DBConnector.Exceptions;
using Ada.Framework.Data.DBConnector.Mapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Ada.Framework.Data.DBConnector.Queries.SP
{
    /// <summary>
    /// Representación de un SP (Store Procedure) o Procedimiento Almacenado.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public abstract class ProcedimientoAlmacenado : Query
    {
        /// <summary>
        /// Permite obtener o establecer el nombre del procedimiento almacenado que representa la instancia actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece la consulta del comando.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public new string Consulta
        {
            get
            {
                base.Consulta = CreadorQuery.ObtenerQuery(this);
                return base.Consulta;
            }
            protected set
            {
                base.Consulta = value;
            }
        }

        /// <summary>
        /// Constructor que inicializa las propiedades.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="conexion">Representación de la conexión a una base de datos relacional.</param>
        /// <param name="mapeadorObjetos">Mapeador que utilizará el procedimiento para transformar la respuesta de sus ejecuciones.</param>
        /// <param name="creadorQuery">Creador de queries como string.</param>
        public ProcedimientoAlmacenado(ConexionBaseDatos conexion, MapeadorDeObjetos mapeadorObjetos, IQueryCreator creadorQuery)
            : base(conexion, mapeadorObjetos, creadorQuery)
        {
            Parametros = new ColeccionParametroSql();
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
        public virtual new RespuestaEjecucion<T> Obtener<T>()
        {
            try
            {
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IDbCommand comando = CrearComando();

                IDataReader reader = CargarParametros(ref comando).ExecuteReader();

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
        /// <exception cref="System.ArgumentException">Excepción lanzada al no establecer el nombre del procedimiento almacenado.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.EjecutarException">Excepción lanzada al ocurrir un error al realizar la consulta.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual new IDictionary<string, object> Obtener()
        {
            try
            {
                IDictionary<string, object> retorno = new Dictionary<string, object>();
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IDbCommand comando = CrearComando();
                IDataReader reader = CargarParametros(ref comando).ExecuteReader();
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
        /// <exception cref="System.ArgumentException">Excepción lanzada al no establecer el nombre del procedimiento almacenado.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual new void Ejecutar()
        {
            try
            {
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IDbCommand comando = CrearComando();
                CargarParametros(ref comando).ExecuteNonQuery();

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
        /// <exception cref="System.ArgumentException">Excepción lanzada al no establecer el nombre del procedimiento almacenado.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual new IList<RespuestaEjecucion<T>> Consultar<T>()
        {
            try
            {
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IList<RespuestaEjecucion<T>> retorno = new List<RespuestaEjecucion<T>>();

                IDbCommand comando = CrearComando();

                IDataReader reader = CargarParametros(ref comando).ExecuteReader();

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
        /// <returns>Colección del tipo especificado.</returns>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.EjecutarException">Excepción lanzada al ocurrir un error al realizar la consulta.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al no establecer el nombre del procedimiento almacenado.</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al abrir la conexión.</exception>
        /// <exception cref="System.ArgumentException">Excepción lanzada al existir un problema con la cadena de conexión(ConnectionString).</exception>
        /// <exception cref="Ada.Framework.Data.DBConnector.Exceptions.ConexionException">Excepción lanzada al existir un problema al cerrar la conexión.</exception>
        public virtual new IList<IDictionary<string, object>> Consultar()
        {
            try
            {
                IList<IDictionary<string, object>> retorno = new List<IDictionary<string, object>>();

                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Open) Conexion.dbConnection.Open();

                IDbCommand comando = CrearComando();

                IDataReader reader = CargarParametros(ref comando).ExecuteReader();

                retorno = Mapeador.ObtenerFilas(reader);
                if (Conexion.AutoConectarse && Conexion.dbConnection.State != ConnectionState.Closed) Conexion.dbConnection.Close();

                reader.Close();
                reader.Dispose();
                reader = null;

                return retorno;
            }
            catch (Exception e)
            {
                throw new EjecutarException(e.Message, e);
            }
        }

        /// <summary>
        /// Crea un comando para la conexión actual. Además de establecer el tipo de comando como SP y
        /// el nombre como el indicado en la propiedad <see cref="Ada.Framework.Data.DBConnector.Queries.SP.ProcedimientoAlmacenado.Nombre"/>.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns>Implementación de un comando sql.</returns>
        /// <exception cref="System.ArgumentException">Excepción lanzada al no establecer el nombre del procedimiento almacenado.</exception>
        public virtual new IDbCommand CrearComando()
        {
            if (Nombre != null)
            {
                Nombre = Nombre.Trim();
            }
            if (string.IsNullOrEmpty(Nombre)) throw new ArgumentException("¡El nombre del procedimiento almacenado no puede ser nulo!");

            IDbCommand retorno = Conexion.dbConnection.CreateCommand();
            retorno.CommandType = CommandType.StoredProcedure;
            retorno.CommandText = Nombre;
            if (Conexion.Transaccion != null)
            {
                retorno.Transaction = Conexion.Transaccion.DbTransaction;
            }
            return retorno;
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
        public override IDbCommand CargarParametros(ref IDbCommand comando)
        {
            if (comando == null)
            {
                comando = CrearComando();
            }

            try
            {
                foreach (ParametroSql parametro in Parametros.Parametros)
                {
                    IDbDataParameter param = comando.CreateParameter();

                    if (!parametro.Tipo.HasValue)
                    {
                        if (parametro.Valor != null)
                        {
                            parametro.Tipo = Mapeador.ObtenerTipoDestino(parametro.Valor.GetType());
                        }
                        else
                        {
                            continue;
                        }
                    }

                    param.DbType = parametro.Tipo.Value;
                    param.Direction = ParameterDirection.Input;
                    param.Value = parametro.Valor;
                    param.ParameterName = parametro.Nombre;

                    comando.Parameters.Add(param);
                }
                Consulta = CreadorQuery.ObtenerQuery(this);
                return comando;
            }
            catch (Exception e)
            {
                throw new EjecutarException("¡Error al cargar los parámetros de entrada del SP!", e);
            }
        }
    }
}