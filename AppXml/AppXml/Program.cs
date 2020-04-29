using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AppXml
{
    class Program
    {
        static void Writefile(string str)
        {
            var fileName = @"C:\Users\BRIAN\Documents\result.txt";
            File.AppendAllText(fileName, str);
        }

        static void Main(string[] args)
        {
            var fileName = @"C:\Users\BRIAN\Documents\bruh.xml";
            XDocument xml = XDocument.Load(fileName);

            var soapResponse = xml.Descendants().Where(x => x.Name.LocalName == "return").Select(x => new SoapResponse()
            {
                numErreur = (int)x.Element(x.Name.Namespace + "numErreur"),
                listNumFic = (int)x.Element(x.Name.Namespace + "listNumFic"),
                msgErreur = (string)x.Element(x.Name.Namespace + "msgErreur")
            }).FirstOrDefault();

            Console.WriteLine("Hello World!");
        }
    }
    public class SoapResponse
    {
        public int numErreur { get; set; }
        public int listNumFic { get; set; }
        public string msgErreur { get; set; }

    }
}
