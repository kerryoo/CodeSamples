using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameCharacterLibrary : GameLibrary
{
    //these prefabs will already have a rigidbody, animator, and character script attached
    [SerializeField] GameObject[] _characterModels;

    [SerializeField] GameObject[] _characterShieldModels;
    [SerializeField] GameObject[] _characterObjects;

    private readonly int NUMBER_OF_CHARACTERS = 8;

    public Dictionary<int, CharacterStats> _statsLibrary = new Dictionary<int, CharacterStats>();
    public Dictionary<int, GameObject> _characterLibrary = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> _characterShieldLibrary = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> _characterObjectsLibrary = new Dictionary<int, GameObject>();

    public override void AssembleLibrary()
    {
        for (int i = SwitchID.Isaac; i < SwitchID.CharacterBuffer + NUMBER_OF_CHARACTERS; i++)
        {
            AssembleCharacterEntry(i);
            _characterShieldLibrary[i] = _characterShieldModels[i - SwitchID.CharacterBuffer];
        }

        int j = SwitchID.ObjectBuffer;
        foreach (GameObject characterObject in _characterObjects)
        {
            _characterObjectsLibrary[j] = characterObject;
            j++;
        }

    }

    private void AssembleCharacterEntry(int characterID)
    {
        _characterLibrary[characterID] = _characterModels[characterID - SwitchID.CharacterBuffer];
        _characterLibrary[characterID].GetComponent<Character>().setCharacterStats(
            GameManager.Instance.getStatsClone(_statsLibrary[characterID]));
        _characterShieldLibrary[characterID] = _characterShieldModels[characterID - SwitchID.CharacterBuffer];
    }

    private void DecodeJson()
    {
        int index = SwitchID.Isaac;
        string m_PassiveItemsPath = Application.dataPath + "/PassiveItems.json";

        var fileStream = new FileStream(m_PassiveItemsPath, FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string json = streamReader.ReadLine();
            CharacterStats currStats = JsonUtility.FromJson<CharacterStats>(@json);
            _statsLibrary[index] = currStats;
        }
    }

    
}