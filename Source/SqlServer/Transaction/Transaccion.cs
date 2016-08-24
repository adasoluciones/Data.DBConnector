using System.Data;

namespace Ada.Framework.Data.DBConnector.SqlServer.Transaction
{
    /// <summary>
    /// Representación de una transacción de SQL Server.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public class Transaccion : DBConnector.Transaction.Transaccion
    {
        /// <summary>
        /// Constructor que inicializa los campos de la transacción
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="conexion">Representación de la conexión a una base de datos.</param>
        /// <param name="isolationLevel">Comportamiento de bloqueo de la transacción para la conexión.</param>
        public Transaccion(IDbConnection conexion, IsolationLevel isolationLevel) : base(conexion, isolationLevel) { }
    }
}
