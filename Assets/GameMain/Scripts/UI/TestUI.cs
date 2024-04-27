using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LGF.UI;
using UnityEngine;

public class TestUI : IUIRoot
{
    protected override void OnInit(string uiName, int uiDepth, object userData)
    {
        Debug.Log("TestUI OnInit");
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        Debug.Log("TestUI OnAwake");
    }

    private void Start()
    {
        Debug.Log("TestUI Start");
    }

    protected override void OnRecycle()
    {
    }

    public override void OnClose()
    {
    }

    protected override void DepthChange(int uiDepth)
    {
    }

    public override void OnDestroy()
    {
    }
}