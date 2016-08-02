using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using HS.UI;
using HS.Manager;

public class MushroomsWitherStateBehaviour : HS_ComponentBase
{
    public enum WitherState
    {
        NULL,
        Wither1,
        Wither2,
        Wither3,
    }

    private Mushrooms _Mushrooms;
    private StateMachine<WitherState> _MushroomsWitherState;

    public void Init(Mushrooms m)
    {
        this._Mushrooms = m;
        _MushroomsWitherState = StateMachine<WitherState>.Initialize(this, WitherState.NULL);

        int stage = 1;
        scheduler.Timeout(() =>
        {
            ChangeState(WitherState.Wither1);
        }, m.SurplusTime / stage++);

        scheduler.Timeout(() =>
        {
            ChangeState(WitherState.Wither2);
        }, m.SurplusTime / stage++);

        scheduler.Timeout(() =>
        {
            ChangeState(WitherState.Wither3);
        }, m.SurplusTime / stage++);

    }

    public void ChangeState(WitherState state)
    {
        if (HS_ViewManager.Get<UIHomeView>().CurrentHumidity > 0f)
        {
            D.Log("湿度为 0");
            return;
        }
        _MushroomsWitherState.ChangeState(state);
    }

    public WitherState GetCurState()
    {
        return _MushroomsWitherState.State;
    }


    #region Wither1
    private void Wither1_Enter()
    {
    }

    private void Wither1_Update()
    {
        
    }

    private void Wither1_Exit()
    {
    }
    #endregion


    #region Wither2
    private void Wither2_Enter()
    {
    }

    private void Wither2_Update()
    {

    }

    private void Wither2_Exit()
    {
    }
    #endregion

    #region Wither3
    private void Wither3_Enter()
    {
    }

    private void Wither3_Update()
    {

    }

    private void Wither3_Exit()
    {
    }
    #endregion





}
