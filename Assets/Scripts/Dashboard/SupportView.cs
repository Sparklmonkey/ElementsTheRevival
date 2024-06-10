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
        var email = "sparklmonkeygames@gmail.com";
        //subject of the mail
        var subject = MyEscapeURL("Feedback & Suggestions");
        //body of the mail which consists of Device Model and its Operating System

        var body = MyEscapeURL("Please Enter your message here\n\n\n\n");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

    }

    public void OpenWebSite()
    {
        Application.OpenURL("https://www.twitch.tv/sparklmonkey");
    }

    private string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}
