using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DBManager : MonoBehaviour
{
    public static string Username;

    public static int Score;
    public static bool LoggedIn { get { return Username != null; } }

    public static int RegisteredUsers;

    private void Start()
    {
        StartCoroutine(SetGameData());
        Messenger.AddListener(GameEvent.ON_GAME_END, CallGameDataUpdate);
    }

    public static void LogOut()
    {
        Username = null;
    }

    private void CallGameDataUpdate()
    {
        StartCoroutine(GameDataUpdate());
    }

    IEnumerator GameDataUpdate()
    {
        WWWForm form = new WWWForm();
        form.AddField("registeredusers", RegisteredUsers);
        UnityWebRequest levelData = UnityWebRequest.Post("http://localhost/sqlconnect/gamedata.php", form);

        yield return levelData;

        if (levelData.downloadHandler.text == "0")
        {
            Debug.Log("Game data successfully updated");
        }
        else
        {
            Debug.Log("Data upload failed. Error #" + levelData.downloadHandler.text);
        }
    }

    IEnumerator SetGameData()
    {
        UnityWebRequest getGameData = new UnityWebRequest("http://localhost/sqlconnect/gamedata.php");
        yield return getGameData;
        string[] results = getGameData.downloadHandler.text.Split('\t');

        RegisteredUsers = int.Parse(results[0]);
    }


}
