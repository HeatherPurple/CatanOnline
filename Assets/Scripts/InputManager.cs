using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    
    private InputActions inputActions;
    
    private void Awake() {
        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.PointerClick.performed += PointerClick_Performed;
    }

    private void PointerClick_Performed(InputAction.CallbackContext obj) {
        Debug.Log("Click!");
    }

    private void OnDestroy() {
        inputActions.Player.PointerClick.performed -= PointerClick_Performed;
    }
}
