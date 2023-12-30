using System;

namespace Plan9Client
{
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