using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {
    public event Action OnSelectionPerformed;
    
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private BuildingSO buildingSO;
    [SerializeField] private GameObject buildingVisual;

    
    private bool isSelected = false;
    
    private void UpdateBuildingVisual() {
        Destroy(buildingVisual);
        buildingVisual = Instantiate(buildingSO.buildingVisual, transform);
    }
    
    public void ChangeBuildingSO(BuildingSO newBuildingSO) {
        buildingSO = newBuildingSO;
        UpdateBuildingVisual();
    }
    
    public bool IsSelected() => isSelected;
    public BuildingSO GetBuildingSO() => buildingSO;
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
