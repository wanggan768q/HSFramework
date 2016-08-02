using UnityEngine;
using System.Collections;
using HS.Manager;
using HS.Base;


public class SpriteManager : HS_Singleton<SpriteManager>
{

    HS_ObjectPool<Mushrooms> _MushroomsPool = new HS_ObjectPool<Mushrooms>();

    private void Test()
    {
        Mushrooms m = _MushroomsPool.New();
    }

    public void Create(MushroomsCfgElement e)
    {
        Mushrooms m =_MushroomsPool.New();
        m.Init(e);
    }
}
