using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

                string rBuffer = Encoding.ASCII.GetString(bReceive);

                var iStartPos = rBuffer.IndexOf("HTTP", 1);
                string sHttpVersion = rBuffer.Substring(iStartPos, 8);


                // Extract the Requested Type and Requested file/directory  
                var sRequest = rBuffer.Substring(0, iStartPos - 1);
                var response = "";
                var header = "";
                var encodeheader= Encoding.ASCII.GetBytes(header);
                if (sRequest.StartsWith("GET"))
                {
                    if (sRequest.Contains("index.html"))
                    {
                        response = File.ReadAllText(@"Index\index.html");

                        var length= Encoding.ASCII.GetBytes(response);
                        header = header + sHttpVersion + " 200 OK" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "text/html" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + length.Length + "\r\n\r\n";
                        encodeheader = Encoding.ASCII.GetBytes(header); 
                        Console.WriteLine(header);

                        var sendBuffer = Encoding.ASCII.GetBytes(response);
                        s.Send(encodeheader);
                        s.Send(sendBuffer);
                    }
                    else if (sRequest.Contains("4k.JPG"))
                    {
                        var responseImage = File.ReadAllBytes(@"Index\4k.JPG");
                       
                        var length = Encoding.ASCII.GetBytes(response);
                        header = header + sHttpVersion + " 200 OK" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "image/jpeg" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + responseImage.Length + "\r\n\r\n";
                        encodeheader = Encoding.ASCII.GetBytes(header);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }
                    else if (sRequest.Contains("favicon.ico"))
                    {
                        var responseImage = File.ReadAllBytes(@"Index\favicon.ico");

                        header = header + sHttpVersion + " 200 OK" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "image/x-icon" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + responseImage.Length + "\r\n\r\n";
                        encodeheader = Encoding.ASCII.GetBytes(header);
                        Console.WriteLine(header);

                        var sendBuffer = Encoding.ASCII.GetBytes(response);
                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }
                    else if (sRequest.Contains("style.css"))
                    {
                       
                        response = File.ReadAllText(@"Index\style.css");

                        var sendCss = Encoding.ASCII.GetBytes(response);
                        
                        var length = Encoding.ASCII.GetBytes(response);
                        header = header + sHttpVersion + " 200 OK" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "text/css" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + response.Length + "\r\n\r\n";
                        encodeheader = Encoding.ASCII.GetBytes(header);
                        Console.WriteLine(header);

                        var sendBuffer = Encoding.ASCII.GetBytes(response);
                        s.Send(encodeheader);
                        s.Send(sendCss);
                    }
                    else if (sRequest.Contains("Sang.jpg"))
                    {
                        var responseImage = File.ReadAllBytes(@"info_files\Sang.JPG");

                        var length = Encoding.ASCII.GetBytes(response);
                        header = header + sHttpVersion + " 200 OK" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "image/jpeg" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + responseImage.Length + "\r\n\r\n";
                        encodeheader = Encoding.ASCII.GetBytes(header);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }
                    else if (sRequest.Contains("VuQuangHop.jpg"))
                    {
                        var responseImage = File.ReadAllBytes(@"info_files\VuQuangHop.JPG");

                        var length = Encoding.ASCII.GetBytes(response);
                        header = header + sHttpVersion + " 200 OK" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "image/jpeg" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + responseImage.Length + "\r\n\r\n";
                        encodeheader = Encoding.ASCII.GetBytes(header);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }
                     else if (sRequest.Contains("background.jpg"))
                    {
                        var responseImage = File.ReadAllBytes(@"info_files\background.JPG");
                       
                        var length = Encoding.ASCII.GetBytes(response);
                        header = header + sHttpVersion + " 200 OK" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "image/jpeg" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + responseImage.Length + "\r\n\r\n";
                        encodeheader = Encoding.ASCII.GetBytes(header);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }


                }
                else if (sRequest.StartsWith("POST"))
                {
                    var admin = "Username=admin&Password=admin";
                    if(rBuffer.Contains(admin))
                    {
                        response = File.ReadAllText(@"info_files\info.html");

                        var length = Encoding.ASCII.GetBytes(response);
                        header = header + sHttpVersion + " 301 Moved Permanently" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "text/html" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + length.Length + "\r\n\r\n";
                        encodeheader = Encoding.UTF8.GetBytes(header);
                        Console.WriteLine(header);

                        var sendBuffer = Encoding.UTF8.GetBytes(response);
                        s.Send(encodeheader);
                        s.Send(sendBuffer);
                    }
                    else
                    {
                        response = File.ReadAllText(@"404\404.html");

                        var length = Encoding.ASCII.GetBytes(response);
                        header = header + sHttpVersion + " 404 Not Found" + "\r\n";
                        header = header + "Server: cx1193719-b\r\n";
                        header = header + "Content-Type: " + "text/html" + "\r\n";
                        header = header + "Accept-Ranges: bytes\r\n";
                        header = header + "Content-Length: " + length.Length + "\r\n\r\n";
                        encodeheader = Encoding.ASCII.GetBytes(header);
                        Console.WriteLine(header);

                        var sendBuffer = Encoding.ASCII.GetBytes(response);
                        s.Send(encodeheader);
                        s.Send(sendBuffer);
                    }
                    
                }
                Console.WriteLine(rBuffer);
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
