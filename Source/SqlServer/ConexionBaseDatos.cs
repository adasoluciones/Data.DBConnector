using Ada.Framework.Data.DBConnector.Entities.DataBase;
using Ada.Framework.Data.DBConnector.Exceptions;
using Ada.Framework.Data.DBConnector.SqlServer.Queries.SP;
using Ada.Framework.Data.DBConnector.SqlServer.Mapper;
using System;
using System.Data;
using System.Data.SqlClient;
using Ada.Framework.Data.DBConnector.Queries;

namespace Ada.Framework.Data.DBConnector.SqlServer
{
    /// <summary>
    /// Representa la conexión a una base de datos relacional.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    internal sealed class ConexionBaseDatos : Data.DBConnector.ConexionBaseDatos
    {
        /// <summary>
        /// Constructor que instancia el tipo de base de datos especificada.
        /// La conexión por defecto es SqlServer.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="db">Base de datos.</param>
        public ConexionBaseDatos(ConexionTO db)
           :base(db)
        {
            DBConnection = new SqlConnection(Conexion.ConnectionString);
        }

        /// <summary>
        /// Obtiene una implementación de la representación de una consulta a base de datos.
        /// </summary>
        /// <returns>Implementación de una  consulta a base de datos.</returns>
        /// <remarks>Registro de versiones:
        /// 1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.</remarks>
        public override Data.DBConnector.Queries.Query CrearQuery()
        {
            return new Queries.Query(this, new MapeadorDeObjetos(), new QueryCreator());
        }

        /// <summary>
        /// Obtiene una implementación de un procedimiento almacenado.
        /// </summary>
        /// <returns>Implementación de un procedimiento almacenado.</returns>
        public override Data.DBConnector.Queries.SP.ProcedimientoAlmacenado CrearProcedimientoAlmacenado()
        {
            return new Queries.SP.ProcedimientoAlmacenado(this, new MapeadorDeObjetos(),new QueryCreator());
        }

        /// <summary>
        /// Crea una transacción para agrupar las ejecuciones de la instancia actual. Además comienza la transacción y abre la conexión.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="isolationLevel">Especifica el comportamiento de bloqueo de la transacción para la conexión.Opcional.</param>
        public override void CrearTransaccion(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            transaccionInterna = new Transaction.Transaccion(DBConnection, isolationLevel);
            transaccionInterna.Iniciar();
        }
    }
}
