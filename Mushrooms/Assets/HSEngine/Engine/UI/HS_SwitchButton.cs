using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;

namespace HS.UI
{

    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(Image))]
    public class HS_SwitchButton : HS_ComponentBase
    {
        public Sprite OnSprite;
        public Sprite OffSprite;

        private Image _Image;
        private Toggle _Toggle;

        public HS_SwitchButtonGrop Grop;

        public bool IsOn
        {
            get
            {
                return _Toggle.isOn;
            }
            set
            {
                if (_Image == null)
                {
                    _Image = this.GetComponent<Image>();
                }
                if (_Toggle == null)
                {
                    _Toggle = this.GetComponent<Toggle>();
                }
                _Toggle.isOn = value;
                //Debug.LogError( "Set: " + transform.gameObject.name + ":" + _Toggle.isOn );
                //_Image.sprite = OffSprite;
            }
        }

        public UnityAction<GameObject, bool> OnClick = null;
        void Awake()
        {
            _Image = this.GetComponent<Image>();
            _Toggle = this.GetComponent<Toggle>();
            _Toggle.onValueChanged.AddListener(OnValueChanged);

            if (Grop != null)
            {
                Grop.RegisterUISwitchButton(this);
            }
            //Debug.LogError( "Init: " + transform.gameObject.name + ":" + _Toggle.isOn );
            OnValueChanged(_Toggle.isOn);
            //IsOn = false;
        }

        public void SetUISwitchButtonGrop(HS_SwitchButtonGrop grop)
        {
            this.Grop = grop;
            if (Grop != null)
            {
                Grop.RegisterUISwitchButton(this);
            }
            _Toggle.group = grop.GetComponent<ToggleGroup>();
            OnValueChanged(_Toggle.isOn);
        }


        private void OnValueChanged(bool b)
        {
            if (b)
            {
                _Image.sprite = OnSprite;
                if (Grop != null)
                {
                    Grop.NotifyUISwitchButtonOn(this);
                }
            }
            else
            {
                _Image.sprite = OffSprite;
            }
            _Image.SetNativeSize();

            if (OnClick != null)
            {
                OnClick(gameObject, b);
            }
        }
    }
}