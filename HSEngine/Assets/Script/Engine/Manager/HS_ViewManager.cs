using UnityEngine;
using System.Collections;
using HS.UI;
using System.Collections.Generic;
using UnityEngine.UI;

namespace HS.Manager
{
public static class HS_ViewManager
{


    class View
    {
        public HS_ViewBase view;

        public View(HS_ViewBase _view)
        {
            this.view = _view;
        }
    }

    private static GameObject mRoot;
    private static Canvas mRootCanavas;
    private static List<View> mViews = new List<View>();
    //private static int mMaxOrder = 0;

    private static GameObject mTagNormal;
    private static GameObject mTagForward;
    private static GameObject mTagMask;
    private static Camera mUICamera;

    private static List<View> mBackwardsQueue = new List<View>();

    public delegate void OnViewChange();

    public static HS_ViewManager.OnViewChange onViewChange;

    private static Color defaultMaskColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);

    static HS_ViewManager()
    {
        if (root == null)
        {
            GameObject r = GameObject.Find("UI");
            if (r != null)
            {
                mTagNormal = r.transform.Find("Normal").gameObject;
                mTagForward = r.transform.Find("Forward").gameObject;
                mTagMask = mTagForward.transform.Find("Mask").gameObject;
                mTagMask.transform.localScale = Vector3.zero;
                mUICamera = r.transform.Find("UICamera").GetComponent<Camera>();
                mRoot = r;
                mRootCanavas = root.GetComponent<Canvas>();

                FormatRectTransform(mTagNormal.GetComponent<RectTransform>());
                FormatRectTransform(mTagForward.GetComponent<RectTransform>());
                //FormatRectTransform(mTagMask.GetComponent<RectTransform>());
            }
        }
    }

    static void FormatRectTransform(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = Vector2.one / 2;
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;
        rect.anchoredPosition3D = Vector3.zero;
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }

    static public void Init()
    {

    }

    public static Vector3 ConvertWorldPosition2ScreenPosition(Vector3 worldPos)
    {
        if (Camera.main == null)
        {
            return Vector3.zero;
        }
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(worldPos);
        Vector3 screenPos = HS_ViewManager.uiCamera.ViewportToScreenPoint(viewPortPos);
        screenPos.Set(screenPos.x / HS_ViewManager.rootCanvas.scaleFactor, screenPos.y / HS_ViewManager.rootCanvas.scaleFactor, screenPos.z);
        return screenPos;
    }

    public static GameObject root
    {
        get
        {
            return mRoot;
        }
        set
        {
            if (mRoot == value)
                return;
            mRoot = value;
        }
    }

    public static int ScreenWidth
    {
        get
        {
            return (int)(uiCamera.pixelWidth / rootCanvas.scaleFactor);
        }
    }

    public static int ScreenHeight
    {
        get
        {
            return (int)(uiCamera.pixelHeight / rootCanvas.scaleFactor);
        }
    }

    public static Canvas rootCanvas
    {
        get
        {
            return mRootCanavas;
        }
        set
        {
            if (mRootCanavas == value)
                return;
            mRootCanavas = value;
        }
    }

    public static Camera uiCamera
    {
        get
        {
            return mUICamera;
        }
    }

    public static GameObject UIAddChild(GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        RectTransform t = go.GetComponent<RectTransform>();
        if (t == null)
        {
            Debug.LogError("Add a wrong prefab,not a UGUI prefab.");
            return null;
        }
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif

        if (go != null && parent != null)
        {
            t.SetParent(parent.transform, false);
            RectTransform prefabRect = prefab.GetComponent<RectTransform>();
            CopyRectTransform(prefabRect, t);
            go.layer = parent.layer;
        }
        return go;
    }

    public static void CopyRectTransform(RectTransform source, RectTransform target)
    {
        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.pivot = source.pivot;
        target.localScale = source.localScale;
        target.localRotation = source.localRotation;
        target.anchoredPosition3D = source.anchoredPosition3D;
        target.anchoredPosition = source.anchoredPosition;
        target.sizeDelta = source.sizeDelta;
    }

    private static int mMaskHolder = 0;

