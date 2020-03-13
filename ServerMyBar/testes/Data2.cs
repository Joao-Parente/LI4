using System;
using System.Runtime.Serialization;

namespace testes
{
    //[DataContract]
    public class Data2
    {
        //[DataMember] 
        public int arroz { get; set; }

        public Data2()
        {
            arroz = 3;
        }
    }
}