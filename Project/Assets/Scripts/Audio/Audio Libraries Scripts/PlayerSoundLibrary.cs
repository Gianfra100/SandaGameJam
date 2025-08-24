using UnityEngine;

public enum PlayerSoundsEnum
{
    Step,
    Jump,
    Death
}

[CreateAssetMenu(menuName = "Audio/Player Sounds Library")]
public class PlayerSoundLibrary : AudioLibrary<PlayerSoundsEnum>
{

}
