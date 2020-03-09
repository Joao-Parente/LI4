using ServerMyBar.ServerGestor;

namespace ServerMyBar
{
    class Server
    {

        static void Main(string[] args)
        {

            Gestor gestor = new Gestor();
            StarterClient st= new StarterClient(gestor);
            
            
           ServerFunc func = new ServerFunc(gestor,st);
           func.run();
            
           // /testing
           // ServerClient c= new ServerClient(gestor);
            //c.run();

            //Cliente x=ClienteDAO.getInfoCliente("augusto","manuel");
            //ClienteDAO.registaCliente("goncalo", "ez","arroz");

        }
    }
}
