using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace Ada.Framework.Data.DBConnector.SqlServer.Mapper
{
    /// <summary>
    /// Contiene las funciones para mapear datos y filas en tipos C# y TO respectivamente.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public sealed class MapeadorDeObjetos : Data.DBConnector.Mapper.MapeadorDeObjetos 
    {
        /// <summary>
        /// Inicializa el mapeador asociando los tipos de datos correspondientes.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public MapeadorDeObjetos() :base() { }
    }
}
