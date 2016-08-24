using System;
using System.Collections.Generic;
using System.Data;

namespace Ada.Framework.Data.DBConnector.Entities.Parameter
{
    /// <summary>
    /// Representación de un DataTable.
    /// </summary>
    /// <remarks>
    ///     Registro de versiones:
    ///     
    ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
    /// </remarks>
    public sealed class Tabla
    {
        /// <summary>
        /// Campo que contiene el <see cref="System.Data.DataTable"/> que generará la entidad.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        internal DataTable dataTable = new DataTable();

        /// <summary>
        /// Diccionario que asocia el nombre de una columna con la propiedad del TO. Utilizado al cargar un TO.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private IDictionary<string, string> ColumnaPropiedad = new Dictionary<string, string>();

        /// <summary>
        /// Fila de <see cref="System.Data.DataTable"/> para agregar los valores.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        private DataRow Fila;

        /// <summary>
        /// Constructor sin parámetros que instancia los campos y propiedades de la instancia.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public Tabla()
        {
            Fila = dataTable.NewRow();
        }

        /// <summary>
        /// Agrega una columna a la estructura del <see cref="System.Data.DataTable"/>.
        /// Utilizar con método <see cref="Ada.Framework.Data.DBConnector.Entities.Parameter.Tabla.AgregarValor(string, object)"/>.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="nombre">Nombre de la columna.</param>
        public void AgregarColumna(string nombre)
        {
            dataTable.Columns.Add(nombre);
        }

        /// <summary>
        /// Agrega una columna especificando su tipo, a la estructura del <see cref="System.Data.DataTable"/>.
        /// Utilizar con método <see cref="Ada.Framework.Data.DBConnector.Entities.Parameter.Tabla.AgregarValor(string, object)"/>.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="nombre">Nombre de la columna.</param>
        /// <param name="tipo">Tipo del valor a asignar a la columna.</param>
        public void AgregarColumna(string nombre, Type tipo)
        {
            dataTable.Columns.Add(nombre, tipo);
        }

        /// <summary>
        /// Asocia el valor de una columna con una propiedad del TO. Además agrega la columna a la estructura del <see cref="System.Data.DataTable"/>.
        /// Utilizar con método <see cref="Ada.Framework.Data.DBConnector.Entities.Parameter.Tabla.Cargar"/>.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="columna">Nombre de la columna.</param>
        /// <param name="propiedad">Propiedad del TO.</param>
        public void AsociarPropiedad(string columna, string propiedad)
        {
            dataTable.Columns.Add(columna);
            ColumnaPropiedad.Add(columna, propiedad);
        }

        /// <summary>
        /// Asocia el valor de una columna con una propiedad del TO. Además agrega la columna y su tipo a la estructura del <see cref="System.Data.DataTable"/>.
        /// Utilizar con método <see cref="Ada.Framework.Data.DBConnector.Entities.Parameter.Tabla.Cargar"/>.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="columna">Nombre de la columna.</param>
        /// <param name="tipo">Tipo del valor a asignar a la columna.</param>
        /// <param name="propiedad">Propiedad del TO.</param>
        public void AsociarPropiedad(string columna, Type tipo, string propiedad)
        {
            dataTable.Columns.Add(columna, tipo);
            ColumnaPropiedad.Add(columna, propiedad);
        }

        /// <summary>
        /// Agregar un valor a la fila actual.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <param name="columna">Nombre de la columna.</param>
        /// <param name="valor">Valor de la columna.</param>
        public void AgregarValor(string columna, object valor)
        {
            Fila[columna] = valor;
        }

        /// <summary>
        /// Agrega la fila al <see cref="System.Data.DataTable"/> y crea una nueva fila.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        public void AgregarFila()
        {
            dataTable.Rows.Add(Fila);
            Fila = dataTable.NewRow();
        }

        /// <summary>
        /// Carga los datos de una colección de objetos según las asociaciones de propiedad.
        /// </summary>
        /// <remarks>
        ///     Registro de versiones:
        ///     
        ///         1.0 02/03/2015 Marcos Abraham Hernández Bravo (Ada Ltda.): versión inicial.
        /// </remarks>
        /// <typeparam name="T">Tipo del TO.</typeparam>
        /// <param name="elementos">Colección de elementos.</param>
        public void Cargar<T>(IList<T> elementos)
        {
            foreach (var elemento in elementos)
            {
                DataRow fila = dataTable.NewRow();
                foreach (KeyValuePair<string, string> par in ColumnaPropiedad)
                {
                    object valor = elemento.GetType().GetProperty(par.Value).GetValue(elemento, null);
                    fila[par.Key] = valor;
                }
                dataTable.Rows.Add(fila);
            }
        }
    }
}
