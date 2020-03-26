using System;

namespace ServerMyBar.comum
{
    public class Reclamacao
    {
        public int idPedido{ get; set; }
        public string motivo{ get; set; }
        public string assunto{ get; set; }
        public DateTime datahora { get; set; }

        public Reclamacao(int id,string m,string a,DateTime d)
        {
            this.idPedido = id;
            this.motivo = m;
            this.assunto = a;
            this.datahora = d;
        }
    }
}