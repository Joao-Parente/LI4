using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace AppCliente
{
    public class Pedido
    {
        private int Id { get; set; }
        private int IdCliente { get; set; }
        private string Detalhes { get; set; }
        private int Avaliacao { get; set; }
        private DateTime DataHora { get; set; }
        [DataMember]
        private List<Produto> Produtos { get; set; }

        public Pedido()
        {
            this.Id = 0;
            this.IdCliente = 0;
            this.Detalhes = "tard";
            this.Avaliacao = 0;
            this.DataHora = DateTime.Now;
            this.Produtos = new List<Produto>();
            this.Produtos.Add(new Produto());
            this.Produtos.Add(new Produto());
            this.Produtos.Add(new Produto());
        }

        public Pedido(int id,int idcliente,string detalhes,int aval,DateTime d,List<Produto> p)
        {
            this.Id = id;
            this.IdCliente = idcliente;
            this.Detalhes = detalhes;
            this.Avaliacao = aval;
            this.DataHora = d;
            this.Produtos = p;
        }

        public void imprimePedido()
        {
            Console.Write(this.ToString());
            int i;
            for (i = 0; i < Produtos.Count; i++)
            {
                Console.WriteLine("          i: " + Produtos[i].ToString());
            }

        }

        public string toString()
        {
            return (" Pedido: " + Id +
                   "\n     idCliente: " + IdCliente +
                   "\n     detalhes: " + Detalhes + " " +
                   "\n     Avaliação: " + Avaliacao + "\n" +
                   "\n     Produtos::  #" + Produtos.Count
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
