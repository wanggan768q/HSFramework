using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HS.Net.HTTP
{


    public class Request
    {
        public bool acceptGzip;
        public byte[] bytes;
        public static byte[] EOL = new byte[] { 13, 10 };
        private static Dictionary<string, string> etags = new Dictionary<string, string>();
        private Dictionary<string, List<string>> headers;
        public bool isDone;
        public int maximumRetryCount;
        public string method;
        public string protocol;
        public Response response;
        public Uri uri;
        public bool useCache;
        public delegate void Call(Request req,Response res);
        public event Call call = null;

        public Request(string method, string uri,Call c)
        {
            this.method = "GET";
            this.protocol = "HTTP/1.1";
            this.response = null;
            this.isDone = false;
            this.maximumRetryCount = 8;
            this.acceptGzip = true;
            this.useCache = false;
            this.headers = new Dictionary<string, List<string>>();
            this.method = method;
            this.uri = new Uri(uri);
            this.call = c;
        }

        public Request(string method, string uri)
        {
            this.method = "GET";
            this.protocol = "HTTP/1.1";
            this.response = null;
            this.isDone = false;
            this.maximumRetryCount = 8;
            this.acceptGzip = true;
            this.useCache = false;
            this.headers = new Dictionary<string, List<string>>();
            this.method = method;
            this.uri = new Uri(uri);
        }

        public Request(string method, string uri, bool useCache)
        {
            this.method = "GET";
            this.protocol = "HTTP/1.1";
            this.response = null;
            this.isDone = false;
            this.maximumRetryCount = 8;
            this.acceptGzip = true;
            this.useCache = false;
            this.headers = new Dictionary<string, List<string>>();
            this.method = method;
            this.uri = new Uri(uri);
            this.useCache = useCache;
        }
        public Request(string method, string uri, byte[] bytes, Call c)
        {
            this.method = "GET";
            this.protocol = "HTTP/1.1";
            this.response = null;
            this.isDone = false;
            this.maximumRetryCount = 8;
            this.acceptGzip = true;
            this.useCache = false;
            this.headers = new Dictionary<string, List<string>>();
            this.method = method;
            this.uri = new Uri(uri);
            this.bytes = bytes;
            this.call = c;
        }

        public Request(string method, string uri, byte[] bytes)
        {
            this.method = "GET";
            this.protocol = "HTTP/1.1";
            this.response = null;
            this.isDone = false;
            this.maximumRetryCount = 8;
            this.acceptGzip = true;
            this.useCache = false;
            this.headers = new Dictionary<string, List<string>>();
            this.method = method;
            this.uri = new Uri(uri);
            this.bytes = bytes;
        }

        public void AddHeader(string name, string value)
        {
            name = name.ToLower().Trim();
            value = value.Trim();
            if (!this.headers.ContainsKey(name))
            {
                this.headers[name] = new List<string>();
            }
            this.headers[name].Add(value);
        }

        public void Send()
        {
            this.isDone = false;
            if (this.acceptGzip)
            {
                this.SetHeader("Accept-Encoding", "gzip");
            }
            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                try
                {
                    int maximumRetryCount = 0;
                    while (++maximumRetryCount < this.maximumRetryCount)
                    {
                        if (this.useCache)
                        {
                            string str = "";
                            if (etags.TryGetValue(this.uri.AbsoluteUri, out str))
                            {
                                this.SetHeader("if-none-match", str);
                            }
                        }
                        this.SetHeader("Host", this.uri.Host);
                        TcpClient client = new TcpClient();
                        client.Connect(this.uri.Host, this.uri.Port);
                        //UnityEngine.D.Log("Host:" + this.uri.Host + " port:" + this.uri.Port);
                        using (NetworkStream stream = client.GetStream())
                        {
                            this.WriteToStream(stream);
                            this.response = new Response(stream);
                        }
                        client.Close();
                        switch (this.response.status)
                        {
                            case 0x12d:
                            case 0x12e:
                            case 0x133:
                                {
                                    this.uri = new Uri(this.response.GetHeader("Location"));
                                    continue;
                                }
                        }
                        maximumRetryCount = this.maximumRetryCount;
                    }
                    if (this.useCache)
                    {
                        string header = this.response.GetHeader("etag");
                        if (header.Length > 0)
                        {
                            etags[this.uri.AbsoluteUri] = header;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Unhandled Exception, aborting request.");
                    Console.WriteLine(exception);
                }
                this.isDone = true;
                if (this.call != null)
                {
                    this.call(this, this.response);
                }
                
            });
        }

        public void SetHeader(string name, string value)
        {
            name = name.ToLower().Trim();
            value = value.Trim();
            if (!this.headers.ContainsKey(name))
            {
                this.headers[name] = new List<string>();
            }
            this.headers[name].Clear();
            this.headers[name].Add(value);
        }

        private void WriteToStream(Stream outputStream)
        {
            BinaryWriter writer = new BinaryWriter(outputStream);
            string[] textArray1 = new string[] { this.method.ToUpper(), " ", this.uri.PathAndQuery, " ", this.protocol };
            writer.Write(Encoding.ASCII.GetBytes(string.Concat(textArray1)));
            writer.Write(EOL);
            foreach (string str in this.headers.Keys)
            {
                foreach (string str2 in this.headers[str])
                {
                    writer.Write(Encoding.ASCII.GetBytes(str));
                    writer.Write(':');
                    writer.Write(Encoding.ASCII.GetBytes(str2));
                    writer.Write(EOL);
                }
            }
            if ((this.bytes != null) && (this.bytes.Length > 0))
            {
                if (!this.headers.ContainsKey("content-length"))
                {
                    int length = this.bytes.Length;
                    writer.Write(Encoding.ASCII.GetBytes("content-length: " + length.ToString()));
                    writer.Write(EOL);
                    writer.Write(EOL);
                }
                writer.Write(this.bytes);
            }
            else
            {
                writer.Write(EOL);
            }
        }
    }
}

