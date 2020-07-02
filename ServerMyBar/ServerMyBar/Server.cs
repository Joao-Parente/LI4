using System.Threading;
using ServerMyBar.comum;
using ServerMyBar.serverCliente;
using ServerMyBar.serverFunc;

namespace ServerMyBar
{
    class Server
    {
        static void Main(string[] args)
        {
            Gestor gestor = new Gestor();

            ServerClient x = new ServerClient(gestor);
            StarterClient st = new StarterClient(gestor);

            ServerFunc func = new ServerFunc(gestor, st);
            Thread tf = new Thread(func.run);
            tf.Start();
        }
    }
}
