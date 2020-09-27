using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public void OnClickCharacterPick(int whichCharacter)
    {
        if(PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedCharecter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }
}
