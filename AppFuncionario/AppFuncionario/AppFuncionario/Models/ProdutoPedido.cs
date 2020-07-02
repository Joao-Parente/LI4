namespace AppFuncionario
{
    public class ProdutoPedido
    {
        public Produto p { get; set; }
        public int quantidades { get; set; }

        public ProdutoPedido(Produto pr, int qua)
        {
            p = pr;
            quantidades = qua;
        }

    }
}
