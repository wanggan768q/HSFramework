using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace HS.IO
{
    public class HS_Path
    {
        /// <summary>
        /// 分隔符
        /// </summary>
        public static char Separator
        {
            get
            {
                char s =  '/';
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    s = Path.AltDirectorySeparatorChar;
                }
                else
                {
                    s = Path.DirectorySeparatorChar;
                }
                return s;
            }
        }

        /// <summary>
        /// 通过 '/' 分割路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] SplitPath(string path)
        {
            return path.Split(Separator);
        }

        public static string CombinePath(params string[] args)
        {
            return string.Join(Separator + "", args);
        }

        /// <summary>
        /// 得到当前路径 例: c:/a/b.txt  返回 c:/a
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string path)
        {
            string dir = string.Empty;
            int index = path.LastIndexOf("/");
            dir = path.Substring(0, index);
            return dir;
        }

        /// <summary>
        /// 得到文件名 例: c:/a/b.txt  返回 b.txt
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileName(string file)
        {
            string fileName = string.Empty;
            int index = file.LastIndexOf("/");
            fileName = file.Substring(index, file.Length);
            return fileName;
        }

        /// <summary>
        /// 得到当前目录名字 例: c:/a/b.txt  返回 a
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetCurrentFolder(string path)
        {
            string folderName = string.Empty;
            int index = path.LastIndexOf("/");
            folderName = path.Substring(index + 1, path.Length - index - 1);
            return folderName;
        }
    }
}

