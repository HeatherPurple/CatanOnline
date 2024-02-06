using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingVisual : MonoBehaviour {

    [SerializeField] private GameObject selectedVisual;
    
    private Building building;

    private void Awake() {
        building = transform.parent.GetComponent<Building>();
    }

    private void Start() {
        building.OnSelectionPerformed += BuildingOnOnSelectionPerformed;
    }

    private void BuildingOnOnSelectionPerformed() {
        selectedVisual.SetActive(building.IsSelected());
    }

    private void OnDestroy() {
        building.OnSelectionPerformed -= BuildingOnOnSelectionPerformed;
    }
}
