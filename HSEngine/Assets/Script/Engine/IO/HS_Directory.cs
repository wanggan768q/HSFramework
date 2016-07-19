using UnityEngine;
using System.Collections;
using System.IO;

#if UNITY_WP8
using Directory = UnityEngine.Windows.Directory;
#endif


namespace HS.IO
{
    public class HS_Directory
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="path"></param>
        public static bool Exists(string path)
        {
            return Directory.Exists(path); ;
        }

    }

}