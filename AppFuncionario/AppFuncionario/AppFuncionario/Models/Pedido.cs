using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppFuncionario
{
    public class Pedido : ViewCell
    {
        public int IdPedido { get; set; }
        public string EmailCliente { get; set; }
        public string Detalhes { get; set; }
        public DateTime DataHora { get; set; }
        public List<Produto> Produtos { get; set; }
        public string PreviewProdutos { get; set; }
        public string EstadoPreparacao { get; set; }

        public Pedido(int idP,string eC,string d,DateTime dH,List<Produto> ps)
        {
            this.IdPedido = idP;
            this.EmailCliente = eC;
            this.Detalhes = d;
            this.DataHora = dH;
            this.Produtos = ps;
            StringBuilder sb = new StringBuilder();
            int total_caracteres = 0;
            for(int i = 0; i < Produtos.Count; i++)
            {
                Produto p = Produtos[i];
                if (p.Nome.Length + total_caracteres < 60)
                {
                    total_caracteres += p.Nome.Length;
                    sb.Append(p.Nome).Append(", ");
                }
                else
                {
                    sb.Append("...");
                    break;
                }               
            }
            this.PreviewProdutos = sb.ToString();
            this.EstadoPreparacao = "Por Preparar";
        }


    }

}
