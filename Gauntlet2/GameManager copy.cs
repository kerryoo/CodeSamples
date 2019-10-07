using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public event System.Action<MasterPlayer> OnLocalPlayerJoined;
    private GameObject gameObject;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
                _instance.gameObject = new GameObject("_gameManager");

                _instance.gameObject.AddComponent<GameCharacterLibrary>();
                _instance._gameCharacterLibrary.AssembleLibrary();

                _instance.gameObject.AddComponent<GamePassiveItemLibrary>();
                _instance._gamePassiveItemLibrary.DecodeJSON();
            }
            return _instance;
        }
    }

    private GameCharacterLibrary _gameCharacterLibrary;
    public GameCharacterLibrary GameCharacterLibrary
    {
        get
        {
            if (_gameCharacterLibrary == null)
            {
                _gameCharacterLibrary = gameObject.GetComponent<GameCharacterLibrary>();
            }
            return _gameCharacterLibrary;
        }
    }

    private GamePassiveItemLibrary _gamePassiveItemLibrary;
    public GamePassiveItemLibrary GamePassiveItemLibrary
    {
        get
        {
            if (_gamePassiveItemLibrary == null)
            {
                _gamePassiveItemLibrary = gameObject.GetComponent<GamePassiveItemLibrary>();
            }
            return _gamePassiveItemLibrary;
        }
    }


}
