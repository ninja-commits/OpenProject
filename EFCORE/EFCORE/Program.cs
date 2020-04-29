using System;
using System.Linq;
using EFCORE.Models;
using Newtonsoft.Json;

namespace EFCORE
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ADONETContext())
            {
                foreach (var it in db.Customer.ToList())
                {
                    Console.WriteLine(JsonConvert.SerializeObject(it));
                }
            }
                Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
