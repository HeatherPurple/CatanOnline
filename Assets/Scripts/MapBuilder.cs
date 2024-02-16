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
    
    private static void SetupBuildingGameField(int hexesNumber) {
        SetupBuildingQueue(hexesNumber);
        
        newBuildingSO = queueToBuild.Dequeue();
    }

    private static void SetupBuildingQueue(int hexesNumber) {
        for (int i = 0; i < hexesNumber; i++) {
            queueToBuild.Enqueue(cellsToBuild[Random.Range(0, cellsToBuild.Count)]);
        }
        //add to queue ~50 hexes, 1 village, 1 road
        
    }
    
    private static void SelectBuilding() {
        Type buildingType = newBuildingSO?.GetBuildingType();
        if (buildingType is null) return;
        Building building = HexGrid.GetNearestToMousePositionGridObject(buildingType)?.GetBuilding();
        if (building is null) {
            currentSelectedBuilding?.UnselectBuilding();
            currentSelectedBuilding = null;
            return;
        }
        if (building == currentSelectedBuilding) return;

        currentSelectedBuilding?.UnselectBuilding();
        currentSelectedBuilding = building;
        currentSelectedBuilding?.SelectBuilding();
    }
    
    private static void PlaceBuilding() {
        if (newBuildingSO is null) return;
        if (currentSelectedBuilding is null) return;
        
        currentSelectedBuilding?.ChangeBuildingSO(newBuildingSO);

        PeekNextBuilding();
    }

    private static void PeekNextBuilding() {
        if (queueToBuild.TryDequeue(out newBuildingSO)) return;
        if (GameHandler.GetCurrentGameState() is GameHandler.GameState.BuildingGameField) {
            GameHandler.ChangeGameState(GameHandler.GameState.DiceRolling);
        }
    }
    
    private static void OnPointerClickPerformed(object sender, EventArgs e) {
        PlaceBuilding();
    }
    
    private void OnDestroy() {
        InputManager.Instance.OnPointerClickPerformed -= OnPointerClickPerformed;
    }
    
}
