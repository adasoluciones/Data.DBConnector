using Ada.Framework.Data.DBConnector.Entities.Parameter;
using Ada.Framework.Data.DBConnector.Mapper;
using Ada.Framework.Data.DBConnector.Queries;
using Ada.Framework.Data.DBConnector.Queries.SP;

namespace Ada.Framework.Data.DBConnector.SqlServer.Mapper
{
    /// <summary>
    /// Creador de consultas como texto.
    /// </summary>
    public sealed class QueryCreator : IQueryCreator
    {
        /// <summary>
        /// Permite obtener el cadena sql para ejecutar un procedimiento almacenado.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="procedimientoAlmacenado">Procedimiento almacenado.</param>
        /// <returns>Representación en cadena de caracteres.</returns>
        public string ObtenerQuery(ProcedimientoAlmacenado procedimientoAlmacenado)
        {
            string retorno = string.Empty;

            retorno = string.Format("EXEC {0} ", procedimientoAlmacenado.Nombre);

            bool esPrimerParametro = true;
            foreach (ParametroSql parametro in procedimientoAlmacenado.Parametros.Parametros)
            {
                if (!esPrimerParametro)
                {
                    retorno += ", ";
                }
                if (parametro.Valor == null)
                {
                    retorno += "NULL";
                }
                else
                {
                    if (parametro.Valor is string)
                    {
                        retorno += string.Format("'{0}'", parametro.Valor);
                    }
                    else
                    {
                        retorno += string.Format("{0}", parametro.Valor);
                    }
                }
                esPrimerParametro = false;
            }
            retorno += ";";

            return retorno;
        }

        /// <summary>
        /// Genera la consulta final remplazando los parametros en la consulta dinámica según corresponda.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="dynamicQuery">Consulta dinámica.</param>
        /// <returns>Representación en cadena de caracteres.</returns>
        public string ObtenerQuery(DynamicQuery dynamicQuery)
        {
            string retorno = dynamicQuery.Consulta;

            foreach (ParametroSql parametro in dynamicQuery.Parametros.Parametros)
            {
                retorno = retorno.Replace("@" + parametro.Nombre, parametro.ObtenerValor());
            }

            return retorno;
        }
    }
}
