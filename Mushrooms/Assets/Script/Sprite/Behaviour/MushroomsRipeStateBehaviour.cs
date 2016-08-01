using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

public class MushroomsRipeStateBehaviour : MonoBehaviour {


    public enum RipeState
    {
        NULL,
        Ripe1,
        Ripe2,
        Ripe3,
    }

    private Mushrooms _Mushrooms;
    private StateMachine<RipeState> _MushroomsRipeState;

    public void Init(Mushrooms m)
    {
        this._Mushrooms = m;
        _MushroomsRipeState = StateMachine<RipeState>.Initialize(this, RipeState.NULL);
    }

    public void ChangeState(RipeState state)
    {
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

}
