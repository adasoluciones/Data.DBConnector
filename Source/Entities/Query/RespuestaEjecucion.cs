using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ada.Framework.Data.DBConnector.Entities.Query
{
    /// <summary>
    /// Representación de la respuesta de ejecución de una comando. Esta clase no puede heredarse.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    /// <typeparam name="T">Tipo de respuesta.</typeparam>
    public sealed class RespuestaEjecucion<T>
    {
        /// <summary>
        /// Contiene las propiedades no mapeadas por DBConnector.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        internal IDictionary<string, object> propiedadesOmitidas;

        /// <summary>
        /// Constructor que instancia las propiedades del objeto.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public RespuestaEjecucion()
        {
            propiedadesOmitidas = new Dictionary<string, object>();
        }

        /// <summary>
        /// Obtiene o establece la respuesta de ejecución.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public T Respuesta { get; set; }

        /// <summary>
        /// Obtiene las propiedades no mapeadas por DBConnector.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public IDictionary<string, object> PropiedadesOmitidas { get { return propiedadesOmitidas; } }
    }
}
