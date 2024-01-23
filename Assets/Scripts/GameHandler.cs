using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHandler {
    
    public static event Action OnGameStateChanged;
    
    public enum GameState {
        BuildingGameField,
        DiceRolling,
        ManagingResources,
        BanditsTurn,
        StealingResources,
        Construction
    }
    
    private static GameState currentState = GameState.BuildingGameField;

    public static GameState GetCurrentGameState() => currentState;

    public static void ChangeGameState(GameState newState) {
        currentState = newState;
        OnGameStateChanged?.Invoke();
    }
}
