using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Login : MonoBehaviour
{
    [SerializeField] private string loginEndpoint = "http://127.0.0.1:3000/account/login";
    [SerializeField] private string createEndpoint = "http://127.0.0.1:3000/account/create";
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI alertText;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button createButton;
    public void OnLoginClick()
    {
        alertText.text = "Signing in";
        ActivateButtons(false);
        StartCoroutine(TryLogin());
    }
    public void OnCreateClick()
    {
        alertText.text = "Creating Account...";
        ActivateButtons(false);
        StartCoroutine(TryCreate());
    }
    private void ActivateButtons(bool toggle)
    {
        loginButton.interactable = toggle;
        createButton.interactable = toggle;
    }
    private IEnumerator TryLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if(username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Invalid username";
            ActivateButtons(true);
            yield break;
        }
        if(password.Length<3 || password.Length > 24)
        {
            alertText.text = "Invalid password";
            ActivateButtons(true);
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(loginEndpoint,form);
        var handler = request.SendWebRequest();
        

        float startTime = 0.0f;
        while(!handler.isDone)
        {
            startTime += Time.deltaTime;

            if(startTime > 10.0f)
            {
                break;
            }
            yield return null;
        }
        Debug.Log(request.downloadHandler.text);
        if(request.result == UnityWebRequest.Result.Success)
        {
            if (request.downloadHandler.text != "Invalid credentials")
            {
                alertText.text = "Welcome";
                ActivateButtons(false);
                GameAccount returnedAccount = JsonUtility.FromJson<GameAccount>(request.downloadHandler.text);
                alertText.text = $" Welcome " + returnedAccount.username + ((returnedAccount.adminFlag == 1)? " Admin" : "");
            }
            else
            {
                alertText.text = "Invalid credentials";
                ActivateButtons(true);
            }
        }
        else
        {
            alertText.text = "Error connecting to the server...";
            ActivateButtons(true);
        }
        yield return null;
        
    }
    private IEnumerator TryCreate()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (username.Length < 3 || username.Length > 24)
        {
            alertText.text = "Invalid username";
            loginButton.interactable = true;
            yield break;
        }
        if (password.Length < 3 || password.Length > 24)
        {
            alertText.text = "Invalid password";
            loginButton.interactable = true;
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("rUsername", username);
        form.AddField("rPassword", password);

        UnityWebRequest request = UnityWebRequest.Post(createEndpoint, form);
        var handler = request.SendWebRequest();
        

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }
            yield return null;
        }
        Debug.Log(request.downloadHandler.text);
        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.downloadHandler.text != "Invalid credentials" && request.downloadHandler.text !="Username is already taken.")
            {
                ActivateButtons(true);
                GameAccount returnedAccount = JsonUtility.FromJson<GameAccount>(request.downloadHandler.text);
                alertText.text = "Account has been created.";
            }
            else
            {
                alertText.text = "Username is already taken.";
                
            }
        }
        else
        {
            alertText.text = "Error connecting to the server...";
           
        }
        ActivateButtons(true);
        yield return null;

    }
}
