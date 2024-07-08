using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    public override void Collect()
    {
        EventManager.CoinCollect();
        ActivateCollectedEffect();
        Destroy(gameObject);
    }
}