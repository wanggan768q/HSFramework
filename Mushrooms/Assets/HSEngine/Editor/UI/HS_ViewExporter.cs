using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using HS.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using HS.IO;

namespace HS.Edit
{
    public class HS_ViewExporter : MonoBehaviour
    {

        [MenuItem("HSTool/导出 View", false)]
        static public void ExportView()
        {
            Export("Assets/SubAssets/Res/Prefabs/UI", "Assets/Script/VLayer", "Assets/SubAssets/Res/Prefabs/UIListCell");
        }

        struct Property
        {
            public string name;
            public string path;
            public Component com;
            public System.Type type;
            public bool isPrivate;

            public Property(string name, string path, Component com, bool isPrivate = false)
            {
                this.name = "_" + LowerFirstLetter(Regex.Replace(name, "[^_0-9a-zA-Z]", "_"));
                this.path = path;
                this.com = com;
                this.type = com.GetType();
                this.isPrivate = isPrivate;
            }
        }

        public static readonly string VIEW_UI = "ViewUI";
        public static readonly string VIEW_PROPERTY = "ViewProperty";

        const string TAB1 = "\t";
        const string TAB2 = "\t\t";
        const string TAB3 = "\t\t\t";
        const string TAB4 = "\t\t\t\t";
        const string TAB5 = "\t\t\t\t\t";

        static private Dictionary<int, string> mObjectNameIds = new Dictionary<int, string>();
        static private Dictionary<Transform, string> mObjectNames = new Dictionary<Transform, string>();
        static private Dictionary<Transform, List<Property>> mPropertyDict = new Dictionary<Transform, List<Property>>();
        static private Dictionary<Transform, Dictionary<string, string>> mPropertyNameDict = new Dictionary<Transform, Dictionary<string, string>>();
        static private Dictionary<string, Transform> mPathDict = new Dictionary<string, Transform>();
        static private List<Transform> mUIListViewList = new List<Transform>();

        static private List<System.Type> mDefinedComponents = new List<System.Type>() {
            typeof(HS_ComponentBase), typeof(Button), typeof(Toggle), typeof(ToggleGroup), typeof(Slider), typeof(InputField), typeof(ScrollRect)
        };

        static private List<System.Type> mDefinedScripts = new List<System.Type>() {
            typeof(Text), typeof(Image), typeof(RawImage), typeof(Animator), typeof(RectTransform),
        };

