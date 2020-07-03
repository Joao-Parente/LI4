using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace AppFuncionario
{
    public class Pedido
    {
        public int id { get; set; }
        public string idCliente { get; set; }
        public string idEmpregado { get; set; }
        public string detalhes { get; set; }
        public DateTime data_hora { get; set; }
        [DataMember]
        public List<ProdutoPedido> produtos { get; set; }
        public string idCliente_notf { get; set; }


        public Pedido(int id, string idCliente, string idEmpregado, string detalhes, DateTime dataHora, List<ProdutoPedido> produto,string idplayer)
        {
            this.id = id;
            this.idCliente = idCliente;
            this.idEmpregado = idEmpregado;
            this.detalhes = detalhes;
            this.data_hora = dataHora;
            this.produtos = produto;
            this.idCliente_notf = idplayer;
        }

    }
}