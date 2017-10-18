using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

namespace HS.UI
{
    [RequireComponent(typeof(Image))]
    public class HS_RepeatButton : HS_ComponentBase, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        /// <summary>
        /// 相应事件
        /// </summary>
        public float Interval = 0.5f;
        /// <summary>
        /// 判定长按事件
        /// </summary>
        public float LongPressDelay = 0.5f;
        private bool _IsPointDown = false;
        private bool _IsLongpress = false;
        private bool _IsTriggerLongpress = false;
        private float _LastInvokeTime = 0;
        private float _TouchBegin = 0;

        public UnityAction<GameObject> OnClick = null;
        public UnityAction<GameObject> OnDown = null;
        public UnityAction<GameObject> OnUp = null;
        public UnityAction<GameObject> OnExit = null;
        public UnityAction<GameObject> OnLongpress = null;
        public UnityAction<GameObject> OnKeepLongpress = null;

        public Sprite sprite
        {
            get
            {
                Image img = gameObject.GetComponent<Image>();
                return img.sprite;
            }
            set
            {
                Image img = gameObject.GetComponent<Image>();
                img.sprite = value;
            }
        }

        public Image Image
        {
            get
            {
                return gameObject.GetComponent<Image>();
            }
        }


        void Update()
        {
            if (_IsPointDown)
            {
                if (_IsLongpress)
                {
                    if (Time.time - _LastInvokeTime > Interval)
                    {
                        if (OnLongpress != null)
                        {
                            if (!_IsTriggerLongpress)
                            {
                                _IsTriggerLongpress = true;
                                OnLongpress(gameObject);
                            }
                        }
                        if (OnKeepLongpress != null)
                        {
                            OnKeepLongpress(gameObject);
                        }
                        _LastInvokeTime = Time.time;
                    }
                }
                else
                {
                    _IsLongpress = Time.time - _TouchBegin > LongPressDelay;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _TouchBegin = Time.time;
            _IsPointDown = true;
            _IsTriggerLongpress = false;
            if (OnDown != null)
            {
                OnDown(gameObject);
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (OnClick != null && _IsLongpress == false)
            {
                OnClick(gameObject);
            }
            _IsPointDown = false;
            _IsLongpress = false;
            if (OnUp != null)
            {
                OnUp(gameObject);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnExit != null && _IsPointDown)
            {
                OnExit(gameObject);
            }
            _IsPointDown = false;
            _IsLongpress = false;
            _IsTriggerLongpress = false;
        }



    }
}