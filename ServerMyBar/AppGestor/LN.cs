using System.Collections.Generic;

namespace AppGestor
{
    public class LN
    {
        private Dictionary<int, Empregado> empregados;
        private Dictionary<int, Reclamacao> reclamacoes;
        private List<Pedido> preparados;
        private List<Pedido> em_preparacao;
        private List<Pedido> por_preparar;


        //+visualizarPedido(idPedido : int) : Pedido

        //+alternarEstadoSistema() : void

        //+notificarClientes(idCliente : int, mensagem : string) : void

        //+mudarEstadoPedido(idPedido : int) : void

        //+adicionarProduto(produto : Produto) : int

        //+editarProduto(idProduto : int, novoProduto : Produto) : void

        //+consultasEstatisticas() : lista string

        //+visualizacaoFeedback() : lista string

        //+alterarInfoEmpresa(novaInfo : lista string) : void

        //+adicionarEmpregado(idEmpregado : int) : void

        //+removerEmpregado(idEmpregado : int) : void

        //+IniciarSessao(email : string, password : string) : void

        //+TerminarSessao()
    }

}