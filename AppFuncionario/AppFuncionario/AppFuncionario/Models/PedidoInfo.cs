using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppFuncionario
{
    public class PedidoInfo
    {
        public int idPedido { get; set; }
        public string produtos { get; set; }
        public string detalhes { get; set; }
        public string estado { get; set; }

        public PedidoInfo(Pedido p)
        {
            idPedido = p.id;
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < p.produtos.Count; i++)
            {
                if (i + 1 != p.produtos.Count)
                {
                    sb.Append(p.produtos[i].quantidades).Append("x").Append(p.produtos[i].p.nome).Append(", ");
                }
                else
                {
                    sb.Append(p.produtos[i].quantidades).Append("x").Append(p.produtos[i].p.nome).Append(".");
                }
            }

            produtos = sb.ToString();
            detalhes = p.detalhes;
            estado = "Por Preparar";
        }
      
    }
}
