using Ada.Framework.Data.DBConnector.Entities.DataBase;
using Ada.Framework.Data.DBConnector.Connections.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Ada.Framework.Data.DBConnector
{
    /// <summary>
    /// Factoría de la conexión a base de datos.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public static class ConexionBaseDatosFactory
    {
        /// <summary>
        /// Obtiene una implementación de la conexion a base de datos.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="db">Base de datos.</param>
        /// <returns>Implementación especificada como instancia.</returns>
        /// <exception cref="ConfiguracionBaseDatosException">¡No se puede crear la instancia de base de datos  + db.Instancia + !</exception>
        public static ConexionBaseDatos ObtenerConexionBaseDatos(ConexionTO db)
        {
            try
            {
                if (db.AssemblyPath!=null)
                {
                    db.AssemblyPath = db.AssemblyPath.Trim();
                }
                if(!string.IsNullOrEmpty(db.AssemblyPath))
                {
                    Assembly ensamblado = Assembly.LoadFile(db.AssemblyPath);
                    Type tipo = ensamblado.GetType(db.Instance);
                    return Activator.CreateInstance(tipo, new object[] { db }) as ConexionBaseDatos;
                }
                return Activator.CreateInstance(db.AssemblyName, db.Instance,
                                                    false, BindingFlags.CreateInstance, null,
                                                    new object[1] { db },
                                                    CultureInfo.InvariantCulture, null, null
                                                    ).Unwrap() as ConexionBaseDatos;
            }
            catch (Exception e)
            {
                throw new ConfiguracionBaseDatosException("¡No se puede crear la instancia de base de datos " + db.Instance + "!", e);
            }
        }
    }
}
