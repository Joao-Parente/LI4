namespace ServerMyBar.comum
{
    public class Reclamacao
    {
        public int idPedido{ get; set; }
        public string motivo{ get; set; }
        public string assunto{ get; set; }

        public Reclamacao(int id,string m,string a)
        {
            this.idPedido = id;
            this.motivo = m;
            this.assunto = a;
        }
    }
}