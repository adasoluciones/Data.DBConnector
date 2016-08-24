using Ada.Framework.Data.DBConnector.Exceptions;
using System;
using System.Data;
using System.Xml.Serialization;

namespace Ada.Framework.Data.DBConnector.Transaction
{
    /// <summary>
    /// Representación de una transacción Sql.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public abstract class Transaccion
    {
        /// <summary>
        /// Obtiene o establece la representación de la conexión a una base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlIgnore]
        internal IDbConnection Conexion { get; set; }

        /// <summary>
        /// Obtiene o establece la epresentación una transacción que se debe realizar en un origen de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        [XmlIgnore]
        internal IDbTransaction DbTransaction { get; set; }

        /// <summary>
        /// Obtiene o establece el comportamiento de bloqueo de la transacción para la conexión.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        protected IsolationLevel IsolationLevel { get; set; }

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
        public Transaccion(IDbConnection conexion, IsolationLevel isolationLevel)
        {
            Conexion = conexion;
            IsolationLevel = isolationLevel;
        }

        /// <summary>
        /// Comienza la transacción en una conexión y abre la conexión.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public virtual void Iniciar()
        {
            try
            {
                if (Conexion.State != ConnectionState.Open)
                {
                    Conexion.Open();
                }
                DbTransaction = Conexion.BeginTransaction(IsolationLevel);
            }
            catch (Exception e)
            {
                if (Conexion.State == ConnectionState.Open)
                {
                    Conexion.Close();
                }
                throw new ConexionException(e.Message, e);                
            }
        }

        /// <summary>
        /// Confirma la transacción a la base de datos y cierra automáticamente la conexión.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public virtual void Commit()
        {
            try
            {
                DbTransaction.Commit();
            }
            catch (Exception e)
            {
                throw new EjecutarException("¡Error al realizar Commit a la transacción!", e);
            }
            finally
            {
                Conexion.Close();
            }
        }

        /// <summary>
        /// Deshace una transacción desde un estado pendiente y cierra automáticamente la conexión.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public virtual void RollBack()
        {
            try
            {
                DbTransaction.Rollback();
            }
            catch (Exception e)
            {
                throw new EjecutarException("¡Error al realizar Rollback de la transacción!", e);
            }
            finally
            {
                Conexion.Close();
            }
        }
    }
}
