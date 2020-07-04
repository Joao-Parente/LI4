using System;
using System.Threading;
using ServerMyBar.comum;

namespace ServerMyBar.serverCliente
{
    public class StarterClient
    {
        private Gestor gestor;
        private ServerClient sv;
        private Thread t;
        public bool estado { get; set; }

        public StarterClient(Gestor g)
        {
            gestor = g;
            estado = false;
            t = null;
        }

        public void offCliente()
        {
            if (estado)
            {
                Console.WriteLine("Turning off serverClient");
                t.Interrupt();
                sv.off();
                estado = false;
            }
        }

        public void onCliente()
        {
            if (estado != true)
            {
                Console.WriteLine("Turning on serverClient");
                sv = new ServerClient(gestor);
                t = new Thread(sv.run);
                t.Start();
                estado = true;
            }
        }
    }
}