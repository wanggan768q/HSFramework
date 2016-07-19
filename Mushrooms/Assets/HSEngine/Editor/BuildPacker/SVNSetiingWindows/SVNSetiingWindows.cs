using UnityEngine;
using System.Collections;
using UnityEditor;
using HS.Edit.Base;
using System.Collections.Generic;
using HS.IO;
using HS.Base;

namespace HS.Edit
{
    [InitializeOnLoad]
    public sealed class GetSVNPath
    {
        public GetSVNPath()
        {
            SVNSetiingWindows.RefreshData();
        }
    }

    public class SVNSetiingWindows : EditorWindow
    {
        
//         [MenuItem("HSTool/SVNSetiingWindows", false, 1000)]
//         static void Init()
//         {
//             EditorWindow.GetWindow<SVNSetiingWindows>(true, "SVN相关设置");
//         }

        public static string SVN_ROOT
        {
            get
            {
                if(PlayerPrefs.HasKey(SVN_ROOT_KEY))
                {
                    string svn = PlayerPrefs.GetString(SVN_ROOT_KEY) + @"/bin/svn.exe";
                    if(!HS_File.Exists(svn))
                    {
                        svn = PlayerPrefs.GetString(SVN_ROOT_KEY) + @"/bin/TortoiseProc.exe";
                    }
                    return svn;
                }
                EditorUtility.DisplayDialog("异常提示", "请通过菜单 HSTool -> SVNSetiingWindows 来设置", "确认");
                return string.Empty;
            }
        }

        static List<string> _UpdatePaths = new List<string>();

        
        public static List<string> UpdatePaths
        {
            get { return _UpdatePaths; }
        }

        const string SVN_ROOT_KEY = "SVN_ROOT";
        const string RES_SAVE_KEY = "RES_SAVE_KEY";
        const string UPDATE_PATH_LIST_KEY = "UPDATE_PATH_LIST_KEY";
        
        HS_Button _SVNPathBut,_ResSaveBut;
        HS_Label _SVNLabel,_ResSaveLavel;
        HS_Area _SVNArea = HS_Area.Create("L", 458, 50,true);
        
        private SVNSetiingWindows()
        {
            _UpdatePaths.Clear();

            _SVNLabel = HS_Label.Create("SVN_ROOT");
            string svnButName = "请选择SVN ROOT";
            if (PlayerPrefs.HasKey(SVN_ROOT_KEY))
            {
                svnButName = PlayerPrefs.GetString(SVN_ROOT_KEY);
            }
            _SVNPathBut = HS_Button.Create(svnButName, () =>
            {
                string temp = EditorUtility.OpenFolderPanel("选择路径", "", "");
                if (!string.IsNullOrEmpty(temp))
                {
                    _SVNPathBut.Name = temp;
                }
                if (PlayerPrefs.HasKey(SVN_ROOT_KEY))
                {
                    PlayerPrefs.DeleteKey(SVN_ROOT_KEY);
                }
                PlayerPrefs.SetString(SVN_ROOT_KEY, _SVNPathBut.Name);
            });

            ////////////////////////////////////////////////

            _ResSaveLavel = HS_Label.Create("资源保存路径");
            string butResSaveName = "请选择资源保存路径";
            if (PlayerPrefs.HasKey(RES_SAVE_KEY))
            {
                butResSaveName = PlayerPrefs.GetString(RES_SAVE_KEY);
            }
            _ResSaveBut = HS_Button.Create(butResSaveName, () =>
            {
                string temp = EditorUtility.OpenFolderPanel("选择路径", "", "");
                if(!string.IsNullOrEmpty(temp))
                {
                    _ResSaveBut.Name = temp;
                }
                if (PlayerPrefs.HasKey(RES_SAVE_KEY))
                {
                    PlayerPrefs.DeleteKey(RES_SAVE_KEY);
                }
                PlayerPrefs.SetString(RES_SAVE_KEY, _ResSaveBut.Name);
            });


            ///////////////////////////////////////
            RefreshData();

        }

