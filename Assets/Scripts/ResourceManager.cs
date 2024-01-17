using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    
    private void Awake() {
        GameHandler.OnGameStateChanged += GameHandler_OnGameStateChanged;
    }

    private void GameHandler_OnGameStateChanged() {
        if (GameHandler.GetCurrentGameState() == GameHandler.GameState.ManagingResources) {
            
        }
    }
    
    

    private void OnDestroy() {
        GameHandler.OnGameStateChanged -= GameHandler_OnGameStateChanged;
    }
    
    
}

