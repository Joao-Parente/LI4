namespace ServerMyBar.comum
{
    public class Empregado
    {
        public int id { get; set; }
        public string nome{ get; set; }
        public string email{ get; set; }
        public string password{ get; set; }
        
        public bool egestor { get; set; }

        public Empregado()
        {
        }

        public Empregado(int id, string nome, string email, string password,bool eg)
        {
            this.id = id;
            this.nome = nome;
            this.email = email;
            this.password = password;
            this.egestor = eg;
        }
    }
}