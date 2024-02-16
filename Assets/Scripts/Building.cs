using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public abstract class Building : MonoBehaviour {
    public event Action OnSelectionPerformed;
    
    [CanBeNull] private BuildingSO buildingSO;
    [CanBeNull] private GameObject buildingVisual;
    private bool isSelected = false;

    private void Awake() {
        if (buildingSO is not null) {
            buildingVisual = Instantiate(buildingSO.buildingVisual, transform);
        }
    }
    
    public void ChangeBuildingSO(BuildingSO newBuildingSO) {
        buildingSO = newBuildingSO;
        UpdateBuildingVisual();
    }

    private void UpdateBuildingVisual() {
        Destroy(buildingVisual); //check for null?
        buildingVisual = Instantiate(buildingSO.buildingVisual, transform);
    }
    
    public bool IsSelected() => isSelected;
    
    public virtual void SelectBuilding() {
        isSelected = true;
        OnSelectionPerformed?.Invoke();
    }
    public virtual void UnselectBuilding() { 
        isSelected = false;
        OnSelectionPerformed?.Invoke();
    }
    
    
    
}
