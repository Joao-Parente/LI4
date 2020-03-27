namespace ServerMyBar.comum
{
    public class Empregado
    {
        public string email { get; set; }
        public string password { get; set; }
        public string nome { get; set; }
        public bool egestor { get; set; }


        public Empregado()
        {
            email = "";
            password = "";
            nome = "";
            egestor = false;
        }


        public Empregado(string e, string p, string n, bool eg)
        {
            this.email = e;
            this.password = p;
            this.nome = n;
            this.egestor = eg;
        }
    }
}