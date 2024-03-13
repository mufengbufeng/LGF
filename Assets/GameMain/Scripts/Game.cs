using LGF.Event;
using LGF.Res;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static EventManager eventManager;
    public static ResManager resourceManager;

    private void Awake()
    {
    
        eventManager = this.AddComponent<EventManager>();
        resourceManager = this.AddComponent<ResManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        eventManager.Trigger("InitFinish");
    }
}