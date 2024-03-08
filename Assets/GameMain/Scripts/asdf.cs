using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Game.eventManager.Add("KeyDownK", (message) =>
        {
            Debug.Log("K is down");
            Debug.Log($"message[0]: {message[0]}, message[1]: {message[1]}, message[2]: {message[2]}");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Game.eventManager.Trigger("KeyDownK", 1, "2", false);
        }
    }
}