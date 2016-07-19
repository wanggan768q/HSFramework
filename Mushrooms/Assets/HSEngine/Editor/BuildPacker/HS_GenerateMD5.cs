using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HS.Edit.Build
{
    public class HS_GenerateMD5
    {
        public static void GenerateMD5(string baseDir, string json)
        {
            HS_GenerateMD5 m = new HS_GenerateMD5();
            m.Generate(baseDir, json);
        }

        private string _BaseDir = string.Empty;
        public void Generate(string baseDir, string json)
        {
            _BaseDir = baseDir;
            string jsonStr = ProcessDirectory(baseDir, json);
            string outFile = baseDir + "/Mainfest.data";
            StreamWriter stream = new StreamWriter(outFile);
            stream.WriteLine("[" + jsonStr + "]");
            stream.Close();
            D.LogForce("Mainfest.data " + outFile);
        }

        string ProcessDirectory(string dir, string json)
        {
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                json = ProcessFile(dir, file, json);
            }
            string[] dirs = Directory.GetDirectories(dir);
            foreach (string sub in dirs)
            {
                json = ProcessDirectory(sub, json);
            }
            return json;
        }

        string ProcessFile(string dir, string file, string json)
        {
            if (file.EndsWith(".meta") || file.EndsWith("Mainfest.data") || file.EndsWith(".svn") || file.EndsWith(".bat"))
            {
                return json;
            }
            string md5 = GetMD5Str(new FileStream(file, FileMode.Open));
            if (json.Length > 1)
                json += ",";
            json += "{\"md5\":\"" + md5 + "\",";
            json += "\"name\":\"" + file.Substring(_BaseDir.Length + 1).Replace("\\", "\\\\") + "\",";
            json += "\"size\":" + (new FileInfo(file)).Length + "}";
            return json;
        }

        public string GetMD5Str(Stream stream)
        {
#if !UNITY_WP8
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
#endif
            return string.Empty;
        }
    }
}

