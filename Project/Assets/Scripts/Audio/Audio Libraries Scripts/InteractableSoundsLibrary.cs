using UnityEngine;

public enum InteratablesSoundsEnum
{ 
    Button,
}

[CreateAssetMenu(menuName = "Audio/Interactables Sounds Library")]
public class InteractableSoundsLibrary : AudioLibrary<InteratablesSoundsEnum>
{

}
