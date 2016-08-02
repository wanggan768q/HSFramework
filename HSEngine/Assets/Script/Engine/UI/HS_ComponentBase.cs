using UnityEngine;
using System.Collections;
using HS.Base;

namespace HS.UI
{
    public abstract class HS_ComponentBase : MonoBehaviour
    {
        protected HS_Scheduler.Proxy mScheduler;

        public HS_Scheduler.Proxy scheduler
        {
            get
            {
                if (mScheduler == null)
                {
                    mScheduler = new HS_Scheduler.Proxy();
                }
                return mScheduler;
            }
        }
    }

}
