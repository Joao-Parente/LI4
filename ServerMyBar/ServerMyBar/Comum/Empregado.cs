using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ServerMyBar.comum
{
    public class Empregado
    {
        public string email { get; set; }
        public string password { get; set; }
        public string nome { get; set; }
        public bool egestor { get; set; }


        public Empregado()
        {
            email = "";
            password = "";
            nome = "";
            egestor = false;
        }


        public Empregado(string e, string p, string n, bool eg)
        {
            this.email = e;
            this.password = p;
            this.nome = n;
            this.egestor = eg;
        }

        public byte[] SavetoBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            XmlSerializer XML = new XmlSerializer(typeof(Empregado));
            XML.Serialize(ms, this);
            ms.Close();
            return ms.ToArray();
        }

        public static Empregado loadFromBytes(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data);
            XmlSerializer XML = new XmlSerializer(typeof(Empregado));
            return (Empregado)XML.Deserialize(ms);
        }
    }
}