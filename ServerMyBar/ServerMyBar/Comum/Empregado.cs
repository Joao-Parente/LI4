namespace ServerMyBar.comum
{
    public class Empregado
    {
        public string nome { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool egestor { get; set; }


        public Empregado()
        {
            nome = "";
            email = "";
            password = "";
            egestor = false;
        }

        public Empregado(string n, string e, string p, bool eg)
        {
            this.nome = n;
            this.email = e;
            this.password = p;
            this.egestor = eg;
        }
    }
}