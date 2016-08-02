using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;
using HS.UI;
using HS.Manager;

public class MushroomsRipeStateBehaviour : HS_ComponentBase
{
    private const int C_RIPE_STAGE = 4;

    public enum RipeState
    {
        NULL,
        Ripe1,
        Ripe2,
        Ripe3,
        Ripe4,
    }

    private Mushrooms _Mushrooms;
    private StateMachine<RipeState> _MushroomsRipeState;

    public void Init(Mushrooms m)
    {
        this._Mushrooms = m;
        _MushroomsRipeState = StateMachine<RipeState>.Initialize(this, RipeState.NULL);

        int stage = 1;
        scheduler.Timeout(() =>
        {
            ChangeState(RipeState.Ripe1);
        }, m.SurplusTime / stage++);

        scheduler.Timeout(() =>
        {
            ChangeState(RipeState.Ripe2);
        }, m.SurplusTime / stage++);

        scheduler.Timeout(() =>
        {
            ChangeState(RipeState.Ripe3);
        }, m.SurplusTime / stage++);

        scheduler.Timeout(() =>
        {
            ChangeState(RipeState.Ripe4);
        }, m.SurplusTime / stage++);
    }

    public void ChangeState(RipeState state)
    {
        if (HS_ViewManager.Get<UIHomeView>().CurrentHumidity == 0f)
        {
            D.Log("湿度为 0");
            return;
        }
        _MushroomsRipeState.ChangeState(state);
    }

    public RipeState GetCurState()
    {
        return _MushroomsRipeState.State;
    }


    #region Ripe1
    private void Ripe1_Enter()
    {
    }

    private void Ripe1_Update()
    {

    }

    private void Ripe1_Exit()
    {
    }
    #endregion


    #region Ripe2
    private void Ripe2_Enter()
    {
    }

    private void Ripe2_Update()
    {

    }

    private void Ripe2_Exit()
    {
    }
    #endregion

    #region Ripe3
    private void Ripe3_Enter()
    {
    }

    private void Ripe3_Update()
    {

    }

    private void Ripe3_Exit()
    {
    }
    #endregion

    #region Ripe4
    private void Ripe4_Enter()
    {
    }

    private void Ripe4_Update()
    {

    }

    private void Ripe4_Exit()
    {
    }
    #endregion

}
