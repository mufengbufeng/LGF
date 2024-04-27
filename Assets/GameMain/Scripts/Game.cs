using System;
using System.Threading.Tasks;
using LGF.Event;
using LGF.Path;
using LGF.Res;
using LGF.UI;
using Unity.VisualScripting;
using UnityEngine;
using YooAsset;

public class Game : MonoBehaviour
{
    public static EventManager eventManager;
    public static ResManager resManager;
    public static UIManager uiManager;

    private void Awake()
    {
        eventManager = this.AddComponent<EventManager>();
        resManager = this.AddComponent<ResManager>();
        uiManager = this.AddComponent<UIManager>();
    }

    void Start()
    {
        eventManager.Trigger("InitFinish");

        eventManager.Add("YooAssetInitialized", (message) => { });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Load();
        }
    }

    private void Load()
    {
        // string path = PathConfig.GetUIPrefabPath("RootCube");
        // AssetInfo assetInfo = resManager.GetAssetInfo(path);
        // GameObject go = resManager.LoadGameObject(path, "", transform);
        // Task<GameObject> go2 = resManager.LoadGameObjectAsync(path, "", transform);
        // go2.ContinueWith(t => { Debug.Log("AAAAAAAAAAAAAAAA"); });
        uiManager.Open<TestUI>();
        Debug.Log("aaaa");
    }
}