using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace HS.Net.HTTP
{
    public class Response
    {
        public byte[] bytes;
        private Dictionary<string, List<string>> headers = new Dictionary<string, List<string>>();
        public string message = "OK";
        public int status = 200;

        public Response(Stream stream)
        {
            this.ReadFromStream(stream);
        }

        public Response(Stream stream,System.Action action)
        {
            this.ReadFromStream(stream);
        }

        private void AddHeader(string name, string value)
        {
            name = name.ToLower().Trim();
            value = value.Trim();
            if (!this.headers.ContainsKey(name))
            {
                this.headers[name] = new List<string>();
            }
            this.headers[name].Add(value);
        }

        public string GetHeader(string name)
        {
            name = name.ToLower().Trim();
            if (!this.headers.ContainsKey(name))
            {
                return string.Empty;
            }
            return this.headers[name][this.headers[name].Count - 1];
        }

        public List<string> GetHeaders(string name)
        {
            name = name.ToLower().Trim();
            if (!this.headers.ContainsKey(name))
            {
                this.headers[name] = new List<string>();
            }
            return this.headers[name];
        }

        private void ReadFromStream(Stream inputStream)
        {
            char[] separator = new char[] { ' ' };
            string[] strArray = this.ReadLine(inputStream).Split(separator);
            MemoryStream stream = new MemoryStream();
            if (!int.TryParse(strArray[1], out this.status))
            {
                throw new HTTPException("Bad Status Code");
            }
            this.message = string.Join(" ", strArray, 2, strArray.Length - 2);
            this.headers.Clear();
            while (true)
            {
                string[] strArray2 = this.ReadKeyValue(inputStream);
                if (strArray2 == null)
                {
                    break;
                }
                this.AddHeader(strArray2[0], strArray2[1]);
            }
            if (!(this.GetHeader("transfer-encoding") == "chunked"))
            {
                int num3 = 0;
                try
                {
                    num3 = int.Parse(this.GetHeader("content-length"));
                }
                catch
                {
                    num3 = 0;
                }
                for (int i = 0; i < num3; i++)
                {
                    stream.WriteByte((byte) inputStream.ReadByte());
                }
            }
            else
            {
                while (true)
                {
                    string s = this.ReadLine(inputStream);
                    //Console.WriteLine("HexLength:" + s);
                    if (s == "0")
                    {
                        break;
                    }
                    int num = int.Parse(s, NumberStyles.AllowHexSpecifier);
                    for (int j = 0; j < num; j++)
                    {
                        stream.WriteByte((byte) inputStream.ReadByte());
                    }
                    inputStream.ReadByte();
                    inputStream.ReadByte();
                }
                while (true)
                {
                    string[] strArray3 = this.ReadKeyValue(inputStream);
                    if (strArray3 == null)
                    {
                        break;
                    }
                    this.AddHeader(strArray3[0], strArray3[1]);
                }
            }
            if (this.GetHeader("content-encoding").Contains("gzip"))
            {
                MemoryStream stream2 = new MemoryStream();
				//TODO...
               stream.Seek(0, SeekOrigin.Begin);
               using (GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress))
               {
                   byte[] buffer = new byte[0x400];
                   int count = 0;
                   while ((count = stream3.Read(buffer, 0, buffer.Length)) > 0)
                   {
                       stream2.Write(buffer, 0, count);
                   }
               }
                this.bytes = stream2.ToArray();
            }
            else
            {
                this.bytes = stream.ToArray();
            }
        }

        private string[] ReadKeyValue(Stream stream)
        {
            string str = this.ReadLine(stream);
            if (str == "")
            {
                return null;
            }
            int index = str.IndexOf(':');
            if (index == -1)
            {
                return null;
            }
            return new string[] { str.Substring(0, index).Trim(), str.Substring(index + 1).Trim() };
        }

        private string ReadLine(Stream stream)
        {
            List<byte> list = new List<byte>();
            while (true)
            {
                byte item = (byte) stream.ReadByte();
                if (item == Request.EOL[1])
                {
                    return Encoding.ASCII.GetString(list.ToArray()).Trim();
                }
                list.Add(item);
            }
        }

        public string Asset
        {
            get
            {
                throw new NotSupportedException("This can't be done, yet.");
            }
        }

        public string Text
        {
            get
            {
                if (this.bytes == null)
                {
                    return "";
                }
                return Encoding.UTF8.GetString(this.bytes);
            }
        }
    }
}

