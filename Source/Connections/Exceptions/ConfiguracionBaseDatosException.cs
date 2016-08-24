using System;

namespace Ada.Framework.Data.DBConnector.Connections.Exceptions
{
    /// <summary>
    /// Clase que representa una excepción lanzada por una configuración de las conexiones incorrecta. Esta clase no puede heredarse.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public sealed class ConfiguracionBaseDatosException:Exception
    {
        /// <summary>
        /// Constructor sin parámetros.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public ConfiguracionBaseDatosException() : base() { }

        /// <summary>
        /// Constructor de la clase que proporciona un mensaje de descripción.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="mensaje">Mensaje de descripción.</param>
        public ConfiguracionBaseDatosException(string mensaje)
            : base(mensaje) { }

        /// <summary>
        /// Constructor de la clase que proporciona un mensaje de descripción, y la excepcion específica que la ocacionó.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="mensaje">Mensaje de descripción.</param>
        /// <param name="innerException">Excepción que la lazó.</param>
        public ConfiguracionBaseDatosException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
