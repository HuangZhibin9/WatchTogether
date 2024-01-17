using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpriteListSO : ScriptableObject
{
    public Sprite Anon;
    public Sprite Soyo;
    public Sprite Taki;
    public Sprite Rana;
    public Sprite Tomori;

    public Sprite GetSprite(MyLobbyManager.PlayerCharacter playerCharacter)
    {
        switch (playerCharacter)
        {
            case MyLobbyManager.PlayerCharacter.Anon:
                return Anon;

            case MyLobbyManager.PlayerCharacter.Soyo:
                return Soyo;

            case MyLobbyManager.PlayerCharacter.Tomori:
                return Tomori;

            case MyLobbyManager.PlayerCharacter.Rana:
                return Rana;

            case MyLobbyManager.PlayerCharacter.Taki:
                return Taki;

            default: return null;
        }
    }
}