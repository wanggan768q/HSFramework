using UnityEngine;
using UnityEditor;
using HS.Edit.Base;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditorInternal;

public class test  : EditorWindow,IEditorWindowsMessage
{
    [MenuItem("Assets/Add Animator Controller", false, 0)]
    static void Windows()
    {
        test pWin = EditorWindow.GetWindow<test>(true, "动作控制编辑器");
        pWin.minSize = new Vector2(1000, 1000);
        pWin.maxSize = new Vector2(1000, 1000);
        pWin.Init();
        pWin.Show();

        
    }

    HS_Button but;
    HS_Label label;
    HS_Space space;
    HS_Popup<HS_Text> popup;
    HS_Text text;
    HS_Toggle toggle;
    List<GUILayoutOption> guiLayout = new List<GUILayoutOption>();

    HS_Area area;
    
    Editor tempEditor;

    public void Init()
    {
        guiLayout.Add(GUILayout.Width(500));
        guiLayout.Add(GUILayout.Height(500));

        but = HS_Button.Create("Butten", null);
        label = HS_Label.Create("Lable");
        space = HS_Space.Create(50);
        popup = HS_Popup<HS_Text>.Create("Popup");
        popup.Add(HS_Text.Create("HS_Text1", "郭晓波"));
        popup.Add(HS_Text.Create("HS_Text2", "郭晓波"));
        popup.Add(HS_Text.Create("HS_Text3", "郭晓波"));
        popup.Add(HS_Text.Create("HS_Text4", "郭晓波"));
        popup.Add(HS_Text.Create("HS_Text5", "郭晓波"));
        popup.Add(HS_Text.Create("HS_Text6", "郭晓波"));

        text = HS_Text.Create("HS_Text", "郭晓波");
        toggle = HS_Toggle.Create("HS_Toggle", false);

        area = HS_Area.Create("L", 500, 500,true);
        
        tempEditor = Editor.CreateEditor(Selection.objects[0]);
    }
    bool isFrist;
    public void OnGUI()
    {
        //GUILayout.Box("", guiLayout.ToArray());
        //GUILayout.BeginArea(new Rect(5, 5, 500, 500));
        area.OnGUIUpdate();
        but.OnGUIUpdate();
        label.OnGUIUpdate();
        space.OnGUIUpdate();
        but.OnGUIUpdate();
        label.OnGUIUpdate();
        popup.OnGUIUpdate();
        text.OnGUIUpdate();
        text.OnGUIUpdate();
        toggle.OnGUIUpdate();
        toggle.OnGUIUpdate();
        toggle.OnGUIUpdate();
        toggle.OnGUIUpdate();
        //GUILayout.EndArea();
        area.EndArea();


        //tempEditor.DrawDefaultInspector();
        //tempEditor.DrawPreview(new Rect(0, 600, 800, 200));
        
        if(tempEditor != null)
        {
            //tempEditor.OnInspectorGUI();
            //tempEditor.DrawPreview(new Rect(0,700, 800, 200));
            tempEditor.OnInteractivePreviewGUI(new Rect(0, 700, 800, 200), EditorStyles.whiteLabel);
        }
        
        //D.LogWarning(tempEditor.target.GetType().ToString());
        //         GameObject hero = tempEditor.target as GameObject;
        //         Animation ani = hero.GetComponent<Animation>();
        //         
        if (!isFrist)
        {
            isFrist = true;
            //tempEditor.OnInteractivePreviewGUI(new Rect(0, 700, 800, 200), EditorStyles.whiteLabel);

        }

    }

    public void OnDestroy()
    {
        
    }

    public void OnFocus()
    {
        
    }

    public void OnHierarchyChange()
    {
        
    }

    public void OnInspectorUpdate()
    {
        
    }

    public void OnLostFocus()
    {
        
    }

    public void OnProjectChange()
    {
        
    }

    public void OnSelectionChange()
    {
        tempEditor.target = Selection.objects[0];
    }

    public void Update()
    {
        this.Repaint();
        Debug.DrawLine(Vector3.zero, new Vector3(1, 0, 0), Color.red);

    }
}
