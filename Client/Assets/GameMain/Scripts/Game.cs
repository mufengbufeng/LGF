using JetBrains.Annotations;
using LGF.Data;
using LGF.Event;
using LGF.Res;
using LGF.UI;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static EventManager EventManager;
    public static ResManager ResManager;
    public static UIManager UIManager;
    private DataManager _dataManager;

    private void Awake()
    {
        EventManager = this.AddComponent<EventManager>();
        ResManager = this.AddComponent<ResManager>();
        UIManager = this.AddComponent<UIManager>();
        GameModule.InitController();
        EventManager.Add(GameEvent.YooAssetInitialized, (message) =>
        {
            _dataManager = this.AddComponent<DataManager>();
            Load();
        });
    }

    void Start()
    {
        // EventManager.Add(GameEvent.YooAssetInitialized, (message) => { Load(); });
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.J))
    //     {
    //         Load();
    //     }
    // }

    private void Load()
    {
        int[] a = new[] { 1, 1, 2, 2, 1 };
        // UIManager.Open<MainView>();
        UIManager.Open<MainView>(a);
    }
}