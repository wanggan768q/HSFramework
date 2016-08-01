using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class MushroomsWitherStateBehaviour : MonoBehaviour
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
    }

    public void ChangeState(WitherState state)
    {
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
