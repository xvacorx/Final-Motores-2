using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temporaryswitch : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.TriggerSwitch();
        }
    }
}
