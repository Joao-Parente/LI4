using System;
using ServerMyBar.comum;


namespace ServerMyBar.serverCliente
{
    public class StarterClient
    {
        private Gestor gestor;
        private ServerClient sv;
        public bool estado { get; set; } // on | off


        public StarterClient(Gestor g)
        {
            gestor = g;
            estado = false;
        }
        
        
        public void offCliente()
        {
            if (estado)
            {
                Console.WriteLine("Turning off serverClient");
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
                sv.run();
                estado = true;
            }
        }
            
            
         public void notificarCliente(  int idCliente, string msg  )
         {
                
         }
    }
}