using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static Action<Material> OnApplySkin;

    [SerializeField, Tooltip("Reference to the default material applied to the player.")]
    private Material m_defaultMaterial = null;

    private void OnEnable()
    {
        SkinButtonUI.OnSkinSelected += ApplySkin;
    }

    private void OnDisable()
    {
        SkinButtonUI.OnSkinSelected -= ApplySkin;
    }

    private void Start()
    {
        if (m_defaultMaterial == null)
            Debug.LogError("The default material is not referenced!", gameObject);

        ApplySkin(m_defaultMaterial);
    }

    private void ApplySkin(Material skinMaterial)
    {
        BroadcastOnSkinSelected(skinMaterial);
    }


    private void BroadcastOnSkinSelected(Material skinMaterial)
    {
        if (OnApplySkin != null)
            OnApplySkin(skinMaterial);
    }
}
