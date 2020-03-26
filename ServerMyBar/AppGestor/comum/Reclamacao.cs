using System;

namespace AppGestor
{
    public class Reclamacao
    {
        public int idPedido { get; set; }
        public string motivo { get; set; }
        public string assunto { get; set; }
        public DateTime data_hora { get; set; }

        public Reclamacao()
        {
            idPedido = 0;
            motivo = "";
            assunto = "";
        }
    }
}