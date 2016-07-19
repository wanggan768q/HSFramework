using UnityEngine;
using System.Collections;


namespace HS.UI
{

    public interface IForwardModalles : IForward
    {

        void MaskClickHandle();
        void CustomMask(Transform mask);
    }
}

