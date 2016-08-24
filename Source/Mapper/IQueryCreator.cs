using Ada.Framework.Data.DBConnector.Queries;
using Ada.Framework.Data.DBConnector.Queries.SP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ada.Framework.Data.DBConnector.Mapper
{
    /// <summary>
    /// Interfaz que define el comportamiento de las implementaciónes que generan una query como texto.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public interface IQueryCreator
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
        string ObtenerQuery(ProcedimientoAlmacenado procedimientoAlmacenado);

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
        string ObtenerQuery(DynamicQuery dynamicQuery);
    }
}
