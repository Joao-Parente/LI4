namespace AppCliente
{
    public class Cliente
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nome { get; set; }


        public Cliente()
        {
            this.Email = "joao";
            this.Password = "parente";
            this.Nome = "tardnation";
        }

        public Cliente(string e, string p, string n)
        {
            this.Email = e;
            this.Password = p;
            this.Nome = n;
        }
    }
}