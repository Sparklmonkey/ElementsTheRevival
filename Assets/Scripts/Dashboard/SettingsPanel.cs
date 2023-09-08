using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    private Option_Toggle quickPlay, animSpeed;
    [SerializeField]
    private Slider bgmVolumeSlider, sfxVolumeSlider;
    public void CloseSettingsPanel()
    {
        PlayerPrefs.SetInt("QuickPlay", quickPlay.currentToggle);
        if(animSpeed.currentToggle == 2)
        {
            PlayerPrefs.SetFloat("AnimSpeed", 0);
        }
        else if(animSpeed.currentToggle == 1)
        {
            PlayerPrefs.SetFloat("AnimSpeed", 0.01f);
        }
        else
        {
            PlayerPrefs.SetFloat("AnimSpeed", 0.05f);
        }
        PlayerPrefs.SetFloat("BGMVolume", bgmVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        SoundManager.Instance.UpdateVolume();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        bgmVolumeSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        quickPlay.SetUpToggle(PlayerPrefs.GetInt("QuickPlay"));
        float animSpeedInt = PlayerPrefs.GetFloat("AnimSpeed");
        if (animSpeedInt == 0)
        {
            animSpeed.SetUpToggle(2);
        }
        else if (animSpeedInt == 0.01f)
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
