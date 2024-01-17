using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class DiceRoller {
    
    public static Action<int> OnDiceRolled;
    
    public static void Roll() {
        int rollValue = Random.Range(0, 7) + Random.Range(0, 7);
        
        OnDiceRolled?.Invoke(rollValue);
        
        GameHandler.ChangeGameState(GameHandler.GameState.ManagingResources);
    }
    
}
