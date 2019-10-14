using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    public readonly int ID_BUFFER = 100000000;
    public int ID;

    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;

    private void Start()
    {
        StartCoroutine(GetGameData());
    }

    public void CallRegister()
    {
        StartCoroutine(RegisterPlayer());
    }

    IEnumerator GetGameData()
    {
        UnityWebRequest gameData = new UnityWebRequest("http://localhost/sqlconnect/gamedata.php");
        yield return gameData;
    }

    IEnumerator RegisterPlayer()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);
        form.AddField("ID", ID_BUFFER + DBManager.registeredUsers);

        UnityWebRequest playerRegister = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form);
      
        yield return playerRegister.SendWebRequest();

        if (playerRegister.isNetworkError || playerRegister.isHttpError)
        {
            Debug.Log("User creation failed. Error: " + playerRegister.error);
        } else
        {
            Debug.Log("User created successfully.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        DBManager.registeredUsers += 1;

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
