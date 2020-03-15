using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace ServerMyBar.comum
{
        public class Pedido
        {
            public int id { get; set; }
            public int idCliente { get; set; }
            public string detalhes { get; set; }
            public int avaliacao { get; set; }
            public DateTime data_hora { get; set; }
            [DataMember]
            public List<Produto> produtos { get; set; }


            public Pedido(int id, int idCliente, string detalhes, int avaliacao, DateTime dataHora,List<Produto> produto)
            {
                this.id = id;
                this.idCliente = idCliente;
                this.detalhes = detalhes;
                this.avaliacao = avaliacao;
                this.data_hora = dataHora;
                this.produtos = produto;
            }


            public Pedido()
            {
                id = 2;
                idCliente = 100;
                detalhes = "arroz man";
                avaliacao = 51;
                data_hora = new DateTime();
                produtos=new List<Produto>();
                produtos.Add(new Produto());
                produtos.Add(new Produto());
                produtos.Add(new Produto());
                produtos.Add(new Produto());
            }


            public void adicionarProduto(String produto)
            {
                Produto p = new Produto(4, produto, "tags", 1, 3); //para teste
                produtos.Add(p);
                
            }


            public void removerProduto(String produto)
            {
                Produto p = new Produto(4, produto, "tags", 1, 3); //para teste
                produtos.Remove(p);
            }   


            public void imprimePedido()
            {
                Console.Write(this.ToString());
                int i;
                for (i = 0; i < produtos.Count; i++)
                {
                    Console.WriteLine("          i: "+produtos[i].ToString());
                }

            }


            public string ToString()
            {
                return (" Pedido: " +id+
                       "\n     idCliente: "+idCliente+
                       "\n     detalhes: "+detalhes+" " +
                       "\n     Avaliação: "+avaliacao + "\n" +
                       "\n     Produtos::  #" +produtos.Count 
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
                return (Pedido) XML.Deserialize(ms);
            }
        }
    }