        static public void Export(string prefabPath, string scriptPath, string cellPath, string baseClassName = "HS_ViewBase", Transform root = null)
        {
            if (root == null)
            {
                GameObject c = GameObject.Find("UI").gameObject;
                if (c == null)
                {
                    Debug.LogError("No UI Compent in Hierarchy.");
                    return;
                }
                root = c.transform;
            }
            if (scriptPath.IndexOf("Assets/") != 0 || prefabPath.IndexOf("Assets/") != 0)
            {
                Debug.LogError("Path alway start with \"Assets/\" .");
                return;
            }
            HS_Directory.CreateDirectory(prefabPath);
            for (int i = root.childCount - 1; i >= 0; --i)
            {
                Transform child = root.GetChild(i);
                if (child.gameObject.activeSelf && child.gameObject.CompareTag(VIEW_UI))
                {
                    string viewName = child.gameObject.name;
                    mObjectNameIds.Clear();
                    mObjectNames.Clear();
                    mPathDict.Clear();
                    mPropertyDict.Clear();
                    mPropertyNameDict.Clear();
                    mUIListViewList.Clear();

                    ParseCanvas(child, child, "");

                    for (int j = 0; j < mUIListViewList.Count; j++)
                    {
                        HS_ListViewBase listView = mUIListViewList[j].GetComponent<HS_ListViewBase>();
                        if (listView != null)
                        {
                            GameObject cellPrefab = listView.GetCellPrefab();
                            if (cellPrefab == null)
                            {
                                string cellName = listView.GetCellPrefabName();
                                string cellPrefabPath = GetCellPathName(cellName, cellPath);
                                Debug.Log("load path:" + cellPrefabPath);
                                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath(cellPrefabPath, typeof(GameObject)) as GameObject;
                                cellPrefab = GameObject.Instantiate(prefab) as GameObject;
                                cellPrefab.transform.SetParent(listView.GetGridContent().transform);
                                cellPrefab.transform.localScale = Vector3.one;
                                cellPrefab.name = "CellPrefab";
                            }
                            if (cellPrefab == null)
                            {
                                Debug.LogError("No cell prefab:" + mUIListViewList[j].name);
                                break;
                            }
                            listView.SetCellPrefab(cellPrefab);
                            ParseCanvas(cellPrefab.transform, cellPrefab.transform, "");
                        }
                        else
                        {
                            mUIListViewList.RemoveAt(j--);
                        }
                    }

                    string newBaseClassName =  GenerateViewBaseClass(child, prefabPath, scriptPath + "/Base", cellPath, baseClassName);

                    GenerateViewClass(scriptPath, viewName, newBaseClassName);
                }
            }

            Debug.Log("Export is completed");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static private void ParseCanvas(Transform panel, Transform transform, string path)
        {
            foreach (Transform child in transform)
            {
                mObjectNames[child] = child.name;
                string childPath = child.name;
                if (!string.IsNullOrEmpty(path))
                {
                    childPath = path + "/" + childPath;
                }
                mPathDict[childPath] = child;

                bool found = false, parseChildren = true;
                Component com = null;
                do
                {
                    com = child.GetComponent<HS_ListViewBase>();
                    if (com != null)
                    {
                        found = true;
                        parseChildren = false;
                        AddProperty(panel, com, child.name, childPath);
                        mUIListViewList.Add(child);
                        break;
                    }

                    bool isProperty = child.gameObject.CompareTag(VIEW_PROPERTY);
                    com = child.GetComponent<Canvas>();
                    if (com != null)
                    {
                        found = true;
                        AddProperty(panel, com, child.name, childPath, !isProperty);
                        break;
                    }

                    for (int i = 0; i < mDefinedComponents.Count; i++)
                    {
                        com = child.GetComponent(mDefinedComponents[i]);
                        if (com != null)
                        {
                            found = true;
                            AddProperty(panel, com, child.name, childPath);
                            if (com is HS_ComponentBase)
                            {
                                parseChildren = false;
                            }
                            break;
                        }
                    }
                    if (!found && isProperty)
                    {
                        for (int i = 0; i < mDefinedScripts.Count; i++)
                        {
                            com = child.GetComponent(mDefinedScripts[i]);
                            if (com != null)
                            {
                                found = true;
                                AddProperty(panel, com, child.name, childPath);
                                break;
                            }
                        }
                    }
                } while (false);


                if (parseChildren && child.childCount > 0)
                {
                    ParseCanvas(panel, child, childPath);
                }
            }
        }

        static private void GenerateViewClass(string scriptPath,string childName,string baseClassName)
        {
            
            StringBuilder code = new StringBuilder();
            string className = "" + childName + "View";
            string fileName = scriptPath + "/View/" + className + ".cs";
            if (File.Exists(fileName))
            {
                return;
            }

            code.AppendLine("using System;");
            code.AppendLine("using System.Collections;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using UnityEngine;");
            code.AppendLine("using HS.Base;");
            code.AppendLine("using HS.UI;");
            code.AppendLine("using HS.Manager;");

            code.AppendLine("");
            code.AppendLine("public class " + className + " : " + baseClassName + " , IForward");
            code.AppendLine("{");
            code.AppendLine("");

            code.AppendLine(TAB1 + "protected override void OnCreated()");
            code.AppendLine(TAB1 + "{");
            code.AppendLine(TAB2 + "base.OnCreated ();");
            code.AppendLine(TAB1 + "}");

            code.AppendLine(TAB1 + "protected override void OnStarted()");
            code.AppendLine(TAB1 + "{");
            code.AppendLine(TAB2 + "base.OnStarted ();");
            code.AppendLine(TAB1 + "}");

            code.AppendLine(TAB1 + "protected override void OnOpened()");
            code.AppendLine(TAB1 + "{");
            code.AppendLine(TAB2 + "base.OnOpened ();");
            code.AppendLine(TAB1 + "}");

            code.AppendLine(TAB1 + "protected override void OnClosed()");
            code.AppendLine(TAB1 + "{");
            code.AppendLine(TAB2 + "base.OnClosed ();");
            code.AppendLine(TAB1 + "}");
            

            code.AppendLine("}");

            WriteString(fileName, code.ToString());
        }

        static private string GenerateViewBaseClass(Transform panel, string prefabPath, string scriptPath, string cellPath, string baseClassName)
        {
            // generate script
            StringBuilder code = new StringBuilder();
            string className = "Base" + UpperFirstLetter(panel.name) + "View";
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using UnityEngine;");
            code.AppendLine("using HS.Base;");
            code.AppendLine("using HS.UI;");
            code.AppendLine("using HS.Manager;");

            code.AppendLine("");
            code.AppendLine("public class " + className + " : " + baseClassName);
            code.AppendLine("{");

            List<Property> properties;
            if (mPropertyDict.TryGetValue(panel, out properties))
            {
                for (int i = 0; i < properties.Count; i++)
                {
                    Property p = properties[i];
                    if (!p.isPrivate)
                    {
                        code.Append(TAB1).AppendLine(string.Format("protected {0} {1};", p.type, p.name));
                    }
                }
            }

            string viewPath = "";
            int index = prefabPath.LastIndexOf("/Resources");
            if (index > 0)
            {
                if (prefabPath[index + 10] == '/')
                {
                    viewPath = prefabPath.Substring(index + 11);
                }
            }
            code.Append(TAB1).AppendLine("");
            code.Append(TAB1).AppendLine("internal override GameObject GetViewPrefab()");
            code.Append(TAB1).AppendLine("{");
            code.Append(TAB2).AppendLine(string.Format("return HS_ResourceManager.LoadAsset<GameObject>(\"{0}\");", viewPath + (string.IsNullOrEmpty(viewPath) ? "" : "/") + panel.name));
            code.Append(TAB1).AppendLine("}");

            code.Append(TAB1).AppendLine("");
            code.Append(TAB1).AppendLine("protected override void OnCreated()");
            code.Append(TAB1).AppendLine("{");


            code.Append(TAB2).AppendLine("base.OnCreated();");

            code.Append(TAB1).AppendLine("");

            code.Append(TAB2).AppendLine("Transform transform = this.transform;");
            code.Append(TAB2).AppendLine("if (transform == HS_ViewManager.root.transform) return;");
            if (properties != null)
            {
                foreach (Property p in properties)
                {
                    code.Append(TAB2).AppendLine("");
                    System.Type type = p.type;
                    string propName = "this." + p.name;
                    if (p.isPrivate)
                    {
                        propName = p.name;
                        code.Append(TAB2).AppendLine(string.Format("{2} {0} = HS_Base.FindProperty<{2}>(transform, \"{1}\");", propName, p.path, type));
                    }
                    else
                    {
                        code.Append(TAB2).AppendLine(string.Format("{0} = HS_Base.FindProperty<{2}>(transform, \"{1}\");", propName, p.path, type));
                    }

                    if (type == typeof(Button))
                    {
                        code.Append(TAB2).AppendLine("this.RegisterButtonClickEvent (" + propName + ");");
                    }
                    else if (type == typeof(Slider))
                    {
                        code.Append(TAB2).AppendLine("this.RegisterSliderEvent (" + propName + ");");
                    }
                    else if (type == typeof(Toggle))
                    {
                        code.Append(TAB2).AppendLine("this.RegisterToggleEvent (" + propName + ");");
                    }
                    else if (type == typeof(Dropdown))
                    {
                        code.Append(TAB2).AppendLine("this.RegisterDropDownEvent (" + propName + ");");
                    }
                    else if (type == typeof(InputField))
                    {
                        code.Append(TAB2).AppendLine("this.RegisterInputFieldEvent (" + propName + ");");
                    }
                    else if (type == typeof(Text))
                    {
                        HS_LocalizationText localizationText = p.com.transform.GetComponent<HS_LocalizationText>();
                        if (localizationText != null && localizationText.languageKey != "")
                        {
                            //Localization.Format ();
                            code.Append(TAB2).AppendLine("LocalizationText " + p.name + "LocalizationText = " + p.name + ".transform.GetComponent<LocalizationText> ();");
                            code.Append(TAB2).AppendLine(propName + ".text = Localization.Format (" + p.name + "LocalizationText.languageKey);");
                        }
                        else
                        {
                            code.Append(TAB2).AppendLine(propName + ".text = \"" + Regex.Replace(((Text)p.com).text, "(\\\"|\\\\)", "\\$0") + "\";");
                        }
                    }
                    else if (type == typeof(HS_UIListView))
                    {
                        code.Append(TAB2).AppendLine(propName + ".onInit += OnListViewInit;");
                        code.Append(TAB2).AppendLine(propName + ".onCellCreated += OnCellCreated;");
                        if (type == typeof(HS_UIListView))
                        {
                            code.Append(TAB2).AppendLine(propName + ".onClick += OnListViewClick;");
                            code.Append(TAB2).AppendLine(propName + ".onSelected += OnListViewSelected;");
                            code.Append(TAB2).AppendLine(propName + ".onDeselected += OnListViewDeselected;");
                        }
                    }
                }
            }
            code.Append(TAB1).AppendLine("}");

            System.Action<Transform, string> generateCellClass = delegate (Transform cell, string name)
            {
                List<Property> props;
                if (!mPropertyDict.TryGetValue(cell, out props))
                {
                    props = new List<Property>();
                }
                code.Append(TAB2).AppendLine("public class " + name);
                code.Append(TAB2).AppendLine("{");
                foreach (Property p in props)
                {
                    if (!p.isPrivate)
                    {
                        code.Append(TAB3).AppendLine(string.Format("public {0} {1};", p.type, p.name));
                    }
                }
                code.Append(TAB2).AppendLine("}");
                code.Append(TAB2).AppendLine("");
            };
            System.Action<Transform, string, string, string> generateCellProperty = delegate (Transform cell, string cellClassName, string insName, string space)
            {
                List<Property> props;
                if (!mPropertyDict.TryGetValue(cell, out props))
                {
                    props = new List<Property>();
                }
                code.Append(space).AppendLine(cellClassName + " " + insName + " = new " + cellClassName + "();");
                foreach (Property p in props)
                {
                    if (p.isPrivate)
                    {
                        code.Append(space).AppendLine(string.Format("{2} {0} = HS_Base.FindProperty<{2}>(t, \"{1}\");", p.name, p.path, p.type));
                        if (p.type == typeof(Text))
                        {
                            code.Append(space).AppendLine(p.name + ".text = \"" + Regex.Replace(((Text)p.com).text, "(\\\"|\\\\)", "\\$0") + "\";");
                        }
                    }
                    else
                    {
                        code.Append(space).AppendLine(string.Format("{0}.{1} = HS_Base.FindProperty<{3}>(t, \"{2}\");", insName, p.name, p.path, p.type));
                    }
                }
            };

            foreach (Transform t in mUIListViewList)
            {
                HS_ListViewBase listView = t.GetComponent<HS_ListViewBase>();
                GameObject cellPrefab = listView.GetCellPrefab();
                if (cellPrefab == null)
                {
                    Debug.LogError("No cell prefab:" + t.name);
                    break;
                }

                string panelName = mObjectNames[t];
                string panelClassName = "TV" + UpperFirstLetter(panelName);
                string cellStructName = "Cell";
                code.Append(TAB1).AppendLine("");
                code.Append(TAB1).AppendLine("#region " + panelName);
                code.Append(TAB1).AppendLine("protected static class " + panelClassName);
                code.Append(TAB1).AppendLine("{");

                mObjectNameIds.Clear();

                generateCellClass(cellPrefab.transform, "Cell");

                code.Append(TAB2).AppendLine("static public " + cellStructName + " Get(HS_UIListViewCell cell)");
                code.Append(TAB2).AppendLine("{");
                code.Append(TAB3).AppendLine("Transform t = cell.transform;");

                generateCellProperty(cellPrefab.transform, "Cell", "obj", TAB3);

                code.Append(TAB3).AppendLine("return obj;");
                code.Append(TAB2).AppendLine("}");

                code.Append(TAB1).AppendLine("}");
                code.Append(TAB1).AppendLine("#endregion");
            }

            code.AppendLine("}");

            WriteString(scriptPath + "/" + className + ".cs", code.ToString());

            foreach (Transform t in mUIListViewList)
            {
                HS_ListViewBase listView = t.GetComponent<HS_ListViewBase>();
                if (listView != null)
                {
                    string cellName = GetCellPrefabName(panel, t);
                    //Debug.Log("RecordCellPrefabName " + cellName);
                    listView.RecordCellPrefabName(cellName);
                    GameObject cell = listView.GetCellPrefab();
                    if (cell != null)
                    {
                        string cellPrefabPath = GetCellPathName(cellName, cellPath);
                        HS_Directory.CreateDirectory(cellPath);
                        PrefabUtility.CreatePrefab(cellPrefabPath, cell, ReplacePrefabOptions.ConnectToPrefab);
                    }

                    GameObject gridContent = listView.GetGridContent();
                    for (int i = gridContent.transform.childCount - 1; i >= 0; i--)
                    {
                        GameObject.DestroyImmediate(gridContent.transform.GetChild(i).gameObject);
                    }
                }
            }

            MakeDirs(prefabPath);
            PrefabUtility.CreatePrefab(prefabPath + "/" + panel.name + ".prefab", panel.gameObject, ReplacePrefabOptions.ConnectToPrefab);

            GameObject.DestroyImmediate(panel.gameObject);

            return className;
        }

        public static void WriteString(string filePath, string content)
        {
            MakeDirs(filePath);
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }

        public static void MakeDirs(string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        static private void AddProperty(Transform panel, Component com, string name, string path, bool isPrivate = false)
        {
            name = GetUniqueName(panel, name, path);
            List<Property> properties;
            if (!mPropertyDict.TryGetValue(panel, out properties))
            {
                properties = new List<Property>();
                mPropertyDict[panel] = properties;
            }
            properties.Add(new Property(name, path, com, isPrivate));
        }

        static private string GetUniqueName(Transform t, string name, string path)
        {
            int i = 0;
            Dictionary<string, string> names;
            if (!mPropertyNameDict.TryGetValue(t, out names))
            {
                names = new Dictionary<string, string>();
                mPropertyNameDict[t] = names;
            }
            if (names.ContainsValue(path))
            {
                throw new UnityException("With path under the same object: " + path);
            }
            string old = name;
            while (names.ContainsKey(name))
            {
                name = old + (++i);
            }
            names.Add(name, path);
            return name;
        }

        static private string GetCellPathName(string cellName, string cellPath)
        {
            return cellPath + HS_Path.Separator + cellName + ".prefab";
        }

        static private string GetCellPrefabName(Transform panel, Transform listView)
        {
            return panel.name + "_" + UpperFirstLetter(mObjectNames[listView]) + "_Cell";
        }

        static private string UpperFirstLetter(string str)
        {
            str = str.Replace(" ", "");
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        static private string LowerFirstLetter(string str)
        {
            str = str.Replace(" ", "");
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
    }
}


