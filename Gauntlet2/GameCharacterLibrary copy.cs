using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GameCharacterLibrary : GameLibrary
{
    //these prefabs will already have a rigidbody, animator, and character script attached
    [SerializeField] GameObject[] _characterModels;

    private readonly int NUMBER_OF_CHARACTERS = 8;
    public Dictionary<int, GameObject> _characterLibrary;

    public override void AssembleLibrary()
    {
        _characterLibrary = new Dictionary<int, GameObject>();

        _characterLibrary[SwitchID.Isaac] = _characterModels[SwitchID.Isaac - SwitchID.CharacterBuffer];
        _characterLibrary[SwitchID.Firefly] = _characterModels[SwitchID.Firefly - SwitchID.CharacterBuffer];
        _characterLibrary[SwitchID.Kassandra] = _characterModels[SwitchID.Kassandra - SwitchID.CharacterBuffer];
        _characterLibrary[SwitchID.Lutece] = _characterModels[SwitchID.Lutece - SwitchID.CharacterBuffer];
        _characterLibrary[SwitchID.Dexter] = _characterModels[SwitchID.Dexter - SwitchID.CharacterBuffer];
        _characterLibrary[SwitchID.Cairne] = _characterModels[SwitchID.Cairne - SwitchID.CharacterBuffer];
        _characterLibrary[SwitchID.XJTen] = _characterModels[SwitchID.XJTen - SwitchID.CharacterBuffer];
        _characterLibrary[SwitchID.Terra] = _characterModels[SwitchID.Terra - SwitchID.CharacterBuffer];

    }

    private void DecodeJson()
    {
        string m_PassiveItemsPath = Application.dataPath + "/PassiveItems.json";

        var fileStream = new FileStream(m_PassiveItemsPath, FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string json = streamReader.ReadLine();
            CharacterStats currStats = JsonUtility.FromJson<CharacterStats>(@json);
        }
    }

    public GameObject getCharacterClone(int ID)
    {
        GameObject characterClone = _characterLibrary[ID];
    }

    private void setConnections(GameObject characterModel)
    {
        Character character = characterModel.GetComponent<Character>();
       

    }

    
}