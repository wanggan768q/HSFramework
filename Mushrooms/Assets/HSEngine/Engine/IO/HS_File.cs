using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using HS.Base;
using System.Collections.Generic;

#if UNITY_WP8
using Directory = UnityEngine.Windows.Directory;
using File = UnityEngine.Windows.File;
#endif

namespace HS.IO
{
    public class HS_File
    {
        /// <summary>
        /// 路径是否存在
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void Delete(string fileName)
        {
            if(Exists(fileName))
            {
                File.Delete(fileName);
            }
        }


        /// <summary>
        /// 从文件里读取所有字节
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(string fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName, Encoding.UTF8);
                string s = sr.ReadToEnd();
                sr.Close();
                return Encoding.UTF8.GetBytes(s);
            }
            catch(Exception _e)
            {
                D.LogErrorForce(_e.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">Path.</param>
        /// <param name="data">Data.</param>
        public static void WriteAllBytes(string fileName, byte[] data)
        {
            try
            {
                string dir = HS_Path.GetDirectoryName(fileName);

                HS_Directory.CreateDirectory(dir);

                StreamWriter sw = new StreamWriter(fileName, false,Encoding.UTF8);
                foreach(byte b in data)
                {
                    sw.Write(b);
                }
                sw.Flush();
                sw.Close();
            }
            catch (Exception _e)
            {
                D.LogErrorForce(_e.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The all text.</returns>
        /// <param name="path">Path.</param>
        public static string ReadAllText(string fileName)
        {
            try
            {
                StreamReader sr = new StreamReader(fileName, Encoding.UTF8);
                string text = sr.ReadToEnd();
                sr.Close();
                return text;
            }
            catch (Exception _e)
            {
                D.LogErrorForce(_e.StackTrace);
            }
            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">Path.</param>
        /// <param name="data">Data.</param>
        public static void WriteAllText(string fileName, string data)
        {
            try
            {
                string dir = HS_Path.GetDirectoryName(fileName);

                HS_Directory.CreateDirectory(dir);

                StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);
                sw.Write(data);
                sw.Flush();
                sw.Close();
            }
            catch (Exception _e)
            {
                D.LogErrorForce(_e.StackTrace);
            }
        }

        /// <summary>
        /// 一行一行的写
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentList"></param>
        public static void WriteWithLine(string fileName, string[] contentList)
        {
            try
            {
                string dir = HS_Path.GetDirectoryName(fileName);

                HS_Directory.CreateDirectory(dir);

                StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);
                for (int i = 0; i < contentList.Length; i++)
                {
                    sw.WriteLine(contentList[i]);
                    sw.Flush();
                }
                sw.Flush();
                sw.Close();
            }
            catch (Exception _e)
            {
                D.LogErrorForce(_e.StackTrace);
            }
        }

        public static string[] ReadWithLine(string fileName)
        {
            if(!HS_File.Exists(fileName))
            {
                D.LogErrorForce(fileName + "不存在");
                return null;
            }
            List<string> contentList = new List<string>();
            try
            {
                StreamReader sr = new StreamReader(fileName, Encoding.UTF8);
                
                while (sr.EndOfStream)
                {
                    contentList.Add(sr.ReadLine());
                }
                sr.Close();
                
            }
            catch (Exception _e)
            {
                D.LogErrorForce(_e.StackTrace);
            }
            return contentList.ToArray();
        }
    }

}


