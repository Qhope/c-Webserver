using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace webServer
{
    class Header
    {
        string httpversion;
        string statusCode;
        string serverName;
        string contentType;
        string contentLength;
        string encoding;
        public string Httpversion{
            get{ return httpversion; }
            set { httpversion = value; }
            }
        public string Statuscode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }
        public string Servername
        {
            get { return serverName; }
            set { serverName = value; }
        }
        public string Contenttype
        {
            get { return contentType; }
            set { contentType = value; }
        }
        public string Contentlength
        {
            get { return contentLength; }
            set { contentLength = value; }
        }
        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }
        public string responseHeader()
        {
            string response="";
            response = httpversion + " "+statusCode+ "\r\n";
            response+=  "Server: "+serverName+"\r\n";
            response+= "Content-Type: " + contentType + "\r\n";
            response+= "Accept-Ranges: bytes\r\n";
            response+="Content-Length: " + contentLength + "\r\n\r\n";
            return response;
        }
        public string chunkResponseHeader()
        {
            string response = "";
            response = httpversion + " " + statusCode + "\r\n";
            response += "Server: " + serverName + "\r\n";
            response += "Content-Type: " + contentType + "\r\n";
            response += "Accept-Ranges: bytes\r\n";
            response += "Transfer-Encoding: chunked" + "\r\n\r\n";
            return response;
        }
    }
    class Program
    {
        static void sendChunk(Socket s, string path, int speed)
        {
            string finalPoint;
            byte[] encode;
            int chunkSize = speed;
            string source = path;

            BinaryReader rdr = new BinaryReader(new FileStream(source, FileMode.Open));
            int streamLength = (int)rdr.BaseStream.Length;

            int size;
            while (rdr.BaseStream.Position < streamLength)
            {
                byte[] b = new byte[chunkSize];

                long remaining = rdr.BaseStream.Length - rdr.BaseStream.Position;
                if (remaining >= chunkSize)
                {
                    rdr.Read(b, 0, chunkSize);
                    size = chunkSize;
                }
                else
                {
                    byte[] c = new byte[(int)remaining];
                    rdr.Read(c, 0, (int)remaining);
                    size = (int)remaining;
                    string ee = size.ToString("X") + "\r\n";
                    s.Send(Encoding.ASCII.GetBytes(ee));
                    s.Send(c);
                    s.Send(Encoding.ASCII.GetBytes("\r\n"));
                    break;
                }
                string sx = size.ToString("X") + "\r\n";
                s.Send(Encoding.ASCII.GetBytes(sx));
                s.Send(b);
                s.Send(Encoding.ASCII.GetBytes("\r\n"));
            }

            finalPoint = "0\r\n" + "\r\n";
            encode = Encoding.ASCII.GetBytes(finalPoint);
            rdr.Close();
            s.Send(encode);
        }
        static void serveForever(Socket server)
        {
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
                byte[] encodeheader;
                if (sRequest.StartsWith("GET"))
                {
                    if (sRequest.Contains("index.html"))
                    {
                        response = File.ReadAllText(@"Index\index.html");
                        var length = Encoding.ASCII.GetBytes(response);

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "text/html";
                        h.Contentlength = (length.Length).ToString();
                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        Console.WriteLine(header);

                        var sendBuffer = Encoding.ASCII.GetBytes(response);
                        s.Send(encodeheader);
                        s.Send(sendBuffer);
                    }
                    else if (sRequest.Contains("file.html"))
                    {
                        response = File.ReadAllText(@"Index\file.html");

                        var length = Encoding.ASCII.GetBytes(response);
                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "text/html";
                        h.Contentlength = (length.Length).ToString();

                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        Console.WriteLine(h);

                        var sendBuffer = Encoding.ASCII.GetBytes(response);
                        s.Send(encodeheader);
                        s.Send(sendBuffer);
                    }
                    else if (sRequest.Contains("4k.JPG"))
                    {
                        var responseImage = File.ReadAllBytes(@"Index\4k.JPG");

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        h.Contentlength = (responseImage.Length).ToString();
                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }
                    else if (sRequest.Contains("favicon.ico"))
                    {
                        var responseImage = File.ReadAllBytes(@"Index\favicon.ico");

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/x-icon";
                        h.Contentlength = (responseImage.Length).ToString();
                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
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

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "text/css";
                        h.Contentlength = (length.Length).ToString();
                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(sendCss);
                    }
                    else if (sRequest.Contains("Sang.jpg"))
                    {
                        var responseImage = File.ReadAllBytes(@"info_files\Sang.JPG");

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        h.Contentlength = (responseImage.Length).ToString();
                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }
                    else if (sRequest.Contains("VuQuangHop.jpg"))
                    {
                        var responseImage = File.ReadAllBytes(@"info_files\VuQuangHop.JPG");

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        h.Contentlength = (responseImage.Length).ToString();
                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }
                    else if (sRequest.Contains("background.jpg"))
                    {
                        var responseImage = File.ReadAllBytes(@"info_files\background.JPG");

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        h.Contentlength = (responseImage.Length).ToString();
                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        Console.WriteLine(header);

                        s.Send(encodeheader);
                        s.Send(responseImage, responseImage.Length, SocketFlags.None);
                    }
                    else if (sRequest.Contains("mountain.jpg"))
                    {

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        var RHeader = "";
                        RHeader = h.chunkResponseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        s.Send(encodeheader);


                        var path = @"download\mountain.jpg";
                        sendChunk(s, path, 102400);

                    }
                    else if (sRequest.Contains("lion.jpg"))
                    {

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        var RHeader = "";
                        RHeader = h.chunkResponseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        s.Send(encodeheader);

                        var path = @"download\lion.jpg";
                        sendChunk(s, path, 102400);

                    }
                    else if (sRequest.Contains("bay.jpg"))
                    {

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        var RHeader = "";
                        RHeader = h.chunkResponseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        s.Send(encodeheader);

                        var path = @"download\bay.jpg";
                        sendChunk(s, path, 102400);

                    }
                    else if (sRequest.Contains("nightcity.jpg"))
                    {

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        var RHeader = "";
                        RHeader = h.chunkResponseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        s.Send(encodeheader);

                        var path = @"download\nightcity.jpg";
                        sendChunk(s, path, 102400);

                    }
                    else if (sRequest.Contains("car.jpg"))
                    {

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 200 OK";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "image/jpeg";
                        var RHeader = "";
                        RHeader = h.chunkResponseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        s.Send(encodeheader);

                        var path = @"download\car.jpg";
                        sendChunk(s, path, 102400);
                    }
                }
                else if (sRequest.StartsWith("POST"))
                {
                    var admin = "Username=admin&Password=admin";
                    if (rBuffer.Contains(admin))
                    {
                        response = File.ReadAllText(@"info_files\info.html");

                        var length = Encoding.ASCII.GetBytes(response);

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 301 Moved Permanentl";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "text/html";
                        h.Contentlength = (length.Length).ToString();

                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
                        Console.WriteLine(header);

                        var sendBuffer = Encoding.UTF8.GetBytes(response);
                        s.Send(encodeheader);
                        s.Send(sendBuffer);
                    }
                    else
                    {
                        response = File.ReadAllText(@"404\404.html");

                        var length = Encoding.ASCII.GetBytes(response);

                        Header h = new Header();
                        h.Httpversion = sHttpVersion;
                        h.Statuscode = " 404 Not Found";
                        h.Servername = "QhopeAndSang";
                        h.Contenttype = "text/html";
                        h.Contentlength = (length.Length).ToString();

                        var RHeader = "";
                        RHeader = h.responseHeader();
                        encodeheader = Encoding.ASCII.GetBytes(RHeader);
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
        static void Main(string[] args)
        {
            var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serveForever(server);


        }
    }
}
