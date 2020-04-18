﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppCliente
{
    public class ProdutoInfo : INotifyPropertyChanged
    {
        private int _id;
        private string _nome;
        private string _preco;
        private int _quantidades;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged("Id");
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
                NotifyPropertyChanged("Nome");
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
                NotifyPropertyChanged("Preco");
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
                NotifyPropertyChanged("Quantidades");
            }
        }

        public ProdutoInfo(int id,string n,string p,int q)
        {
            _id = id;
            _nome = n;
            _preco = p;
            _quantidades = q;
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