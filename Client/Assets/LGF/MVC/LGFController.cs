using LGF.Event;
using UnityEngine;

namespace LGF.MVC
{
    public class LGFController : Singleton<LGFController>
    {
        protected EventManager EventManager;

        public virtual void Init()
        {
            Debug.Log("LGFController Init");
            EventManager = EventManager.Instance;
        }

    }
}