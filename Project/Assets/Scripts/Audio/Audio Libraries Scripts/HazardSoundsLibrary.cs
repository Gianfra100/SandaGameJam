using UnityEngine;

public enum HazardSoundsEnum
{
    Electricity,
    Ball
}

[CreateAssetMenu(menuName = "Audio/Hazard Sounds Library")]
public class HazardSoundsLibrary : AudioLibrary<HazardSoundsEnum>
{

}
