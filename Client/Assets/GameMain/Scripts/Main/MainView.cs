using LGF.MVC;
using LGF.Path;
using UnityEngine;
using UnityEngine.UI;

public class MainView : LGFView
{
    public Image UI_Img_Test;

    protected override void OnInit(string uiName, int uiDepth, object userData)
    {
        foreach (var info in (int[])userData)
        {
            Debug.Log(info);
        }

        // UI_Img_Test = transform.Find("TImg").GetComponent<Image>();
        if (UI_Img_Test != null)
        {
            // UI_Img_Test.sprite = Game.ResManager.LoadSprite("Main", "Img_01");
        }
    }

    public override void OnClose()
    {
    }

    public override void OnDestroy()
    {
    }
}