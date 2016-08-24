using Ada.Framework.Data.DBConnector.Entities.Parameter;
using Ada.Framework.Data.DBConnector.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ada.Framework.Data.DBConnector.Queries
{
    /// <summary>
    /// Representación de una consulta a la base de datos que puede cambiar de implementación.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public sealed class DynamicQuery : Query
    {
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
        public DynamicQuery(ConexionBaseDatos conexion, MapeadorDeObjetos mapeadorObjetos, IQueryCreator creadorQuery) : base(conexion, mapeadorObjetos, creadorQuery) { }

        /// <summary>
        /// Obtiene o establece la consulta del comando.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public new string Consulta
        {
            internal set
            {
                base.Consulta = value;
            }
            get
            {
                return base.Consulta;
            }
        }

        /// <summary>
        /// Obtiene o establece el nombre de la consulta dinámica.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Nombre { get; internal set; }

        /// <summary>
        /// Obtiene o establece el tipo de base de datos al que corresponde.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 31/07/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string TipoBaseDatos { get; internal set; }

        /// <summary>
        /// Obtiene o establece el tipo de consulta. Puede ser una ProcedimientoAlmacenado(StoreProcedure) o una Consulta directa (Query). 
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 08/10/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public string Tipo { get; internal set; }

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
        public override System.Data.IDbCommand CrearComando()
        {
            System.Data.IDbCommand retorno = base.CrearComando();
            retorno.CommandText = CreadorQuery.ObtenerQuery(this);
            return retorno;
        }
    }
}
