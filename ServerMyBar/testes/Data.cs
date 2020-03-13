using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace testes
{
    public class Data
    {


        public int Age { get; set; }
        public bool Male { get; set; }
        public string Name { get; set; }
        [DataMember]
        public List<Data2> pois { get; set; }
        
        
        public Data()
        {
            Data2 k= new Data2();
            pois=new List<Data2>();
            
            pois.Add(k);pois.Add(k);pois.Add(k);
        }

        public void Save(string filename)
        {
            
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                XmlSerializer XML = new XmlSerializer(typeof(Data));
                XML.Serialize(stream,this);
            }
        }
        
        public static Data loadFromFile(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                XmlSerializer XML = new XmlSerializer(typeof(Data));
                return (Data) XML.Deserialize(stream);
            }
        }
        
        
    }
}