using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ServerMyBar.comum
{
    public class Produto
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public string nome { get; set; }
        public string detalhes { get; set; }
        public int disponibilidade { get; set; }
        public float preco { get; set; }
        public byte[] imagem { get; set; }

        public Produto(int i, string t, string n, string de, int di, float p,byte[] img)
        {
            id = i;
            tipo = t;
            nome = n;
            detalhes = de;
            disponibilidade = di;
            preco = p;
            imagem = img;
        }

    }
}
