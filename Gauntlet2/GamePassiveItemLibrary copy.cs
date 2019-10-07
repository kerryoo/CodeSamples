using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GamePassiveItemLibrary : GameLibrary
{

    private Dictionary<int, PassiveItem> passiveItemLibrary;

    public override void AssembleLibrary()
    {
        passiveItemLibrary = new Dictionary<int, PassiveItem>();

        string m_PassiveItemsPath = Application.dataPath + "/PassiveItems.json";
        string json;

        var fileStream = new FileStream(m_PassiveItemsPath, FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            while ((json = streamReader.ReadLine()) != null)
            {
                PassiveItem currPassiveItem = JsonUtility.FromJson<PassiveItem>(@json);
                passiveItemLibrary[currPassiveItem.ID] = currPassiveItem;

            }
        }
    }

    public void onPassiveItemCollected(ref Character character, int ItemID)
    {
        character.implementPassiveItem(passiveItemLibrary[ItemID]);
    }

}