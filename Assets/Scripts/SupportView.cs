using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SupportView : MonoBehaviour
{
    public void OpenOetgDiscord()
    {
        Application.OpenURL("https://discord.gg/6GG9j4xzek");
    }

    public void OpenSparkDevDiscord()
    {
        Application.OpenURL("https://discord.gg/XQs6Muman3");
    }

    public GameObject webObject;
    public Image webImage;
    public TextMeshProUGUI webText;
    public void EmailSparky()
    {
        //email Id to send the mail to
        string email = "sparklmonkeygames@gmail.com";
        //subject of the mail
        string subject = MyEscapeURL("Feedback & Suggestions");
        //body of the mail which consists of Device Model and its Operating System
        
        string body = MyEscapeURL("Please Enter your message here\n\n\n\n");
           
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

    }

    private void Start()
    {
#if PLATFORM_WEBGL
        webText.text = "A Coffee?";
#else
        webText.text = "Play on Web";
#endif
    }

    public void OpenWebSite()
    {
#if PLATFORM_WEBGL
        Application.OpenURL("https://www.buymeacoffee.com/sparklmonkey");
#else
        Application.OpenURL("https://elementrevival.sparklmonkeygames.com/");
#endif
    }

    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}
