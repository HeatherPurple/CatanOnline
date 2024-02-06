using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {

    public event Action OnSelectionPerformed;
    
    [SerializeField] private LayerMask layerMask;
    
    private bool isSelected = false;
    
    public bool IsSelected() => isSelected;
    public LayerMask GetLayerMask() => layerMask;
    
    public virtual void SelectBuilding() {
        isSelected = true;
        OnSelectionPerformed?.Invoke();
    }
    public virtual void UnselectBuilding() { 
        isSelected = false;
        OnSelectionPerformed?.Invoke();
    }
    
    
    
}
