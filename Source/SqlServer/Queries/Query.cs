using Ada.Framework.Data.DBConnector;
using Ada.Framework.Data.DBConnector.Entities.Query;
using Ada.Framework.Data.DBConnector.Exceptions;
using Ada.Framework.Data.DBConnector.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ada.Framework.Data.DBConnector.SqlServer.Queries
{
    /// <summary>
    /// Representación de una consulta a base de datos relacional. Esta clase no puede heredarse.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    internal sealed class Query : Data.DBConnector.Queries.Query
    {
        /// <summary>
        /// Constructor que inicializa las propiedades.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="conexion">Representación de la conexión a una base de datos relacional.</param>
        /// <param name="mapeadorObjetos">Mapeador encargado de convertir la respuesta de una consulta a un objeto.</param>
        /// <param name="creadorQuery">Creador de queries como string.</param>
        public Query(ConexionBaseDatos conexion, MapeadorDeObjetos mapeadorObjetos, IQueryCreator creadorQuery)
            : base(conexion, mapeadorObjetos, creadorQuery) { }
    }
}
