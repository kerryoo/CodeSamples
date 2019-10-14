﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;
    
    public Button submitButton;

    public void CallLogin()
    {
        StartCoroutine(LoginPlayer());
    }

    IEnumerator LoginPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        UnityWebRequest playerLogin = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form);

        yield return playerLogin;

        if (playerLogin.downloadHandler.text == "0") //no error
        {
            DBManager.username = nameField.text;
            DBManager.score = int.Parse(playerLogin.downloadHandler.text.Split('\t')[1]);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);

        } else
        {
            Debug.Log("User login failed. Error #" + playerLogin.downloadHandler.text);
        }

    }

    public void VerifyInputs()
    {
        submitButton.interactable = VerifyUserName(nameField.text) && VerifyPassword(nameField.text);
    }

    //checks if it is between 8 and 20 characters and not repeating letters
    private bool VerifyPassword(string password)
    {
        if (password.Length < 8 || password.Length > 20)
            return false;

        char first = password[0];
        bool distinct = false;

        foreach (char curr in password)
        {
            if (curr != first)
            {
                distinct = true;
            }
        }

        return distinct;

    }

    private bool VerifyUserName(string username)
    {
        return username.Length >= 8 && username.Length <= 20;
    }
}
