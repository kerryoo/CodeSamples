using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UserData : MonoBehaviour
{
    private float levelStartTime;
    private float levelCompleteTime;
    private int deathCount;
    private int specialAbilityCount;
    private string playerPositions = "";

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("player");

        Messenger.AddListener(GameEvent.ON_LEVEL_START, OnLevelStart);
        Messenger.AddListener(GameEvent.ON_LEVEL_COMPLETE, OnLevelComplete);
        Messenger.AddListener(GameEvent.ON_DEATH, OnDeath);
        Messenger.AddListener(GameEvent.ON_SPECIAL_ABILITY, OnSpecialAbility);
    }

    private void OnLevelStart()
    {
        levelStartTime = Time.time;
        levelCompleteTime = 0;
        deathCount = 0;
        specialAbilityCount = 0;
        playerPositions = "";
        InvokeRepeating("RecordPositions", 0, 10f);
    }

    private void OnLevelComplete()
    {
        levelCompleteTime = Time.time - levelStartTime;
        CancelInvoke();
        StartCoroutine(RegisterData());
    }

    private void OnDeath()
    {
        deathCount += 1;
        levelStartTime = Time.time; //we only want to record the time taken for sucessful rounds
    }

    private void OnSpecialAbility()
    {
        specialAbilityCount += 1;
    }

    private void RecordPositions()
    {
        Vector3 currPosition = player.transform.position;
        currPosition = new Vector3(Mathf.RoundToInt(currPosition.x), Mathf.RoundToInt(currPosition.y), Mathf.RoundToInt(currPosition.z));
        playerPositions += formatPositionString(Time.time - levelStartTime, currPosition);
    }

    private string formatPositionString(float time, Vector3 position)
    {
        return LevelManager.Level + "\t" + Mathf.RoundToInt(time) + "\t" + Mathf.RoundToInt(position.x)
            + "\t" + Mathf.RoundToInt(position.y) + "\t" + Mathf.RoundToInt(position.z) + "\n";
    }

    IEnumerator RegisterData()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", DBManager.Username);
        form.AddField("level", LevelManager.Level);
        form.AddField("timetocompletelevel", Mathf.RoundToInt(levelCompleteTime));
        form.AddField("deathcount", deathCount);
        form.AddField("specialabilitycount", specialAbilityCount);
        form.AddField("playerpositions", playerPositions);

        UnityWebRequest levelData = UnityWebRequest.Post("http://localhost/sqlconnect/playerdata.php", form);

        yield return levelData;

        if (levelData.downloadHandler.text == "0")
        {
            Debug.Log("Data successfully registered.");
        } else {
            Debug.Log("Data upload failed. Error #" + levelData.downloadHandler.text);
        }
    }

}
