using LGF.Event;
using LGF.Path;
using LGF.Res;
using Unity.VisualScripting;
using UnityEngine;
using YooAsset;

public class Game : MonoBehaviour
{
    public static EventManager eventManager;
    public static ResManager resManager;

    private void Awake()
    {
        eventManager = this.AddComponent<EventManager>();
        resManager = this.AddComponent<ResManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        eventManager.Trigger("InitFinish");

        eventManager.Add("YooAssetInitialized", (message) =>
        {
            AssetInfo assetInfo = resManager.GetAssetInfo(PathConfig.GetUIPrefabPath("RootCube.prefab"));
            Debug.Log(assetInfo.PackageName);
        });
    }
}