using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    private const int SLOTS = 10;
    private int numCharacters;

    private MasterPlayer player;
    public GameObject currCharacter { get; private set; }

    public Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    //every player starts with Isaac
    public void initializeInventory()
    {
        addCharacter(SwitchID.Isaac);
        currCharacter = Characters[SwitchID.Isaac];
        currCharacter.SetActive(true);
    }

    //if the player already has a character, increase his or her experience.
    //if not, add the character to the inventory
    public void handleCharacterCollect(int characterID)
    {
        if (Characters.ContainsKey(characterID))
        {
            Characters[characterID].GetComponent<Character>().addExperience();
            return;
        } 
       
        addCharacter(characterID);
        Debug.Log("character added");
    }

    //Handle the connections necessary when a player gets a new character.
    public void addCharacter(int characterID)
    {
        if (numCharacters >= SLOTS)
        {
            Debug.Log("Inventory full");
            return;
        }

        GameObject newCharacter = Instantiate(GameManager.Instance.GameCharacterLibrary._characterLibrary[characterID]);
        Characters[characterID] = newCharacter;
        newCharacter.GetComponent<Character>().setCharacterActive(false);
        newCharacter.GetComponent<Character>().prepareCharacter(player.InputControl, newCharacter.transform);
        player.handleAddCharacter(newCharacter.GetComponent<Character>());

        numCharacters += 1;
    }

    public void removeCharacter(int characterID)
    {
        Characters.Remove(characterID);
    }

    private void reset()
    {
        Characters.Clear();
    }

    public void switchCharacter(int characterID)
    {
        currCharacter.SetActive(false);
        currCharacter = Characters[characterID];
        currCharacter.SetActive(true);
    }
}
