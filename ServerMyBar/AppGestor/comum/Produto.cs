using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace AppGestor
{
    public class Produto
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public string nome { get; set; }
        public string detalhes { get; set; }
        public int disponibilidade { get; set; }
        public float preco { get; set; }
        public string imagem { get; set; }


        public Produto()
        {
            id = 0;
            tipo = "";
            nome = "";
            detalhes = "";
            disponibilidade = 0;
            preco = 0;
            imagem = "";
        }

        public Produto(int i, string t, string n, string de, int di, float p)
        {
            id = i;
            tipo = t;
            nome = n;
            detalhes = de;
            disponibilidade = di;
            preco = p;
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

        public override string ToString()
        {
            return ("Id: " + id +
                        "\n Tipo: " + tipo +
                        "\n Nome: " + nome +
                        "\n Detalhes: " + detalhes +
                        "\n Disponibilidade: " + disponibilidade +
                        "\n Preço: " + preco +
                        "\n Imagem: " + imagem);
        }
    }
}
