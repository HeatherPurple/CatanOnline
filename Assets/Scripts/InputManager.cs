using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

    public static InputManager Instance { get; private set;}

    public EventHandler OnPointerClickPerformed;
    
    private InputActions inputActions;
    
    
    private void Awake() {
        Instance = this;
        
        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.PointerClick.performed += PointerClick_Performed;
    }

    private void PointerClick_Performed(InputAction.CallbackContext obj) {
        OnPointerClickPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        inputActions.Player.PointerClick.performed -= PointerClick_Performed;
    }
}
