using System;
using System.Collections;
using System.Collections.Generic;
using LGF.Event;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static EventManager eventManager;


    private void Awake()
    {
        eventManager = this.AddComponent<EventManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        eventManager.Trigger("InitFinish");
    }
}