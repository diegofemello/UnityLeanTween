using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

public class RequestManager : SingletonPersistent<RequestManager>
{
    private readonly string baseUrl = "https://ambevquiz.azurewebsites.net";
    public string bearer = null;

    private void OnEnable()
    {
        bearer = PlayerPrefs.GetString("accessToken");
    }


    // Método de login informando login e senha e retornando usuário
    public async Task<Auth> Login(string login, string password)
    {
        try
        {
            WWWForm form = new();
            form.AddField("login", login);
            form.AddField("password", password);

            string result = await Post("/Auth/Signin", form, false, true);
            if (string.IsNullOrEmpty(result)) return null;
            Auth responseData = JsonUtility.FromJson<Auth>(result);

            bearer = responseData.accessToken;
            PlayerPrefs.SetString("accessToken", responseData.accessToken);

            return responseData;
        }
        catch (Exception e)
        {
            LoadingManager.Instance.ShowError(e.Message);
        }

        return null;
    }

    // Retornando sessão do usuário
    public async Task<User> GetUser()
    {
        try
        {
            string result = await Get("/Auth/GetUser");
            if (string.IsNullOrEmpty(result)) return null;

            User responseData = JsonConvert.DeserializeObject<User>(result);

            return responseData;
        }
        catch (Exception e)
        {
            LoadingManager.Instance.ShowError(e.Message);
        }

        return null;
    }

    //// Obtém ranking dos usuário a partir do id da fábrica
    //public async Task<List<UserPoint>> GetRanking(int factoryId)
    //{
    //    try
    //    {
    //        string result = await Get($"/User/Ranking/{factoryId}");
    //        if (string.IsNullOrEmpty(result)) return null;

    //        List<UserPoint> responseData = JsonConvert.DeserializeObject<List<UserPoint>>(result);

    //        return responseData;
    //    }
    //    catch (Exception e)
    //    {
    //        LoadingManager.Instance.ShowError(e.Message);
    //    }

    //    return null;
    //}


    ////Obtém a lista de perguntas com uma quantidade definida e opcionamente a ilha.
    //public async Task<List<Question>> GetQuestions(int factoryId, int qtd = 0, EnumIsland? islandId = null)
    //{
    //    try
    //    {
    //        // Caso solicite pela ilha, está obtendo perguntas no modo aventura, se não modo desafio.
    //        bool isChallenge = islandId == null;

    //        string requestUrl = $"/Question?FactoryId={factoryId}&IsChallenge={(isChallenge ? "true" : "false")}";
    //        if (qtd != 0) requestUrl += $"&qtd={qtd}";
    //        if (!isChallenge) requestUrl += $"&IslandId={(int)islandId}";

    //        string result = await Get(requestUrl);
    //        if (string.IsNullOrEmpty(result)) return null;

    //        List<Question> responseData = JsonConvert.DeserializeObject<List<Question>>(result);
    //        return responseData;

    //    }
    //    catch (Exception e)
    //    {
    //        LoadingManager.Instance.ShowError(e.Message);
    //    }

    //    return null;
    //}

    //// Obtem o progresso do usuário em uma determinada fábrica
    //public async Task<Progress> GetProgress(int factoryId)
    //{
    //    try
    //    {
    //        string requestUrl = $"/Question/Progress/{factoryId}";
    //        string result = await Get(requestUrl);
    //        if (string.IsNullOrEmpty(result)) return null;

    //        Progress responseData = JsonConvert.DeserializeObject<Progress>(result);

    //        return responseData;
    //    }
    //    catch (Exception e)
    //    {
    //        LoadingManager.Instance.ShowError(e.Message);
    //    }

    //    return null;
    //}

    //// Remove o progresso do usuário em uma determinada fábrica
    //public async Task<bool> RemoveAnswers(int factoryId, EnumIsland? islandId = null)
    //{
    //    try
    //    {
    //        string requestUrl = $"/Question/Answers/{factoryId}";
    //        if (islandId != null) requestUrl += $"/Island/{(int)islandId}";
    //        return await Delete(requestUrl);
    //    }
    //    catch (Exception e)
    //    {
    //        LoadingManager.Instance.ShowError(e.Message);
    //    }

    //    return false;
    //}

