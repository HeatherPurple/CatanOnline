using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManagerUI : MonoBehaviour
{
    private void Awake() {
        GameHandler.OnGameStateChanged += GameHandler_OnGameStateChanged;
        
        Hide();
    }

    private void GameHandler_OnGameStateChanged() {
        if (GameHandler.GetCurrentGameState() != GameHandler.GameState.Construction) {
            Hide();
        } else {
            Show();
        }
    }
    
    private void Hide() => gameObject.SetActive(false);
    private void Show() => gameObject.SetActive(true);
    
    private void OnDestroy() {
        GameHandler.OnGameStateChanged -= GameHandler_OnGameStateChanged;
    }
}
