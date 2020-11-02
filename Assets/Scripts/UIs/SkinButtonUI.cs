using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButtonUI : MonoBehaviour
{
    public static Action<Material> OnSkinSelected;

    [SerializeField]
    private Material m_skinMaterial = null;
    [SerializeField]
    private Image m_skinImage = null;

    private void Start()
    {
        if (m_skinMaterial == null)
            Debug.LogError("The material component is missing!", gameObject);
        if (m_skinImage == null)
            Debug.LogError("The image component is missing!", gameObject);

        m_skinImage.color = m_skinMaterial.GetColor("_EmissionColor");
    }


    //called by button to select a skin
    public void SelectSkin()
    {
        BroadcastOnSkinSelected();
    }

    private void BroadcastOnSkinSelected()
    {
        if (OnSkinSelected != null)
            OnSkinSelected(m_skinMaterial);
    }
}
