using System;
using System.Data;

namespace Ada.Framework.Data.DBConnector.Entities.Parameter
{
    /// <summary>
    /// Representación de un parámetro de un Procedimiento Almacenado. Esta clase no puede heredarse.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public sealed class ParametroSql
    {
        /// <summary>
        /// Permite obtener o establecer el nombre del parámetro.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Nombre { get; set; }

        /// <summary>
        /// Permite obtener o establecer el tipo Sql del parámetro.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public Nullable<DbType> Tipo { get; set; }

        /// <summary>
        /// Permite obtener o establecer el valor C# del parámetro.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public object Valor { get; set; }

        /// <summary>
        /// Obtiene el valor de un parámetro, para ser reemplazado en una consulta SQL.
        /// Incluye comillas simples a la respuesta si el tipo de parámetro lo requiere.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <returns></returns>
        public string ObtenerValor()
        {
            if(Valor is string)
            {
                return string.Format("'{0}'", Valor.ToString());
            }
            else if (Valor is DateTime)
            {
                return string.Format("'{0}'", Valor.ToString());
            }
            return Valor.ToString();
        }
    }
}
