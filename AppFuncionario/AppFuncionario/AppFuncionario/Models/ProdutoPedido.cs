using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace AppFuncionario
{
    public class ProdutoPedido
    {
        public Produto p { get; set; }
        public int quantidades { get; set; }

        public ProdutoPedido()
        {
            p = new Produto();
            quantidades = 0;
        }

        public ProdutoPedido(Produto pr, int qua)
        {
            p = pr;
            quantidades = qua;
        }

        public byte[] SavetoBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            XmlSerializer XML = new XmlSerializer(typeof(Produto));
            XML.Serialize(ms, this);
            ms.Close();
            return ms.ToArray();
        }

        public static Produto loadFromBytes(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data);
            XmlSerializer XML = new XmlSerializer(typeof(Produto));
            return (Produto)XML.Deserialize(ms);
        }
    }
}
