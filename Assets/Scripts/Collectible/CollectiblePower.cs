using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePower : Collectible
{
    public static Action OnCollectiblePowerCollected;

    public override void TriggerCollectibleEffect()
    {
        base.TriggerCollectibleEffect();
        BroadcastOnCollectiblePowerCollected();
    }

    private void BroadcastOnCollectiblePowerCollected()
    {
        if (OnCollectiblePowerCollected != null)
            OnCollectiblePowerCollected();
    }
}
