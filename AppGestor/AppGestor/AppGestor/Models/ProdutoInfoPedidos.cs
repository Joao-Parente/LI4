using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AppGestor
{
    public class ProdutoInfoPedidos 
    {
        private int _id;
        private string _nome;
        private string _preco;
        private int _quantidades;
        private string _quantidade;
        private ImageSource _img;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                _nome = value;
            }
        }

        public string Preco
        {
            get
            {
                return _preco;
            }
            set
            {
                _preco = value;
            }
        }

        public int Quantidades
        {
            get
            {
                return _quantidades;
            }
            set
            {
                _quantidades = value;
            }
        }

        public string Quantidade
        {
            get
            {
                return _quantidade;
            }
            set
            {
                _quantidade = value;
            }
        }

        public ImageSource Imagem
        {
            get
            {
                return _img;
            }
            set
            {
                _img = value;
            }
        }

        public ProdutoInfoPedidos(int id, string n, string p, int q, byte[] img)
        {
            _id = id;
            _nome = n;
            _preco = p;
            _quantidades = q;
            _quantidade = "Q: " + q;
            _img = null;
        }
    }
}