        public static void RefreshData()
        {
            if (PlayerPrefs.HasKey(UPDATE_PATH_LIST_KEY))
            {
                string temp = PlayerPrefs.GetString(UPDATE_PATH_LIST_KEY);
                if (!string.IsNullOrEmpty(temp))
                {
                    string[] paths = temp.Split('|');
                    for (int i=0;i<paths.Length;++i)
                    {
                        paths[i] = HS_Base.N2C(paths[i]);
                    }
                    _UpdatePaths.Clear();
                    _UpdatePaths.AddRange(paths);
                    D.LogForce("刷新SVN路径");
                }
            }
        }

        private void OnGUI()
        {
            _SVNArea.OnGUIUpdate();
            GUILayout.BeginHorizontal();
            _SVNLabel.OnGUIUpdate();
            _SVNPathBut.OnGUIUpdate();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            _ResSaveLavel.OnGUIUpdate();
            _ResSaveBut.OnGUIUpdate();
            GUILayout.EndHorizontal();
            _SVNArea.EndArea();
            
            HS_Label.Create("需要更新的目录：").OnGUIUpdate();
            HS_GUITool.BeginGroup();
            {
                foreach (string path in UpdatePaths)
                {
                    HS_Label.Create(path).OnGUIUpdate();
                }
            }
            HS_GUITool.EndGroup();


            GUILayout.BeginHorizontal();
            HS_Button.Create("添加目录", () => 
            {
                string temp = EditorUtility.OpenFolderPanel("选择路径", "", "");
                if (!string.IsNullOrEmpty(temp) && !_UpdatePaths.Contains(temp))
                {
                    _UpdatePaths.Add(temp);
                    temp = HS_Base.C2N(temp);
                    if (!PlayerPrefs.HasKey(UPDATE_PATH_LIST_KEY))
                    {
                        PlayerPrefs.SetString(UPDATE_PATH_LIST_KEY, temp);
                    }
                    else
                    {
                        string paths = PlayerPrefs.GetString(UPDATE_PATH_LIST_KEY);
                        paths += "|" + temp;
                        PlayerPrefs.SetString(UPDATE_PATH_LIST_KEY, paths);
                    }
                }
                PlayerPrefs.Save();
            }).OnGUIUpdate();

            HS_Button.Create("清空目录", () =>
            {
                if(EditorUtility.DisplayDialog("提示", "确定清空目录吗？", "确认","取消"))
                {
                    _UpdatePaths.Clear();
                    PlayerPrefs.DeleteKey(UPDATE_PATH_LIST_KEY);
                    PlayerPrefs.Save();
                }
            }).OnGUIUpdate();
            GUILayout.EndHorizontal();

            /*
            HS_Button.Create("测试更新", () =>
            {
                foreach (string p in UpdatePaths)
                {
                    RunCommand(p, SVN_ROOT, " clear", true);
                    RunCommand(p, SVN_ROOT, " uodate", true);
                }
            }).OnGUIUpdate();
            */
        }

        /// <summary>
        /// 更新设置的所有目录
        /// </summary>
        public static void Update()
        {
            if (PlayerPrefs.HasKey(UPDATE_PATH_LIST_KEY))
            {
                RefreshData();
                foreach (string p in _UpdatePaths)
                {
                    if(SVN_ROOT.Contains("svn.exe"))
                    {
                        RunCommand(p, SVN_ROOT, " cleanup", true);
                        RunCommand(p, SVN_ROOT, " update", true);
                    }
                    else
                    {
                        RunCommand(p, SVN_ROOT, " /command:cleanup /path:" + p + " /closeonend:1", true);
                        RunCommand(p, SVN_ROOT, " /command:update /path:" + p + " /closeonend:1", true);
                    }
                }
            }
        }

        static void RunCommand(string work, string cmd, string arg, bool printError)
        {
            D.LogForce(cmd + arg);
            System.Diagnostics.Process foo = new System.Diagnostics.Process();
            foo.StartInfo.FileName = cmd;
            foo.StartInfo.Arguments = arg;
            foo.StartInfo.WorkingDirectory = work;
            foo.StartInfo.UseShellExecute = false;
            foo.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            foo.StartInfo.RedirectStandardOutput = false;
            foo.StartInfo.RedirectStandardError = true;
            foo.Start();
            //string output = foo.StandardOutput.ReadToEnd();
            string err = foo.StandardError.ReadToEnd();
            foo.WaitForExit();
            if (printError && err.Length > 0)
            {
                D.LogErrorForce(err);
            }
        }
    }
}


