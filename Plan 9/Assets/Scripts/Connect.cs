using Cysharp.Threading.Tasks;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Plan9Client
{
    public class Connect : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _errorText;

        [SerializeField]
        private TMP_InputField _login;

        [SerializeField]
        private TMP_InputField _password;

        private const string SERVER_ADDRESS = "https://stage.arenagames.api.ldtc.space/api/v3/gamedev/client/auth/sign-in";

        public async void OnConnectClicked()
        {
            UserInfo userInfo = new UserInfo()
            {
                login = _login.text,
                password = _password.text
            };

            string jsonUserInfo = JsonUtility.ToJson(userInfo);
            UnityWebRequest request = UnityWebRequest.Post(SERVER_ADDRESS, new WWWForm());
            byte[] userInfoInBytes = Encoding.UTF8.GetBytes(jsonUserInfo);
            request.uploadHandler = new UploadHandlerRaw(userInfoInBytes);
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");

            try
            {
                await request.SendWebRequest();

                ServerSuccessResponse postFromServer = JsonUtility.FromJson<ServerSuccessResponse>(request.downloadHandler.text);
                Debug.LogError(postFromServer);
            }
            catch (UnityWebRequestException)
            {
                ServerFailureResponse postFromServer = JsonUtility.FromJson<ServerFailureResponse>(request.downloadHandler.text);
                _errorText.text = postFromServer.message;
            }
        }
    }
}