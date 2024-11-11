using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ChatHost
{
    internal class Program
    {
        //Host implementation
        //need to connect System.ServiceModel
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(wcf_chat.ServiceChat)))
            {
                //open host
                host.Open();
                Console.WriteLine("Host started.");
                //as it's a console app we call Console.ReadLine() method to prevent console from closing
                Console.ReadLine();
            }
        }
    }
}
