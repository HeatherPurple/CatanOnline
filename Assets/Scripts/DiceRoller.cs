using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class DiceRoller {
    
    public static event Action<int> OnDiceRolled;
    
    public static void Roll() {
        int rollValue = Random.Range(1, 7) + Random.Range(1, 7);
        
        GameHandler.ChangeGameState(GameHandler.GameState.ManagingResources);
        
        OnDiceRolled?.Invoke(rollValue);
    }
    
}
