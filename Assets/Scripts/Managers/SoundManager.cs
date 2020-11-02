using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource m_SFXAudioSourceReference = null;

    [SerializeField]
    private AudioSource m_BGMAudioSourceReference = null;

    [Header("Sounds List")]
    [SerializeField]
    private AudioClip m_buttonPressSound = null;

    [SerializeField]
    private AudioClip m_brickExplosionSound = null;

    [SerializeField]
    private AudioClip m_pickUpSound = null;

    [SerializeField]
    private AudioClip m_deathSound = null;

    private bool m_isSoundOn;

    private void OnEnable()
    {
        PlayerController.OnPowerActivated += PlayBrickExplosionSound;
        CollectibleCurrency.OnCollectibleCurrencyCollected += PlayPickUpSound;
        PlayerController.OnPlayerDead += PlayDeathSound;

        OptionsManager.OnToggleSoundOption += SetSoundState;
    }

    private void OnDisable()
    {
        PlayerController.OnPowerActivated -= PlayBrickExplosionSound;
        CollectibleCurrency.OnCollectibleCurrencyCollected -= PlayPickUpSound;
        PlayerController.OnPlayerDead -= PlayDeathSound;

        OptionsManager.OnToggleSoundOption -= SetSoundState;
    }

    private void Start()
    {
        if (m_BGMAudioSourceReference == null)
            Debug.LogError("The BGM audio source component is missing!", gameObject);
        if (m_SFXAudioSourceReference == null)
            Debug.LogError("The SFX audio source component is missing!", gameObject);

        if (m_buttonPressSound == null)
            Debug.LogError("The button pressed sound is missing!", gameObject);
        if (m_brickExplosionSound == null)
            Debug.LogError("The brick explosion sound is missing!", gameObject);
        if (m_pickUpSound == null)
            Debug.LogError("The pick up sound is missing!", gameObject);
        if (m_deathSound == null)
            Debug.LogError("The death sound is missing!", gameObject);

    }

    private void SetSoundState(bool isSoundOn)
    {
        m_isSoundOn = isSoundOn;

        if (!m_isSoundOn)
        {
            m_SFXAudioSourceReference.Stop();
            m_BGMAudioSourceReference.Stop();
        }
        else if(!m_BGMAudioSourceReference.isPlaying)
        {
            m_BGMAudioSourceReference.Play();
        }

    }

    private void PlaySound(AudioClip soundToPlay)
    {
        if (m_isSoundOn)
        {
            m_SFXAudioSourceReference.clip = soundToPlay;
            m_SFXAudioSourceReference.Play();
        }
    }

    //called by button
    public void PlayOnButtonPressedSound()
    {
        PlaySound(m_buttonPressSound);
    }

    private void PlayBrickExplosionSound()
    {
        PlaySound(m_brickExplosionSound);
    }

    private void PlayPickUpSound(int pickUpValue)
    {
        PlaySound(m_pickUpSound);
    }

    private void PlayDeathSound()
    {
        PlaySound(m_deathSound);
    }
}