    public static T Open<T>() where T : HS_ViewBase
    {
        if (mRoot == null)
        {
            Debug.LogError("Have No UI root gameObject.");
            return null;
        }


        View view;
        int index;
        T t = Get<T>(out view, out index);
        if (t == null)
        {
            T temp = mRoot.AddComponent<T>();
            GameObject prefab = temp.GetViewPrefab();
            GameObject go = UIAddChild((temp is IForward) ? mTagForward : mTagNormal, prefab);
            GameObject.Destroy(temp);

            t = go.AddComponent<T>();
            view = new View(t);
            mViews.Add(view);
            HS_ViewBase.InternalOpened(view.view);
        }
        GameObject rootCanvas = (t is IForward) ? mTagForward : mTagNormal;
        view.view.transform.SetSiblingIndex(rootCanvas.transform.childCount - 1);

        if (t is IForwardModalles)
        {
            mMaskHolder++;

            mTagMask.transform.SetSiblingIndex(view.view.transform.GetSiblingIndex() - 1);
            mTagMask.GetComponent<Image>().color = defaultMaskColor;
            mTagMask.transform.localScale = Vector3.one;
            (t as IForwardModalles).CustomMask(mTagMask.transform);

            mTagMask.GetComponent<Button>().onClick.AddListener(delegate {
                (t as IForwardModalles).MaskClickHandle();
            });
        }

        return t;
    }

    public static void DisableMask()
    {
        mTagMask.transform.localScale = Vector3.zero;
    }

    public static void BringToTop(HS_ViewBase view)
    {
        GameObject rootCanvas = (view is IForward) ? mTagForward : mTagNormal;
        view.transform.SetSiblingIndex(rootCanvas.transform.childCount - 1);
    }
    public static void BringToDown(HS_ViewBase view)
    {
        //GameObject rootCanvas = (view is IForward) ? mTagForward : mTagNormal;
        view.transform.SetSiblingIndex(0);
    }

    public static Color getDefaultMaskColor()
    {
        return defaultMaskColor;
    }
    public static void Close<T>(System.Action closedCallBack = null, bool showDisappearEffect = true) where T : HS_ViewBase
        {
        View view;
        int index;
        T t = Get<T>(out view, out index);
        if (t != null)
        {
            Close(view, index);
        }
    }

    public static void Close(HS_ViewBase view)
    {
        if (view == null)
            return;
        for (int i = mViews.Count - 1; i >= 0; i--)
        {
            View obj = mViews[i];
            if (obj.view == view)
            {
                Close(obj, i);
                break;
            }
        }
    }

    public static void CloseAll()
    {
        while (mViews.Count > 0)
        {
            int index = mViews.Count - 1;
            View view = mViews[index];
            Close(view, index);

        }
    }

    private static void Close(View view, int index)
    {
        System.Action closeAction = delegate {

            mViews.RemoveAt(index);
            HS_ViewBase.InternalClosed(view.view);
            GameObject.Destroy(view.view.gameObject);

            if (view.view is IForwardModalles)
            {
                bool found = false;
                for (int i = mViews.Count - 1; i >= 0; i--)
                {
                    if (mViews[i].view is IForwardModalles)
                    {
                        mTagMask.transform.SetAsLastSibling();
                        mViews[i].view.transform.SetAsLastSibling();
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    mTagMask.transform.localScale = Vector3.zero;
                }
            }


        };
        closeAction();
    }

    /*public static U Replace<T, U>()
    where T : ViewBase
        where U : ViewBase
{
    Close<T>();
    return (U)(Open<U>());
}*/

    public static void Replace<T, U>()where T : HS_ViewBase where U : HS_ViewBase
        {
        Close<T>(delegate {
            Open<U>();
        });
    }

    public static void Toggle<T>()
  where T : HS_ViewBase
        {
        if (Exist<T>())
        {
            Close<T>();
        }
        else
        {
            Open<T>();
        }
    }

    public static bool Exist<T>() where T : HS_ViewBase
        {
        View view;
        int index;
        return Get<T>(out view, out index) != null;
    }

    public static T Get<T>() where T : HS_ViewBase
        {
        View view;
        int index;
        return Get<T>(out view, out index);
    }

    private static T Get<T>(out View view, out int index) where T : HS_ViewBase
        {
        T t = null;
        view = null;
        index = 0;
        System.Type type = typeof(T);
        for (int i = mViews.Count - 1; i >= 0; i--)
        {
            View obj = mViews[i];
            if (obj.view.GetType() == type)
            {
                view = obj;
                index = i;
                t = (T)obj.view;
                break;
            }
        }
        return t;
    }



}

}
