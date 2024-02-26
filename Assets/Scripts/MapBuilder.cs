using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MapBuilder : MonoBehaviour {
    
    [SerializeField] private List<CellSOCount> possibleCellsList;
    
    [Serializable]
    private struct CellSOCount {
        public CellSO cellSO;
        public int count;
    }

    private static Action OnBuildingPlaced;
    private static readonly Queue<BuildingSO> queueToBuild = new Queue<BuildingSO>();
    [CanBeNull] private static BuildingSO newBuildingSO;
    [CanBeNull] private static Building currentSelectedBuilding;
    
    private void Awake() {
        OnBuildingPlaced += OnBuildingPlacedPerformed;
    }

    private void Start() {
        InputManager.Instance.OnPointerClickPerformed += OnPointerClickPerformed;
        
        SetupBuildingGameField(HexGrid.GetCellsAmount());
    }

    private void Update() {
        SelectBuilding();
    }
    
    private void SetupBuildingGameField(int hexesNumber) {
        SetupBuildingQueue(hexesNumber);
        TryEndBuildingGameField();
    }

    private void SetupBuildingQueue(int hexesNumber) {
        while (hexesNumber > 0) {
            foreach (var item in possibleCellsList) {
                for (int i = 0; i < item.count; i++) {
                    if (hexesNumber == 0) {
                        TryPeekNextBuilding();
                        return;
                    }
                    queueToBuild.Enqueue(item.cellSO);
                    hexesNumber--;
                }
            }
        }
    }
    
    private static void SelectBuilding() {
        if (newBuildingSO is null) return;
        Building building = HexGrid.GetNearestToMousePositionBuilding(newBuildingSO.buildingType);
        if (building is null) {
            currentSelectedBuilding?.UnselectBuilding();
            currentSelectedBuilding = null;
            return;
        }
        if (building == currentSelectedBuilding) return;

        currentSelectedBuilding?.UnselectBuilding();
        currentSelectedBuilding = building;
        currentSelectedBuilding?.SelectBuilding(newBuildingSO.buildingVisual);
    }
    
    private static void PlaceBuilding() {
        if (newBuildingSO is null) return;
        if (currentSelectedBuilding is null) return;
        
        currentSelectedBuilding?.ChangeBuildingSO(newBuildingSO);
        newBuildingSO = null;
        OnBuildingPlaced?.Invoke();
        
        TryPeekNextBuilding();
    }

    private static void TryPeekNextBuilding() {
        if (newBuildingSO is not null) return;
        queueToBuild.TryDequeue(out newBuildingSO);
    }
    
    private static void TryEndBuildingGameField() {
        if (GameHandler.GetCurrentGameState() is GameHandler.GameState.BuildingGameField && queueToBuild.Count == 0) {
            GameHandler.ChangeGameState(GameHandler.GameState.DiceRolling);
        }
    }
    
    private static void OnBuildingPlacedPerformed() {
        TryEndBuildingGameField();
    }
    
    private static void OnPointerClickPerformed(object sender, EventArgs e) {
        PlaceBuilding();
    }
    
    private void OnDestroy() {
        InputManager.Instance.OnPointerClickPerformed -= OnPointerClickPerformed;
        OnBuildingPlaced -= OnBuildingPlacedPerformed;
    }

    public static void AddBuildingToBuildingQueue(BuildingSO buildingSO) {
        queueToBuild.Enqueue(buildingSO);
        TryPeekNextBuilding();
    }
    
}
