using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterDict<TestCharacter> dict;

    void Start()
    {
        var items = DataTableMgr.GetTable<TestCharacterTable>();

  //      int[] ids = new int[items];

  //      for (int i = 0; i < items.Length; i++)
  //      {
  //          ids[i] = items[i].id;
  //      }

		//dict = new CharacterDict<TestCharacter>(ids, items);
	}

    void Update()
    {
        
    }
}
