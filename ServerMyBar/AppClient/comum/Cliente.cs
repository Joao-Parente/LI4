namespace AppClient
{
    public class Cliente
    {
        public string email { get; set; }
        public string password { get; set; }
        public string nome { get; set; }


        public Cliente()
        {
            email = "";
            password = "";
            nome = "";
        }


        public Cliente(string e, string p, string n)
        {
            email = e;
            password = p;
            nome = n;
        }
    }
}