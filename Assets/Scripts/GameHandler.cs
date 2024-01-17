using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

   // public static GameHandler Instance { get; private set; }
   
    public static Action OnGameStateChanged;
    
    public enum GameState {
        BuildingGameField,
        DiceRolling,
        ManagingResources,
        BanditsTurn,
        StealingResources,
        Construction
    }
    // dice roll => (getting cards) / (losing cards -> bandits turn -> stealing a card) => construction phase
    
    private static GameState currentState = GameState.BuildingGameField;
    
    // private void Awake() {
    //     //Instance = this;
    //     
    //     currentState = GameState.BuildingGameField;
    // }

    public static void ChangeGameState(GameState newState) {
        currentState = newState;
        OnGameStateChanged?.Invoke();
    }
}
