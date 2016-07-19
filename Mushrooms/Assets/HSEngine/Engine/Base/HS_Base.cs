using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
using System.Xml.Serialization;
using HS.IO;

namespace HS.Base
{
    public static class HS_Base
    {

        public static T FindProperty<T>(Transform root, string path) where T : Component
        {
            string[] arr = path.Split('/');
            for (int index = 0; index < arr.Length; index++)
            {
                bool find = false;
                string key = arr[index];
                foreach (Transform child in root)
                {
                    D.LogErrorForce("child: " + child.name);
                    if (child.name == key)
                    {
                        root = child;
                        find = true;
                        break;
                    }
                }
                if (!find)
                    return null;
            }
            return root.GetComponent<T>();
        }

        /// <summary>
        /// 获取 StreamingAssets 资源路径
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="subfolderName"></param>
        /// <returns></returns>
        public static string GetStreamingAssetsFilePath(string filename, string subfolderName = "")
        {
            string subfolderData = "";
            if (subfolderName != "")
            {
                subfolderData = subfolderName + "/";
            }
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    return "jar:file://" + Application.dataPath + "!/assets/" + subfolderData + filename;
                case RuntimePlatform.IPhonePlayer:
                    return "file://" + Application.dataPath + "/Raw/" + subfolderData + filename;
                default:
                    return "file://" + Application.streamingAssetsPath + "/" + subfolderData + filename;
            }
        }
        
        
        /// <summary>
        /// 删除当前目录下想所有文件 保留当前目录
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static bool DeleteDir(string path)
        {
            try
            { // 清除空格 
                //strPath = @strPath.Trim().ToString(); // 判断文件夹是否存在 
                if (HS_Directory.Exists(path))
                { 
                    // 获得文件夹数组 
                    string[] strDirs = System.IO.Directory.GetDirectories(path); // 获得文件数组 
                    string[] strFiles = System.IO.Directory.GetFiles(path); // 遍历所有子文件夹 
                    foreach (string strFile in strFiles)
                    { 
                        System.IO.File.Delete(strFile);
                        HS_File.Delete(strFile);
                    } 
                    // 遍历所有文件 
                    foreach (string strdir in strDirs)
                    { 
                        // 删除文件 
                        System.IO.Directory.Delete(strdir, true);
                    }
                } 
                // 成功 
                return true;
            }
            catch (Exception Exp) // 异常处理         
            { 
                // 异常信息 
                D.LogError(Exp.ToString());
                return false;
            }
        }

        /// <summary>
        /// 电脑平台 删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void SystemDeleteFolder(string dir)
        {
            if(!Directory.Exists(dir))
            {
                return;
            }
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        SystemDeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }

        /// <summary>
        /// 电脑平台 拷贝目录
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        public static void SystemCopyDirectory(string sourcePath, string destinationPath,params string[] filterPrefix)
        {
            try
            {
                DirectoryInfo info = new DirectoryInfo(sourcePath);
                Directory.CreateDirectory(destinationPath);
                foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
                {
                    bool isSetp = false;
                    foreach (string filter in filterPrefix)
                    {
                        if (fsi.FullName.Contains(filter))
                        {
                            isSetp = true;
                            break;
                        }
                    }
                    if (isSetp)
                    {
                        continue;
                    }
                    string destName = Path.Combine(destinationPath, fsi.Name);
                    if (fsi is System.IO.FileInfo)
                    {
                        File.Copy(fsi.FullName, destName);
                    }
                    else
                    {
                        Directory.CreateDirectory(destName);
                        SystemCopyDirectory(fsi.FullName, destName, filterPrefix);
                    }
                }
            }
            catch
            {
            	
            }
            
        }
        

        /// <summary>
        /// 合并byteArray
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static byte[] ConsolidationByteArray(params byte[][] arg)
        {
            int size = 0;
            foreach (byte[] ba in arg)
            {
                size += ba.Length;
            }
            int index = 0;
            byte[] buf = new byte[size];
            foreach (byte[] ba in arg)
            {
                Array.Copy(ba, 0, buf, index, ba.Length);
                index += ba.Length;
            }
            return buf;
        }


        

        /// <summary>
        /// 中文转成数值
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string C2N(string text)
        {
            string number = string.Empty;
            if(!string.IsNullOrEmpty(text))
            {
                for (int i = 0; i < text.Length; ++i)
                {
                    int temp = char.ConvertToUtf32(text, i);
                    number = new StringBuilder().Append(number).
                        Append(number == string.Empty ? "" : ".").Append(temp).ToString();
                }
            }
            return number;
        }

        /// <summary>
        /// 数字转字汉字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string N2C(string number)
        {
            string c = string.Empty;
            string[] tempStr = number.Split("."[0]);
            foreach(string s in tempStr)
            {
                try
                {
                    int tempInt = int.Parse(s);
                    c = c + char.ConvertFromUtf32(tempInt);
                }
                catch
                {
                    D.LogForce(s);
                }
                
                
            }
            return c;
        }
    }

}
