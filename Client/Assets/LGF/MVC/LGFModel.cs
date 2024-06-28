using LGF.Event;

namespace LGF.MVC
{
    public class LGFModel : MonoSingleton<LGFModel>
    {
        protected EventManager EventManager;

        protected override void Awake()
        {
            base.Awake();
            EventManager = Game.EventManager;
        }
    }
}