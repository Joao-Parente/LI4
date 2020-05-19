using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AppFuncionario
{
    public class PedidoInfo : INotifyPropertyChanged
    {
        private int _idPedido;
        private string _produtos;
        private string _detalhes;
        private string _estado;
        public int idPedido 
        {
            get
            {
                return _idPedido;
            }
            set
            {
                _idPedido = value;
                NotifyPropertyChanged("Id");
            }
        }
        public string produtos
        {
            get
            {
                return _produtos;
            }
            set
            {
                _produtos = value;
                NotifyPropertyChanged("produtos");
            }
        }
        public string detalhes
        {
            get
            {
                return _detalhes;
            }
            set
            {
                _detalhes = value;
                NotifyPropertyChanged("detalhes");
            }
        }
        public string estado
        {
            get
            {
                return _estado;
            }
            set
            {
                _estado = value;
                NotifyPropertyChanged("estado");
            }
        }

        public PedidoInfo(Pedido p)
        {
            _idPedido = p.id;
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

            _produtos = sb.ToString();
            _detalhes = p.detalhes;
            _estado = "Por Preparar";
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
