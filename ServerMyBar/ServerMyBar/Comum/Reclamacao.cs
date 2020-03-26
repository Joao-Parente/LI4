using System;

namespace ServerMyBar.comum
{
    public class Reclamacao
    {
        public int idPedido { get; set; }
        public string motivo { get; set; }
        public string assunto { get; set; }
        public DateTime data_hora { get; set; }

        public Reclamacao()
        {
            this.idPedido = 0;
            this.motivo = "";
            this.assunto = "";
        }

        public Reclamacao(int id, string m, string a, DateTime d)
        {
            this.idPedido = id;
            this.motivo = m;
            this.assunto = a;
            this.data_hora = d;
        }
    }
}