using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    private OptionToggle quickPlay, animSpeed;
    [SerializeField]
    private Slider bgmVolumeSlider, sfxVolumeSlider;
    [SerializeField]
    private Button closeButton;
    public void CloseSettingsPanel()
    {
        PlayerPrefs.SetInt("QuickPlay", quickPlay.currentToggle);
        if (animSpeed.currentToggle == 2)
        {
            PlayerPrefs.SetFloat("AnimSpeed", 0);
        }
        else if (animSpeed.currentToggle == 1)
        {
            PlayerPrefs.SetFloat("AnimSpeed", 0.5f);
        }
        else
        {
            PlayerPrefs.SetFloat("AnimSpeed", 1f);
        }
        PlayerPrefs.SetFloat("BGMVolume", bgmVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        SoundManager.Instance.UpdateVolume();
    }

    public void SetCloseAction(ButtonActionNoParams closeButtonDelegate)
    {
        closeButton.onClick.AddListener(delegate { closeButtonDelegate(); });
    }
    private void Awake()
    {
        bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        quickPlay.SetUpToggle(PlayerPrefs.GetInt("QuickPlay"));
        var animSpeedInt = PlayerPrefs.GetFloat("AnimSpeed");
        if (animSpeedInt == 0)
        {
            animSpeed.SetUpToggle(2);
        }
        else if (animSpeedInt == 0.5f)
        {
            animSpeed.SetUpToggle(1);
        }
        else
        {
            animSpeed.SetUpToggle(0);
        }
    }

    public void UpdateVolumeSlider()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        SoundManager.Instance.UpdateVolume();
    }
}
}