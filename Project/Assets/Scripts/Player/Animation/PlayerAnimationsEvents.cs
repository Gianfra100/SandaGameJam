using System;
using UnityEngine;

public static class PlayerAnimationsEvents
{
    public static event Action OnPlayerDeathEnd;
    
    public static void PlayerDeathEnd() => OnPlayerDeathEnd?.Invoke();
}