    //// Atualiza o avatar do usuário logado
    //public async Task<User> UpdateAvatar(EnumAvatar avatarId)
    //{
    //    WWWForm form = new WWWForm();

    //    form.AddField("avatarId", (int)avatarId);

    //    string result = await Post("/Auth/ChangeAvatar", form);
    //    if (string.IsNullOrEmpty(result)) return null;

    //    User responseData = JsonConvert.DeserializeObject<User>(result);
    //    return responseData;
    //}

    //// Computa as respostas de uma pergunta informando o id da pergunta e os ids das respostas.
    //public async Task<ComputeQuestion> ComputePoints(int questionId, int questionOption, bool isChallenge)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("QuestionId", questionId);
    //    form.AddField("QuestionOption", questionOption);
    //    form.AddField("isChallenge", isChallenge ? "true" : "false");

    //    string result = await Post("/Question/Answer", form);
    //    if (string.IsNullOrEmpty(result)) return null;

    //    ComputeQuestion responseData = JsonConvert.DeserializeObject<ComputeQuestion>(result);
    //    return responseData;
    //}


    // Método auxiliar para realizar requisições via POST
    private async Task<string> Post(string path, WWWForm form = null, bool sendBearer = true, bool isLogin = false)
    {
        LoadingManager.Instance.RequestLoader(true);
        try
        {
            using UnityWebRequest request = UnityWebRequest.Post(baseUrl + path, form);
            if (sendBearer) request.SetRequestHeader("Authorization", "Bearer " + bearer);

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            LoadingManager.Instance.RequestLoader(false);

            string result = request?.downloadHandler?.text;

            if (request.result == Result.Success)
            {
                return result;
            }
            else if (request.responseCode == 401 && sendBearer)
            {
                if (!isLogin) DropUser();
            }
            else
            {
                ErrorMessage error = JsonConvert.DeserializeObject<ErrorMessage>(result);

                if (error.message != null && !isLogin)
                {
                    LoadingManager.Instance.ShowError(error.message);
                }
            }

            return null;
        }
        finally
        {
            LoadingManager.Instance.RequestLoader(false);
        }
    }

    // Método auxiliar para realizar requisições via GET
    private async Task<string> Get(string path)
    {
        LoadingManager.Instance.RequestLoader(true);
        try
        {
            using UnityWebRequest request = UnityWebRequest.Get(baseUrl + path);
            request.SetRequestHeader("Authorization", "Bearer " + bearer);

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            LoadingManager.Instance.RequestLoader(false);

            string result = request?.downloadHandler?.text;

            if (request.result == Result.Success)
            {
                return result;
            }
            else if (request.responseCode == 401)
            {
                DropUser();
            }
            else
            {
                ErrorMessage error = JsonConvert.DeserializeObject<ErrorMessage>(result);

                if (error.message != null)
                {
                    LoadingManager.Instance.ShowError(error.message);
                }
            }

            return null;
        }
        finally
        {
            LoadingManager.Instance.RequestLoader(false);
        }
    }

    // Método auxiliar para realizar requisiçoes via DELETE
    private async Task<bool> Delete(string path)
    {
        LoadingManager.Instance.RequestLoader(true);
        try
        {
            using UnityWebRequest request = UnityWebRequest.Delete(baseUrl + path);
            request.SetRequestHeader("Authorization", "Bearer " + bearer);

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            LoadingManager.Instance.RequestLoader(false);

            if (request.result == Result.Success)
            {
                return true;
            }
            else if (request.responseCode == 401)
            {
                DropUser();
            }
            else
            {
                string result = request?.downloadHandler?.text;
                ErrorMessage error = JsonConvert.DeserializeObject<ErrorMessage>(result);

                if (error.message != null)
                {
                    LoadingManager.Instance.ShowError(error.message);
                }

                return false;
            }
            return request.result == Result.Success;
        }
        finally
        {
            LoadingManager.Instance.RequestLoader(false);
        }

    }

    public void DropUser(bool isRequest = false)
    {
        if (!isRequest)
        {
            LoadingManager.Instance.ShowError("Favor efetuar login novamente.");
        }

        bearer = null;
        PlayerPrefs.DeleteAll();
        //if (!GameManager.Instance.isLoginScreen)
        //{
            LoadingManager.Instance.NextLevel(0);
        //}
    }

}
