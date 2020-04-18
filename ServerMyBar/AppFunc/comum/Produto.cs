using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace AppFunc
{
    public class Produto
    {
        private int id { get; set; }
        private string tipo { get; set; }
        private string nome { get; set; }
        private string detalhes { get; set; }
        private int disponibilidade { get; set; }
        private float preco { get; set; }
        private string imagem { get; set; }


        public Produto()
        {
            this.id = 0;
            this.tipo = "";
            this.nome = "";
            this.detalhes = "";
            this.disponibilidade = 0;
            this.preco = 0;
            this.imagem = "";
        }

        public Produto(int i, string t, string n, string de, int di, float p)
        {
            this.id = i;
            this.tipo = t;
            this.nome = n;
            this.detalhes = de;
            this.disponibilidade = di;
            this.preco = p;
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
            return ("Id: " + this.id +
                        "\n Tipo: " + this.tipo +
                        "\n Nome: " + this.nome +
                        "\n Detalhes: " + this.detalhes +
                        "\n Disponibilidade: " + this.disponibilidade +
                        "\n Preco: " + this.preco +
                        "\n Imagem: " + this.imagem);
        }
    }
}
