using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class BaseButton : MonoBehaviour {
    
    private Button button;

    protected virtual void Awake() {
        button = GetComponent<Button>();
    }

    protected virtual void Start() { 
        button.onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();

    protected virtual void OnDestroy() {
        button.onClick.RemoveListener(OnClick);
    }
}
