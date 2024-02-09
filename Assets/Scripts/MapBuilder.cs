using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapBuilder : MonoBehaviour {
    
    [SerializeField] private List<CellSO> possibleCellsToBuild;
    
    private static List<CellSO> cellsToBuild;
    private static readonly Queue<BuildingSO> queueToBuild = new Queue<BuildingSO>();
    [CanBeNull] private static BuildingSO newBuildingSO;
    [CanBeNull] private static Building currentSelectedBuilding;
    
    private void Awake() {
        cellsToBuild = possibleCellsToBuild;
    }

    private void Start() {
        InputManager.Instance.OnPointerClickPerformed += OnPointerClickPerformed;
        
        SetupBuildingGameField(HexGrid.GetCellsAmount());
    }

    private void Update() {
        SelectBuilding();
    }
    
    private static void SelectBuilding() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayMaxDistance = 100f;
        LayerMask layerMask = newBuildingSO?.layerMask ?? new LayerMask();
        
        if (Physics.Raycast(ray, out var hit, rayMaxDistance,layerMask)) {
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

    private void Build<T>(T buildingSO) where T: BuildingSO {
        //add to queue
        //peek to newBuilding
        //instantiate Tbuilding
    }

    private void SelectGridObject() {
        //var item = Grid.buildingsHashSet.Where(x => x. == T).Select one with nearest coord
        //currentItem = item
        //Tbuilding.transform.position = item.GetPosition()
    }

    private void PlaceT() {
        //curentItem.SetTransform
    }
    
    private static void Build() {
        if (newBuildingSO is null) return;
        if (currentSelectedBuilding is null) return;
        
        currentSelectedBuilding?.ChangeBuildingSO(newBuildingSO);

        PeekNextBuilding();
    }

    private static void PeekNextBuilding() {
        if (!queueToBuild.TryDequeue(out newBuildingSO)) {
            if (GameHandler.GetCurrentGameState() is GameHandler.GameState.BuildingGameField) {
                GameHandler.ChangeGameState(GameHandler.GameState.DiceRolling);
            }
        }
    }
    
    private void OnPointerClickPerformed(object sender, EventArgs e) {
        Build();
    }

    private static void SetupBuildingQueue(int hexesNumber) {
        for (int i = 0; i < hexesNumber; i++) {
            queueToBuild.Enqueue(cellsToBuild[Random.Range(0, cellsToBuild.Count)]);
        }
        //add to queue ~50 hexes, 1 village, 1 road
        
    }
    
    private static void SetupBuildingGameField(int hexesNumber) {
        SetupBuildingQueue(hexesNumber);
        
        newBuildingSO = queueToBuild.Dequeue();
    }
    
    private void OnDestroy() {
        InputManager.Instance.OnPointerClickPerformed -= OnPointerClickPerformed;
    }
    
}
