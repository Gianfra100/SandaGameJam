using UnityEngine;

public enum MenuSoundsEnum
{
    Open1,
    Open2,
    Close1,
    Close2,
    Button,
}

[CreateAssetMenu(menuName = "Audio/Menu Sounds Library")]
public class MenuSoundsLibrary : AudioLibrary<MenuSoundsEnum> { }
