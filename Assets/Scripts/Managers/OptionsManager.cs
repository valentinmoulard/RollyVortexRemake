using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static Action<bool> OnToggleSoundOption;
    public static Action<bool> OnToggleVibrationOption;

    private bool m_isVibrationOn;
    private bool m_isSoundOn;

    private void OnEnable()
    {
        PlayerController.OnPowerActivated += Vibrate;
        UIManager.OnOptionUI += BroadcastOptionsParameters;
    }

    private void OnDisable()
    {
        PlayerController.OnPowerActivated -= Vibrate;
        UIManager.OnOptionUI -= BroadcastOptionsParameters;
    }

    private void Start()
    {
        FetchOptionsFromSave();
    }

    /// <summary>
    /// Retreive the options parameters from a save file. (Currently using player prefs as there is currently no save system).
    /// </summary>
    private void FetchOptionsFromSave()
    {
        m_isVibrationOn = PlayerPrefs.GetInt("Vibration", 1) == 1;
        m_isSoundOn = PlayerPrefs.GetInt("SoundOption", 1) == 1;
        BroadcastOptionsParameters();
    }

    private void BroadcastOptionsParameters()
    {
        BroadcastOnToggleSoundOption();
        BroadcastOnToggleVibrationOption();
    }

    private void Vibrate()
    {
        if (m_isVibrationOn)
            Vibration.Vibrate(500);
    }

    public void SwithVibration()
    {
        m_isVibrationOn = !m_isVibrationOn;
        PlayerPrefs.SetInt("Vibration", m_isVibrationOn ? 1 : 0);
        BroadcastOnToggleVibrationOption();
    }

    public void SwitchSound()
    {
        m_isSoundOn = !m_isSoundOn;
        PlayerPrefs.SetInt("SoundOption", m_isSoundOn ? 1 : 0);
        BroadcastOnToggleSoundOption();
    }

    private void BroadcastOnToggleSoundOption()
    {
        if (OnToggleSoundOption != null)
            OnToggleSoundOption(m_isSoundOn);
    }

    private void BroadcastOnToggleVibrationOption()
    {
        if (OnToggleVibrationOption != null)
            OnToggleVibrationOption(m_isVibrationOn);
    }
}
