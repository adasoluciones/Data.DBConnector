using System;

namespace Ada.Framework.Data.DBConnector.Exceptions
{
    /// <summary>
    /// Clase que representa una excepción al ejecutar una operación en base de datos. Esta clase no puede heredarse.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    [Serializable]
    public class EjecutarException : DBConnectorException
    {
        /// <summary>
        /// Constructor sin parámetros.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public EjecutarException() : base() { }

        /// <summary>
        /// Constructor de la clase que proporciona un mensaje de descripción.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="mensaje">Mensaje de descripción.</param>
        public EjecutarException(string mensaje)
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
        public EjecutarException(string mensaje, Exception innerException)
            : base(mensaje, innerException) { }
    }
}
