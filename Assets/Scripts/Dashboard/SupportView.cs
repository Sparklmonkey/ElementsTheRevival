using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SupportView : MonoBehaviour
{

    public GameObject webObject;
    public Image webImage;
    public TextMeshProUGUI webText;
    public Sprite coffeeSprite, webSprite;
    private void Awake()
    {
#if Android
        webText.text = "Website";
        webImage.sprite = webSprite;
#else
        webText.text = "Ko-fi";
        webImage.sprite = coffeeSprite;
#endif
    }

    public void OpenOetgDiscord()
    {
        Application.OpenURL("https://discord.gg/6GG9j4xzek");
    }

    public void OpenSparkDevDiscord()
    {
        Application.OpenURL("https://discord.gg/XQs6Muman3");
    }
    public void EmailSparky()
    {
        //email Id to send the mail to
        var email = "sparklmonkeygames@gmail.com";
        //subject of the mail
        var subject = MyEscapeURL("Feedback & Suggestions");
        //body of the mail which consists of Device Model and its Operating System

        var body = MyEscapeURL("Please Enter your message here\n\n\n\n");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

    }

    public void OpenTwitchChannel()
    {
        Application.OpenURL("https://www.twitch.tv/sparklmonkey");
    }

    public void OpenKofiOrWeb()
    {
        #if Android
            Application.OpenURL("https://www.elementstherevival.com/");
        #else
            Application.OpenURL("https://ko-fi.com/sparklmonkey");
        #endif
    }

    private string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}
