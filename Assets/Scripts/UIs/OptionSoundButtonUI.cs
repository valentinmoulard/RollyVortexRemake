using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSoundButtonUI : MonoBehaviour
{
    [SerializeField]
    private Image m_imageReference = null;
    [SerializeField, Tooltip("Value of the albedo of the image when the corresponding option is disabled.")]
    private float m_imageOpacityDisabledOption = 0.35f;

    private Color m_colorBuffer;

    private void OnEnable()
    {
        OptionsManager.OnToggleSoundOption += UpdateImageOpacity;
        m_colorBuffer = m_imageReference.color;
    }

    private void OnDisable()
    {
        OptionsManager.OnToggleSoundOption -= UpdateImageOpacity;
    }

    private void Start()
    {
        if (m_imageReference == null)
            Debug.LogError("The image reference is missing!", gameObject);

        
    }

    private void UpdateImageOpacity(bool isOptionActive)
    {
        if (isOptionActive)
        {
            m_colorBuffer.a = 1f;
            m_imageReference.color = m_colorBuffer;
        }
        else
        {
            m_colorBuffer.a = m_imageOpacityDisabledOption;
            m_imageReference.color = m_colorBuffer;
        }
    }
}
