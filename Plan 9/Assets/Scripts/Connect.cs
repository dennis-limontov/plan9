using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Connect : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _errorText;

    [SerializeField]
    private TMP_InputField _login;

    [SerializeField]
    private TMP_InputField _password;

    private string _serverAddress = "https://stage.arenagames.api.ldtc.space/api/v3/gamedev/client/auth/sign-in";

    private IEnumerator SendRequest()
    {
        WWWForm formData = new WWWForm();

        UserInfo userInfo = new UserInfo()
        {
            login = _login.text,
            password = _password.text
        };

        string jsonUserInfo = JsonUtility.ToJson(userInfo);

        UnityWebRequest request = UnityWebRequest.Post(_serverAddress, formData);

        byte[] userInfoInBytes = Encoding.UTF8.GetBytes(jsonUserInfo);

        request.uploadHandler = new UploadHandlerRaw(userInfoInBytes);

        request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            ServerSuccessResponse postFromServer = JsonUtility.FromJson<ServerSuccessResponse>(request.downloadHandler.text);
            Debug.Log(postFromServer);
        }
        else
        {
            ServerFailureResponse postFromServer = JsonUtility.FromJson<ServerFailureResponse>(request.downloadHandler.text);
            _errorText.text = postFromServer.message;
        }
        
        //Debug.Log(request.downloadHandler.text);
    }

    public void OnConnectClicked()
    {
        StartCoroutine(SendRequest());
    }

    [Serializable]
    public struct UserInfo
    {
        public string login;
        public string password;
    }

    [Serializable]
    public struct AccessToken
    {
        public string token;
        public long expiresIn;

        public override string ToString()
        {
            return $"token: {token}, expiresIn: {expiresIn}";
        }
    }

    [Serializable]
    public struct ServerSuccessResponse
    {
        public AccessToken accessToken;
        public AccessToken refreshToken;

        public override string ToString()
        {
            return $"{accessToken}\n{refreshToken}";
        }
    }

    [Serializable]
    public struct ServerFailureResponse
    {
        public string id;
        public string code;
        public string type;
        public string message;
    }
}
