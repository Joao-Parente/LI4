namespace ServerMyBar.comum
{
    public class Produto
    {
        public int id { get; set; }
        public string nome { get; set; }
        public float preco { get; set; }
        public string tags { get; set; }
        public int disponibilidade { get; set; }
        public string imagem { get; set; }


        public Produto()
        {
            id = 1;
            nome = "Arroz com atum";
            preco = 3.5f;
            tags = "glutenfree vegan";
            imagem = "muitos bytes";
        }


        public Produto(int i, string n, string d, int di, float p)
        {
            id = i;
            nome = n;
            preco = p;
            tags = d;
            disponibilidade = di;
        }


        public override string ToString()
        {
            return ("Id: " + id +
                        "\n Nome: " + nome +
                        "\n Preço: " + preco +
                        "\n Tags: " + tags +
                        "\n Imagem: " + imagem);
        }
    }
}
