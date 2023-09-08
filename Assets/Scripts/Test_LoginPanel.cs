//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class Test_LoginPanel : MonoBehaviour
//{
//    [SerializeField]
//    private TMP_InputField userInput, pwdInput;
//    [SerializeField]
//    private PvP_LobbyConnecteduser pvp_LobbyConnecteduser;

//    public void Login()
//    {
//        LoginRequest request = new LoginRequest
//        {
//            Username = userInput.text,
//            Password = pwdInput.text
//        };
//        StartCoroutine(ApiManager.Instance.Login(request, LoginSuccess, LoginFailure));
//    }

//    public void LoginSuccess(LoginResponse loginResponse)
//    {
//        ApiManager.Instance.SetToken(loginResponse.token);
//        ApiManager.Instance.SetUsernameAndEmail(userInput.text, loginResponse.emailAddress);
//        PlayerData.shared = loginResponse.playerData;
//        Game_PvpHubConnection.shared.StartPvpHubConnection();
//        StartCoroutine(ApiManager.Instance.RefreshToken());
//        //gameObject.SetActive(false);
//    }

//    public void LoginFailure(LoginResponse loginResponse)
//    {

//    }

//    public void FindOpponent()
//    {
//        Game_PvpHubConnection.shared.ConnectToPvpRoom();
//    }
//    public void ConfirmOpponent()
//    {
//        Game_PvpHubConnection.shared.ConfirmOpponent();
//    }
//}
