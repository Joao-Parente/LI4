using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppCliente
{
    public class CarrinhoComprasViewModel : INotifyPropertyChanged
    {
        private ProdutoCell p;

        public int ID
        {
            get
            {
                return p.id;
            }
        }

        public string Nome
        {
            get
            {
                return p.Nome;
            }
            set
            {
                p.Nome = value;
                NotifyPropertyChanged("Nome");
            }
        }
        public string Preco
        {
            get
            {
                return p.Preco;
            }
            set
            {
                p.Preco = value;
                NotifyPropertyChanged("Preco");
            }
        }
        public int Quantidades
        {
            get
            {
                return p.Quantidades;
            }
            set
            {
                p.Quantidades = value;
                NotifyPropertyChanged("Quantidades");
            }
        }

        public CarrinhoComprasViewModel(ProdutoCell p)
        {
            this.p = p;
        }

        public ProdutoCell GetProdutoCell()
        {
            return this.p;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
