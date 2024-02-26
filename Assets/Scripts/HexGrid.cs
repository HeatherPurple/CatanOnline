using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class HexGrid : MonoBehaviour {
    [Tooltip("Amount of 'circles' on a map, where =1 is just a hex in a middle")] [SerializeField]
    private int mapSize;
    [SerializeField] private List<BuildingData> defaultBuildingDataList = new List<BuildingData>();

    private const int HEX_CORNERS_AMOUNT = 6;
    private const float CELL_SIZE = 10;

    private static Vector3 gridPosition;
    private static float xOffsetToUpperCell;
    private static float zOffsetToUpperCell;
    private static float outerRadius;
    private static int cellsAmount;
    private static Vector3[] cellGenerationOffsetArray;
    private static Vector3[] crossingsPositionArray = new Vector3[HEX_CORNERS_AMOUNT];
    private static Vector3[] roadsPositionArray = new Vector3[HEX_CORNERS_AMOUNT];
    private static HashSet<Building> buildingsHashSet = new HashSet<Building>();

    private void Awake() {
        for (int i = 0; i < mapSize; i++) {
            if (i == 0) {
                cellsAmount = 1;
            }

            cellsAmount += 6 * i;
        }

        gridPosition = transform.position;

        outerRadius = CELL_SIZE / 2;
        xOffsetToUpperCell = (float)Math.Round(Mathf.Cos(Mathf.Deg2Rad * 30f), 3) * outerRadius;
        zOffsetToUpperCell = 0.75f * CELL_SIZE;

        CalculateCellGenerationOffset();
        CalculateCrossingsCoordinates();
        CalculateRoadsCoordinates();
    }

    private void Start() {
        CreateGrid();
    }

    private static void CalculateCellGenerationOffset() {
        float xOffsetToRightCell = xOffsetToUpperCell * 2;

        cellGenerationOffsetArray = new Vector3[] {
            new Vector3(xOffsetToRightCell, 0f, 0f),
            new Vector3(xOffsetToUpperCell, 0f, -zOffsetToUpperCell),
            new Vector3(-xOffsetToUpperCell, 0f, -zOffsetToUpperCell),
            new Vector3(-xOffsetToRightCell, 0f, 0),
            new Vector3(-xOffsetToUpperCell, 0f, zOffsetToUpperCell),
            new Vector3(xOffsetToUpperCell, 0f, zOffsetToUpperCell),
        };
    }

    private static void CalculateCrossingsCoordinates() {
        for (int i = 0; i < HEX_CORNERS_AMOUNT; i++) {
            float angleRad = Mathf.PI / 3 * i + Mathf.PI / 6;

            crossingsPositionArray[i] = new Vector3((float)Math.Round(Mathf.Cos(angleRad), 3),
                0f, (float)Math.Round(Mathf.Sin(angleRad), 3)) * outerRadius;
        }
    }

    private static void CalculateRoadsCoordinates() {
        for (int i = 0; i < HEX_CORNERS_AMOUNT; i++) {
            int j = i + 1;
            if (j == HEX_CORNERS_AMOUNT) {
                j = 0;
            }

            roadsPositionArray[i] = (crossingsPositionArray[i] + crossingsPositionArray[j]) / 2;
        }
    }


    private void CreateGrid() {
        Vector3 circleStartPosition = Vector3.zero;
        Vector3 offsetToNewCirclePosition = new Vector3(-xOffsetToUpperCell, 0f, zOffsetToUpperCell);

        for (int i = 0; i < mapSize; i++) {
            if (i == 0) {
                SpawnPlaceFor<Cell>(circleStartPosition);
            } else {
                int hexesInCurrentCircle = HEX_CORNERS_AMOUNT * i;
                int hexesOnCurrentSideMax = i + 1;
                int hexesOnCurrentSide = 0;
                int offsetIndex = 0;

                circleStartPosition += offsetToNewCirclePosition;
                Vector3 circleGenerationOffset = Vector3.zero;

                for (int j = 0; j < hexesInCurrentCircle; j++) {
                    Vector3 hexPosition = circleStartPosition + circleGenerationOffset;
                    SpawnPlaceFor<Cell>(hexPosition);

                    hexesOnCurrentSide++;
                    if (hexesOnCurrentSide >= hexesOnCurrentSideMax) {
                        hexesOnCurrentSide = 1;
                        offsetIndex++;
                    }

                    circleGenerationOffset += cellGenerationOffsetArray[offsetIndex];
                }
            }
        }
    }

    private void SpawnPlaceFor<T>(Vector3 position) where T : Building {
        if (!CanPlaceBuildingHere(position)) return;

        GameObject spawnedBuildingGameObject = new GameObject {
            transform = {
                parent = transform,
                position = position,
            }
        };
        spawnedBuildingGameObject.transform.localScale *= outerRadius;
        Building building = spawnedBuildingGameObject.AddComponent<T>();
        buildingsHashSet.Add(building);

        BuildingSO defaultBuildingSO = defaultBuildingDataList
            .FirstOrDefault(x => x.buildingType == building.GetBuildingType())
            ?.buildingSO;
        if (defaultBuildingSO is not null) {
            building.SetDefaultBuildingSO(defaultBuildingSO);
        }

        if (typeof(T) != typeof(Cell)) return;

        for (int i = 0; i < HEX_CORNERS_AMOUNT; i++) {
            SpawnPlaceFor<Crossing>(position + crossingsPositionArray[i]);
            SpawnPlaceFor<Road>(position + roadsPositionArray[i]);
        }
    }

    private static bool CanPlaceBuildingHere(Vector3 position) {
        const float BUILDING_PLACE_RADIUS = 0.05f;
        return buildingsHashSet
            .Count(x => Vector3.Distance(x.transform.position, position) <= BUILDING_PLACE_RADIUS) <= 0;
    }

    [CanBeNull]
    public static Building GetNearestToMousePositionBuilding(BuildingType buildingType, bool forceBuild = false) {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Camera.main.transform.position.y - gridPosition.y;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        return buildingsHashSet
            .Where(x => x.GetBuildingType() == buildingType)
            .Where(x => !x.IsBuilt() || forceBuild)
            .Where(x => Vector3.Distance(x.transform.position, worldPosition) <= outerRadius)
            .OrderBy(x => Vector3.Distance(x.transform.position, worldPosition))
            .FirstOrDefault();
    }

    public static int GetCellsAmount() => cellsAmount;
}