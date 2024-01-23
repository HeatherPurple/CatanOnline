using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollerUI : MonoBehaviour {

    [SerializeField] private Button rollButton;
    [SerializeField] private TextMeshProUGUI rollButtonText;
    
    private void Awake() {
        GameHandler.OnGameStateChanged += GameHandler_OnGameStateChanged;
        DiceRoller.OnDiceRolled += DiceRoller_OnDiceRolled;
        
        //DisableRollButton();
    }

    private void DiceRoller_OnDiceRolled(int value) {
        rollButtonText.text = value.ToString();
    }

    private void GameHandler_OnGameStateChanged() {
        if (GameHandler.GetCurrentGameState() == GameHandler.GameState.DiceRolling) {
            EnableRollButton();
        } else {
            DisableRollButton();
        }
    }

    private void EnableRollButton() => rollButton.interactable = true;
    private void DisableRollButton() => rollButton.interactable = false;

    private void OnDestroy() {
        GameHandler.OnGameStateChanged -= GameHandler_OnGameStateChanged;
        DiceRoller.OnDiceRolled -= DiceRoller_OnDiceRolled;
    }
}
