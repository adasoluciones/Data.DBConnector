using Ada.Framework.Data.DBConnector.Entities.Parameter;
using Ada.Framework.Data.DBConnector.Exceptions;
using Ada.Framework.Data.DBConnector.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Ada.Framework.Data.DBConnector.SqlServer.Queries.SP
{
    /// <summary>
    /// Representación de un SP (Store Procedure) o Procedimiento Almacenado.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    internal sealed class ProcedimientoAlmacenado : DBConnector.Queries.SP.ProcedimientoAlmacenado
    {
        /// <summary>
        /// Constructor que inicializa las propiedades.
        /// </summary>
        /// <param name="conexion">Conexión con base de datos.</param>
        /// <param name="mapeadorObjetos">Mapeador encargado de convertir la respuesta de una ejecución a un objeto.</param>
        /// <param name="creadorQuery">Creador de queries como string.</param>
        public ProcedimientoAlmacenado(ConexionBaseDatos conexion, MapeadorDeObjetos mapeadorObjetos, IQueryCreator creadorQuery)
            : base(conexion, mapeadorObjetos, creadorQuery) { }

        public override IDbCommand CargarParametros(ref IDbCommand comando)
        {
            if (comando == null) throw new NullReferenceException("¡El comando no puede ser nulo!");

            try
            {
                foreach (ParametroSql parametro in Parametros.Parametros)
                {
                    IDbDataParameter param = comando.CreateParameter();

                    if (!parametro.Tipo.HasValue)
                    {
                        if (parametro.Valor != null)
                        {
                            if (parametro.Valor.GetType() != typeof(DataTable))
                            {
                                parametro.Tipo = Mapeador.ObtenerTipoDestino(parametro.Valor.GetType());
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (parametro.Valor.GetType() != typeof(DataTable))
                    {
                        param.DbType = parametro.Tipo.Value;
                    }
                    else
                    {
                        (param as SqlParameter).SqlDbType = SqlDbType.Structured;
                    }
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
