using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Ada.Framework.Data.DBConnector.Entities.DataBase;

namespace Ada.Framework.Data.DBConnector.Connections.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlType(TypeName = "DBConnector")]
    public class ConfiguracionTO
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlArray(ElementName = "Connections")]
        public List<ConexionTO> Conexiones { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlArray(ElementName = "Connectors")]
        public List<ConectorTO> Conectores { get; set; }
    }
}
