using Ada.Framework.Core.CustomAttributes.Object;
using Ada.Framework.Data.DBConnector.Entities.Query;
using Ada.Framework.Util.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace Ada.Framework.Data.DBConnector.Mapper
{
    /// <summary>
    /// Contiene las funciones para mapear datos y filas en tipos C# y TO respectivamente.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public abstract class MapeadorDeObjetos
    {
        /// <summary>
        /// Obtiene o establece un valor que indica si se deben quitar los caracteres de espacio en
        /// blanco del principio y el final de cada valor.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public bool RecortarTexto { get; set; }

        /// <summary>
        /// Contiene las relaciones entre Tipos de C# y Sql.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private Dictionary<Type, DbType> dbTypeWrapper = new Dictionary<Type, DbType>();

        /// <summary>
        /// Define las relaciones entre Tipos de C# y Sql.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        protected Dictionary<Type, DbType> DbTypeWrapper { get { return dbTypeWrapper; } }

        /// <summary>
        /// Inicializa el mapeador asociando los tipos de datos correspondientes.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public MapeadorDeObjetos()
        {
            DbTypeWrapper.Add(typeof(string), DbType.String);
            DbTypeWrapper.Add(typeof(byte[]), DbType.Binary);
            DbTypeWrapper.Add(typeof(bool), DbType.Boolean);
            DbTypeWrapper.Add(typeof(byte), DbType.Byte);
            DbTypeWrapper.Add(typeof(DateTime), DbType.Date);
            DbTypeWrapper.Add(typeof(DateTimeOffset), DbType.DateTimeOffset);
            DbTypeWrapper.Add(typeof(decimal), DbType.Decimal);
            DbTypeWrapper.Add(typeof(double), DbType.Double);
            DbTypeWrapper.Add(typeof(Guid), DbType.Guid);
            DbTypeWrapper.Add(typeof(Int16), DbType.Int16);
            DbTypeWrapper.Add(typeof(Int32), DbType.Int32);
            DbTypeWrapper.Add(typeof(Int64), DbType.Int64);
            DbTypeWrapper.Add(typeof(object), DbType.Object);
            DbTypeWrapper.Add(typeof(sbyte), DbType.SByte);
            DbTypeWrapper.Add(typeof(Single), DbType.Single);
            DbTypeWrapper.Add(typeof(char), DbType.StringFixedLength);
            DbTypeWrapper.Add(typeof(TimeSpan), DbType.Time);
            DbTypeWrapper.Add(typeof(UInt16), DbType.UInt16);
            DbTypeWrapper.Add(typeof(UInt32), DbType.UInt32);
            DbTypeWrapper.Add(typeof(UInt64), DbType.UInt64);
            DbTypeWrapper.Add(typeof(DataTable), DbType.Object);
        }

        /// <summary>
        /// Mapea la respuesta de la ejecución de un comando a el tipo C# especificado.
        /// Puede ser un objeto definido por el usuario, por ejemplo, un TO.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <typeparam name="T">Tipo de la respuesta (entidad).</typeparam>
        /// <param name="record">Respuesta obtenida de la ejecución de un comando en una base de datos.</param>
        /// <param name="campoPropiedad">Diccionario que asocia el campo de la respuesta con una propiedad de un objeto.</param>
        /// <returns>Una instancia cargada del tipo especificado y datos obtenidos de IDataReader (parámetro).</returns>
        /// <exception cref="System.InvalidOperationException">Excepción lanzada al ocurrir un erro al mapear la respuesta.</exception>
        public virtual RespuestaEjecucion<T> Mapear<T>(IDataRecord record, IDictionary<string, string> campoPropiedad)
        {
            if (record == null) throw new NullReferenceException("¡Record no puede ser nulo!");
            if (campoPropiedad == null) throw new NullReferenceException("¡CampoPropiedad no puede ser nulo!");

            try
            {
                RespuestaEjecucion<T> retorno = new RespuestaEjecucion<T>();

                //Si es un objeto definido por el usuario.
                if (!ObjetoFactory.ObtenerObjeto().esPrimitivo(typeof(T)))
                {
                    bool esInmutable = false;

                    //Obtener los atributos personalizados (Annotations) del tipo.
                    var attributos = typeof(T).GetCustomAttributes(false);

                    foreach (var atributo in attributos)
                    {
                        if (atributo is Inmutable)
                        {
                            esInmutable = (atributo as Inmutable).EsInmutable;
                        }
                    }

                    if (!esInmutable)
                    {
                        retorno.Respuesta = Activator.CreateInstance<T>();

                        IList<PropertyInfo> parametros = typeof(T).GetProperties();

                        for (int x = 0; x < record.FieldCount; x++)
                        {
                            string campo = record.GetName(x);

                            bool existeCampo = false;

                            PropertyInfo[] propiedades = retorno.Respuesta.GetType().GetProperties();

                            foreach (PropertyInfo propiedad in propiedades)
                            {
                                //Comprueba existencia de Property con mismo nombre que el campo y el mismo tipo.
                                if (propiedad.Name == campo)
                                {
                                    if (record.GetValue(x).GetType() == propiedad.PropertyType)
                                    {
                                        object valor = record[x];
                                        if (valor.GetType() == typeof(DBNull))
                                        {
                                            valor = null;
                                        }
                                        if (valor is string && RecortarTexto)
                                        {
                                            valor = valor.ToString().Trim();
                                        }
                                        propiedad.SetValue(retorno.Respuesta, valor, null);
                                        existeCampo = true;
                                        break;
                                    }
                                    else if (ObjetoFactory.ObtenerObjeto().esEnumeracion(propiedad.PropertyType))
                                    {
                                        try
                                        {
                                            object valorEnum = Enum.Parse(propiedad.PropertyType, record.GetString(x));
                                            propiedad.SetValue(retorno.Respuesta, valorEnum, null);
                                        }
                                        catch { }
                                    }

                                }
                            }

                            //Si no existe una coincidencia directa (nombre) entre Campo y Propiedad.
                            if (!existeCampo)
                            {
                                //Obtiene desde el diccionario la propiedad correspondiente al campo y carga el valor.
                                object valor = record[x];
                                if (valor is string && RecortarTexto)
                                {
                                    valor = valor.ToString().Trim();
                                
                                }

                                //Comprueba existencia de Mapeador entre Campo y Propiedad.
                                if (campoPropiedad!=null)
                                {
                                    bool existePropiedad = false;

                                    foreach (string xpropCampoPropiedad in campoPropiedad.Keys)
                                    {
                                        if (valor.GetType() == typeof(DBNull))
                                        {
                                            valor = null;
                                        }

                                        if (xpropCampoPropiedad.Contains(campo))
                                        {
                                            existePropiedad = true;
                                            ObjetoFactory.ObtenerObjeto().SetValorPropiedad(retorno.Respuesta, xpropCampoPropiedad, valor);
                                        }
                                    }

                                    if (!existePropiedad)
                                    {
                                        //Agrega el campo a las propiedades omitidas.
                                        retorno.PropiedadesOmitidas.Add(campo, valor);
                                    }
                                }
                                else
                                {
                                    //Agrega el campo a las propiedades omitidas.
                                    retorno.PropiedadesOmitidas.Add(campo, valor);
                                }
                            }
                        }

                        return retorno;
                    }
                    else
                    {
                        for (int x = 0; x < record.FieldCount; x++)
                        {
                            string campo = record.GetName(x);
                            object valor = record.GetValue(x);
                            if (valor is string && RecortarTexto)
                            {
                                valor = valor.ToString().Trim();
                            }
                            retorno.PropiedadesOmitidas.Add(campo, valor);
                        }

                        return retorno;
                    }
                }
                else
                {
                    object valor = record.GetValue(0);
                    if (valor is string && RecortarTexto)
                    {
                        valor = valor.ToString().Trim();
                    }
                    return new RespuestaEjecucion<T>() { Respuesta = (T)Convert.ChangeType(valor, typeof(T), CultureInfo.InvariantCulture) };
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("¡No se ha podido convertir la respuesta al tipo especificado!", e);
            }
        }

        /// <summary>
        /// Obtiene la respuesta de la ejecución de un comando de manera encapsulada, ordenada por fila y columna.
        /// Cada diccionario contiene como clave el nombre de las columnas.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="reader">Respuesta de comando.</param>
        /// <returns>Encapsulación de respuesta.</returns>
        public virtual IList<IDictionary<string, object>> ObtenerFilas(IDataReader reader)
        {
            IList<IDictionary<string, object>> retorno = new List<IDictionary<string, object>>();

            while (reader.Read())
            {
                retorno.Add(new Dictionary<string, object>());
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object valor = reader[i];
                    if (valor is string && RecortarTexto)
                    {
                        valor = valor.ToString().Trim();
                    }
                    retorno[retorno.Count - 1].Add(reader.GetName(i), valor);
                }
            }

            return retorno;
        }

        /// <summary>
        /// Obtiene la respuesta de la ejecución de un comando de manera encapsulada, ordenada por fila y columna.
        /// Cada diccionario contiene como clave el nombre de las columnas.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="reader">Respuesta de comando.</param>
        /// <returns>Encapsulación de respuesta.</returns>
        public virtual IDictionary<string, object> ObtenerFila(IDataReader reader)
        {
            IDictionary<string, object> retorno = new Dictionary<string, object>();

            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object valor = reader[i];
                    if (valor is string && RecortarTexto)
                    {
                        valor = valor.ToString().Trim();
                    }
                    retorno.Add(reader.GetName(i), valor);
                }
            }

            return retorno;
        }

        /// <summary>
        /// Obtiene el tipo equivalente en Sql para el tipo C#.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="tipo">Tipo de C#.</param>
        /// <returns>Tipo de Sql.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Excepción lanzada al no existir un mapeador compatible para el tipo.</exception>
        public virtual DbType ObtenerTipoDestino(Type tipo)
        {
            if (tipo == null) throw new NullReferenceException("¡El objeto no puede ser nulo!");

            if (DbTypeWrapper.ContainsKey(tipo))
            {
                return DbTypeWrapper[tipo];
            }

            throw new KeyNotFoundException("No existe un mapeador para el tipo " + tipo.Name + " !");
        }

        /// <summary>
        /// Obtiene el tipo equivalente en C# para el tipo Sql.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="tipo">Tipo de Sql.</param>
        /// <returns>Tipo de C#.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Excepción lanzada al no existir un mapeador compatible para el tipo.</exception>
        public virtual Type ObtenerTipoDestino(DbType tipo)
        {
            foreach (KeyValuePair<Type, DbType> item in DbTypeWrapper)
            {
                if (item.Value == tipo)
                {
                    return item.Key;
                }
            }

            throw new KeyNotFoundException("No existe un mapeador para el tipo " + tipo.ToString() + " !");
        }
    }
}