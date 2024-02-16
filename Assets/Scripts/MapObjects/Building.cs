using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum BuildingType {
    Cell,
    Crossing,
    Road,
}

public abstract class Building : MonoBehaviour {
    public event Action OnSelectionPerformed;

    protected BuildingType buildingType;
    
    [CanBeNull] private BuildingSO buildingSO;
    [CanBeNull] private GameObject buildingVisual;
    private bool isSelected = false;
    private bool isBuilt = false;

    protected virtual void Awake() {
        if (buildingSO is not null) {
            buildingVisual = Instantiate(buildingSO.buildingVisual, transform);
        }
    }

    private void UpdateBuildingVisual() {
        if (buildingVisual is not null) {
            Destroy(buildingVisual);
        }
        buildingVisual = Instantiate(buildingSO?.buildingVisual, transform);
    }
    
    public void SetDefaultBuildingSO(BuildingSO buildingBuildingSO) {
        ChangeBuildingSO(buildingBuildingSO);
        isBuilt = false;
    }
    
    public void ChangeBuildingSO(BuildingSO newBuildingSO) {
        isBuilt = true;
        buildingSO = newBuildingSO;
        UpdateBuildingVisual();
    }
    
    public virtual void SelectBuilding() {
        isSelected = true;
        OnSelectionPerformed?.Invoke();
    }
    public virtual void UnselectBuilding() { 
        isSelected = false;
        OnSelectionPerformed?.Invoke();
    }
    
    public bool IsSelected() => isSelected;
    public bool IsBuilt() => isBuilt;
    public BuildingType GetBuildingType() => buildingType;

}
