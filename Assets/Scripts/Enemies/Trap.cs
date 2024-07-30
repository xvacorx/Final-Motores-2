using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Enemy
{
    private void Start()
    {
        damage = PlayerStats.Instance.health;
    }
}
