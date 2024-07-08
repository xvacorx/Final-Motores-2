using System;
using UnityEngine;

public static class EventManager
{
    public static event Action OnSwitch;
    private static float switchCooldown = 1.0f;
    private static float lastSwitchTime = 0f;

    public static event Action CollectedCoin;
    public static void CoinCollect()
    {
        CollectedCoin?.Invoke();
    }
    public static void TriggerSwitch()
    {
        if (Time.time >= lastSwitchTime + switchCooldown)
        {
            OnSwitch?.Invoke();
            lastSwitchTime = Time.time;
        }
    }
}