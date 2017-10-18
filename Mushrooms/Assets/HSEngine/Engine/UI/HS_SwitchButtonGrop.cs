using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace HS.UI
{
[RequireComponent(typeof(ToggleGroup))]
public class HS_SwitchButtonGrop : HS_ComponentBase
{
    List<HS_SwitchButton> _UISwitchButtons = new List<HS_SwitchButton>();

    public bool allowSwitchOff { get; set; }

    public IEnumerable<HS_SwitchButton> ActiveUISwitchButtons()
    {
        return _UISwitchButtons;
    }

    public bool AnyUISwitchButtonOn()
    {
        bool r = false;
        _UISwitchButtons.ForEach( (HS_SwitchButton but ) =>
        {
            if (but.IsOn == true)
            {
                r = true;
            }
        } );
        return r;
    }

    public void NotifyUISwitchButtonOn(HS_SwitchButton but )
    {
        for (int i = 0; i < _UISwitchButtons.Count; ++i)
        {
            if (_UISwitchButtons[i] == but)
            {
                _UISwitchButtons[i].IsOn = true;
            }
            else
            {
                _UISwitchButtons[i].IsOn = false;
            }
        }
    }

    public void RegisterUISwitchButton(HS_SwitchButton but )
    {
        _UISwitchButtons.Add( but );
    }

    public void SetAllUISwitchButtonOff()
    {
        _UISwitchButtons.ForEach( (HS_SwitchButton but ) =>

        {
            but.IsOn = false;
        } );
    }

    public void UnregisterUISwitchButton(HS_SwitchButton but )
    {
        if (_UISwitchButtons.Contains( but ))
        {
            _UISwitchButtons.Remove( but );
        }
    }
}
}