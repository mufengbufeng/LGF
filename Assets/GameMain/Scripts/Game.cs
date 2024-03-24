using System.Threading.Tasks;
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

    void Start()
    {
        eventManager.Trigger("InitFinish");

        eventManager.Add("YooAssetInitialized", (message) =>
        {
            string path = PathConfig.GetUIPrefabPath("RootCube");

            AssetInfo assetInfo = resManager.GetAssetInfo(path);
            // GameObject go = resManager.LoadGameObject(path, "", transform);
            Task<GameObject> go2 = resManager.LoadGameObjectAsync(path, "", transform);
            go2.ContinueWith(t => { Debug.Log("AAAAAAAAAAAAAAAA"); });

            Debug.Log("aaaa");
            // Debug.Log(assetInfo.PackageName);
        });
    }
}