using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCurrency : Collectible
{
    public static Action<int> OnCollectibleCurrencyCollected;

    [SerializeField, Tooltip("The value of currency to add when this collectible is picked up.")]
    private int m_collectibleCurrencyValue = 0;

    public override void TriggerCollectibleEffect()
    {
        base.TriggerCollectibleEffect();
        BroadcastOnCollectibleCurrencyCollected();
    }

    private void BroadcastOnCollectibleCurrencyCollected()
    {
        if (OnCollectibleCurrencyCollected != null)
            OnCollectibleCurrencyCollected(m_collectibleCurrencyValue);
    }

}
