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
    [CanBeNull] private GameObject newVisual;
    private bool isSelected = false;
    private bool isBuilt = false;

    protected virtual void Awake() {
        if (buildingSO is not null) {
            buildingVisual = Instantiate(buildingSO.buildingVisual, transform);
        } else {
            
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
    
    public virtual void SelectBuilding(GameObject prefabToShow = default) {
        if (prefabToShow is null) {
            isSelected = true;
            OnSelectionPerformed?.Invoke();
        } else {
            newVisual = Instantiate(prefabToShow, transform);
            buildingVisual?.SetActive(false);
        }
    }
    public virtual void UnselectBuilding() {
        if (newVisual is null) {
            isSelected = false;
            OnSelectionPerformed?.Invoke();
        } else {
            Destroy(newVisual);
            buildingVisual?.SetActive(true);
        }
    }
    
    public bool IsSelected() => isSelected;
    public bool IsBuilt() => isBuilt;
    public BuildingType GetBuildingType() => buildingType;

}
