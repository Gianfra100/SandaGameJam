using UnityEngine;

public enum Music
{
    Menu,
    Level
}

[CreateAssetMenu(menuName = "Audio/Music Library")]
public class MusicLibrary : AudioLibrary<Music> { }
