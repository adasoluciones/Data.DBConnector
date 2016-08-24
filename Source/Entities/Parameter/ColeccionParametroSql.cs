using Ada.Framework.Data.DBConnector.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Ada.Framework.Data.DBConnector.Entities.Parameter
{
    /// <summary>
    /// Representación de una colección de parametros para una consulta sql. Esta clase no puede heredarse.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public class ColeccionParametroSql
    {
        /// <summary>
        /// Contiene la lista de parametros.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private IList<ParametroSql> parametros;

        /// <summary>
        /// Permite obtener los parametros del procedimiento almacenado actual.
        /// Instancia la colección de no estarlo.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public IList<ParametroSql> Parametros 
        {
            get
            {
                if (parametros == null) parametros = new List<ParametroSql>();
                return parametros;
            } 
        }

        private object Objeto { get; set; }

        /// <summary>
        /// Permite obtener el número de parametros de la consulta.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public int Count { get { return Parametros.Count; } }

        /// <summary>
        /// Permite agregar un objeto como parámetro, para posteriormente acceder a sus propiedades.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 29/12/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="objeto"></param>
        public void AddTO(object objeto)
        {
            Objeto = objeto;
        }

        /// <summary>
        /// Agrega un parametro para la ejecución de un Procedimiento Almacenado.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="nombre">Nombre del parámetro.</param>
        /// <param name="valor">Valor del parámetro.</param>
        /// <seealso cref="Add(string, object, DbType?)"></seealso>
        public void Add(string nombre, object valor)
        {
            if (valor is Tabla)
            {
                valor = (valor as Tabla).dataTable;
            }
            Add(nombre, valor, null);
        }
        
        /// <summary>
        /// Agrega un parametro para la ejecución de un Procedimiento Almacenado.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="nombre">Nombre del parámetro.</param>
        /// <param name="valor">Valor del parámetro.</param>
        /// <param name="tipo">Tipo (Sql) del parámetro.</param>
        public void Add(string nombre, object valor, Nullable<DbType> tipo)
        {
            if (valor is Tabla)
            {
                valor = (valor as Tabla).dataTable;
            }
            Parametros.Add(new ParametroSql() { Nombre = nombre, Tipo = tipo, Valor = valor });
        }
    }
}
