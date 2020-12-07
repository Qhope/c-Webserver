using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace webServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Loopback, 80));
            server.Listen(10);
            while (true)
            {
                var s = server.Accept();
                var bReceive = new byte[1024];
                var sss = s.Receive(bReceive);
                string sBuffer = Encoding.ASCII.GetString(bReceive);


                var iStartPos = sBuffer.IndexOf("HTTP", 1);
                string sHttpVersion = sBuffer.Substring(iStartPos, 8);


                // Extract the Requested Type and Requested file/directory  
                var sRequest = sBuffer.Substring(0, iStartPos - 1);
                var response = "";

  
                    if (sRequest.StartsWith("GET"))
                    {
                        if (sRequest.Contains("index.html"))
                        {
                            response = File.ReadAllText("index.html");
                            
                        }
                        else if (sRequest.Contains("4k.JPG"))
                        {
                            var reponse = File.ReadAllBytes("4k.JPG");
                        s.Send(reponse, reponse.Length, SocketFlags.None);
                        var statusCode = sHttpVersion + "301" + "\r\n";
                        var status = Encoding.ASCII.GetBytes(statusCode);
                        s.Send(status);
                    }
                        
                    }
                    else if (sRequest.StartsWith("POST"))
                    {
                        var admin = "admin";
                        if (admin == "admin")
                        {
                            response = sHttpVersion + " 301 Moved Permanently \r\n l ocation: /info.html";
                        }
                    }
                Console.WriteLine(sBuffer);
                //Console.WriteLine("sHttpVersion "  +sHttpVersion);
                //Console.WriteLine("sRequest " + sRequest);
                //var response = "400 hop";
                ////response = sHttpVersion + " 404 Not Found" + "\r\n";
                //response = sHttpVersion + " 200" + "\r\n";
                var sendBuffer = Encoding.ASCII.GetBytes(response);
                s.Send(sendBuffer);
                s.Close();
            }
        }

        private string IndexPage(string url) {
            if (url == "index.html")

                return File.ReadAllText("index.html");
            return "";
        }
    }
}
