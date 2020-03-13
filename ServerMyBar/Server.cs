using System.Threading;
using ServerMyBar.comum;
using ServerMyBar.serverCliente;
using ServerMyBar.serverFunc;
using ServerMyBar.serverGestor;

namespace ServerMyBar
{
    class Server
    {

        static void Main(string[] args)
        {

           Gestor gestor = new Gestor();
           StarterClient st= new StarterClient(gestor);

           ServerFunc func = new ServerFunc(gestor,st);
           Thread tf = new Thread(func.run); 
          
            
            
           ServerGestor sges= new ServerGestor(gestor,st);
           Thread tg = new Thread(sges.run);

           tf.Start();
           tg.Start();

           tf.Join();
           tg.Join();

           // /testing
           // ServerClient c= new ServerClient(gestor);
           //c.run();

           //Cliente x=ClienteDAO.getInfoCliente("augusto","manuel");
           //ClienteDAO.registaCliente("goncalo", "ez","arroz");

        }
    }
}