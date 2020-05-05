using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace AppClient
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


        public Pedido(int id, string idCliente, string idEmpregado, string detalhes, DateTime dataHora, List<ProdutoPedido> produto)
        {
            this.id = id;
            this.idCliente = idCliente;
            this.idEmpregado = idEmpregado;
            this.detalhes = detalhes;
            this.data_hora = dataHora;
            this.produtos = produto;
        }


        public Pedido()
        {
            id = 0;
            idCliente = "" + 0;
            idEmpregado = "null";
            detalhes = "";
            data_hora = new DateTime();
            produtos = new List<ProdutoPedido>();
        }


        public void adicionarProduto(String produto)
        {
            Produto p = new Produto(4, "tipo", produto, "tags", 1, 3,null); //para teste
            produtos.Add(new ProdutoPedido(p, 1));
        }

        public void addProduto(Produto p)
        {
            this.produtos.Add(new ProdutoPedido(p, 1));
        }

        public void removerProduto(String produto)
        {
            Produto p = new Produto(4, "tipo", produto, "tags", 1, 3,null); //para teste
            //produtos.Remove(p);
        }


        public void imprimePedido()
        {
            Console.Write(this.ToString());
            int i;
            for (i = 0; i < produtos.Count; i++)
            {
                Console.WriteLine("i: " + produtos[i].ToString());
            }

        }


        public string toString()
        {
            return (" Pedido: " + id +
                   "\n     idCliente: " + idCliente +
                   "\n     detalhes: " + detalhes + " " +
                   "\n     Produtos::  #" + produtos.Count
                );
        }


        public byte[] SavetoBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            XmlSerializer XML = new XmlSerializer(typeof(Pedido));
            XML.Serialize(ms, this);
            ms.Close();
            return ms.ToArray();

        }


        public static Pedido loadFromBytes(byte[] data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(data);
            XmlSerializer XML = new XmlSerializer(typeof(Pedido));
            return (Pedido)XML.Deserialize(ms);
        }
    }
}