using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField userName;
    public InputField userPassword;
    public Text alertText;
    public GameObject landingPanel;
    public string loggedUsername;
    public Text invalidDataText;
    public Image loginPannel;
    // Start is called before the first frame update
    void Start()
    {
        landingPanel.SetActive(false);
        invalidDataText.gameObject.SetActive(false);
    }

    public void DoLogin()
    {
        if (userName.text == "" || userPassword.text == "")
        {
            invalidDataText.gameObject.SetActive(true);
            StartCoroutine(DeactivateTextl());
        }
        else
        {

            SubmitLogin();
        }
    }

    public void SubmitLogin()
    {
        StartCoroutine(Submit());
    }
    IEnumerator DeactivateTextl()
    {
        yield return new WaitForSeconds(1);
        invalidDataText.gameObject.SetActive(false);

    }
    IEnumerator Submit()
    {
        const string LOGIN_URL = "http://gamerdata.gear.host/api/Login";
        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        LoginObject postData = new LoginObject();
        postData.Username = userName.text;
        postData.Password = userPassword.text;

        var jsonData = JsonMapper.ToJson(postData);

        //var formData = System.Text.Encoding.UTF8.GetBytes("{'Username':'" + userName.text
        //    + "', 'Password':'" + userPassword.text + "'}");

        www = new WWW(LOGIN_URL, System.Text.Encoding.UTF8.GetBytes(jsonData), postHeader);

        yield return www;
        if (www.error != null)
        {
            alertText.text = "Check Your User Name Password";
        }
        else
        {
            Debug.Log("request success");
            alertText.text = "Login Success";
            landingPanel.SetActive(true);
            //loginPannel.enabled = false;
            print(www.text);

            var userData = JsonMapper.ToObject<PlayerObject>(www.text);

            loggedUsername = userData.Name;
            StartCoroutine(Deactivatetext());
        }
    }

    IEnumerator Deactivatetext()
    {
        yield return new WaitForSeconds(.5f);
        alertText.gameObject.SetActive(false);
    }
}



[Serializable]
public class LoginObject
{
    public string Username { get; set; }
    public string Password { get; set; }
}