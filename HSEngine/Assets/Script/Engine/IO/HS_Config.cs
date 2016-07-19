using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using HS.Base;

namespace HS.IO
{
    /// <summary>
    /// 加载INI配置文件
    /// </summary>
    public class HS_Config : HS_Singleton<HS_Config>
    {
        private Dictionary<string, Dictionary<string, string>> _Ini = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void LoadConfigINI(TextAsset text)
        {
            LoadConfigINI(text.bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public void LoadConfigINI(byte[] config)
        {
            StreamReader sr = new StreamReader(new MemoryStream(config));
            string line;
            string section = null;
            _Ini.Clear();

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                D.Log(line);
                if (line.StartsWith("#") || string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    section = line.Substring(1, line.Length - 2);
                    if (!_Ini.ContainsKey(section))
                    {
                        _Ini.Add(section, new Dictionary<string, string>());
                    }
                }
                else
                {
                    string[] array = line.Split('=');
                    string k = array[0].Replace(" ", "");
                    string v = array[1].TrimStart(' ');
                    Dictionary<string, string> kv = _Ini[section];
                    kv.Add(k, v);
                }

            }
        }

        /// <summary>
        /// 是否存在 section
        /// </summary>
        /// <param name="section">section name</param>
        /// <returns></returns>
        public bool ContainsSection(string section)
        {
            return _Ini.ContainsKey(section) ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string section, string key)
        {
            if (_Ini.ContainsKey(section))
            {
                Dictionary<string, string> kv = _Ini[section];
                if (kv.ContainsKey(key))
                {
                    return kv[key];
                }
            }
            return null;
        }
    }
}

