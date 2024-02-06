using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapBuilder : MonoBehaviour {
    
    [SerializeField] private List<CellSO> possibleBuildingList;
    private static readonly Queue<CellSO> queueToBuild = new Queue<CellSO>();
    [CanBeNull] private static CellSO newBuildingSO;
    
    [CanBeNull] private static Building currentSelectedBuilding;
    
    private void Awake() {
        SetupBuildingGameField();
    }

    private void Start() {
        InputManager.Instance.OnPointerClickPerformed += OnPointerClickPerformed;
    }

    private void Update() {
        SelectBuilding();
    }

    private void SetupBuildingGameField() {
        queueToBuild.Enqueue(possibleBuildingList[0]);
        queueToBuild.Enqueue(possibleBuildingList[1]);
        queueToBuild.Enqueue(possibleBuildingList[1]);
        queueToBuild.Enqueue(possibleBuildingList[0]);
        //queueToBuild.Enqueue();
        //add to queue ~50 hexes, 1 village, 1 road
        newBuildingSO = queueToBuild.Dequeue();
    }
    
    private void SelectBuilding() {
        //LayerMask layerMask = newBuilding?.GetLayerMask() ?? new LayerMask();
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit)) { // layermask
            if (!hit.transform.TryGetComponent(out Building building)) return;
            if (building == currentSelectedBuilding) return;
                
            currentSelectedBuilding?.UnselectBuilding();
            currentSelectedBuilding = building;
            building.SelectBuilding();
        } else {
            currentSelectedBuilding?.UnselectBuilding();
            currentSelectedBuilding = null;
        }
    }
    
    private static void Build() {
        if (newBuildingSO is null) return;
        if (currentSelectedBuilding is null) return;
        
        Cell currentCell = currentSelectedBuilding as Cell;
        currentCell?.ChangeCellSO(newBuildingSO);

        PeekNextBuilding();
    }

    private static void PeekNextBuilding() {
        if (queueToBuild.TryDequeue(out newBuildingSO)) {
            
        }
    }
    
    private void OnPointerClickPerformed(object sender, EventArgs e) {
        Build();
    }

    private void OnDestroy() {
        InputManager.Instance.OnPointerClickPerformed -= OnPointerClickPerformed;
    }

    public static void StartBuilding() {
        //newBuilding = queueToBuild.Peek();
    }
    
    
}
