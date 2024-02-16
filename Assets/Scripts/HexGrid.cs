using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HexGrid : MonoBehaviour
{
    [Tooltip("Distance between opposite hex edges")] [SerializeField] private float cellSize;
    [Tooltip("Amount of 'circles' on a map, where =1 is just a hex in a middle")] [SerializeField] private int mapSize;
    [SerializeField] private BuildingSO defaultCellSO;

    private const int HEX_CORNERS_AMOUNT = 6;

    private static Vector3 gridPosition;
    private static float xOffsetToUpperCell;
    private static float zOffsetToUpperCell;
    private static float outerRadius;
    private static int cellsAmount;
    private static Vector3[] cellGenerationOffsetArray;
    private static readonly Vector3[] crossingsPositionArray = new Vector3[HEX_CORNERS_AMOUNT];
    private static readonly Vector3[] roadsPositionArray = new Vector3[HEX_CORNERS_AMOUNT];
    private static readonly HashSet<GridObject> buildingsHashSet = new HashSet<GridObject>();

    private void Awake() { 
        for (int i = 0; i < mapSize; i++) {
            if (i == 0) {
                cellsAmount = 1;
            }
            cellsAmount += HEX_CORNERS_AMOUNT * i;
        }

        gridPosition = transform.position;
        
        outerRadius = cellSize / 2;
        xOffsetToUpperCell = (float) Math.Round(Mathf.Cos(Mathf.Deg2Rad * 30f),3) * outerRadius;
        zOffsetToUpperCell = 0.75f * cellSize;
        
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
            new Vector3(xOffsetToRightCell,0f,0f),
            new Vector3(xOffsetToUpperCell,0f, -zOffsetToUpperCell),
            new Vector3( -xOffsetToUpperCell,0f, -zOffsetToUpperCell),
            new Vector3(-xOffsetToRightCell,0f,0),
            new Vector3( -xOffsetToUpperCell,0f, zOffsetToUpperCell),
            new Vector3( xOffsetToUpperCell,0f, zOffsetToUpperCell),
        };
    }
    
    private static void CalculateCrossingsCoordinates() {
        for (int i = 0; i < HEX_CORNERS_AMOUNT; i++) {
            float angleRad = Mathf.PI / 3 * i + Mathf.PI / 6;
            
            crossingsPositionArray[i] = new Vector3((float) Math.Round(Mathf.Cos(angleRad),3), 
                0f, (float) Math.Round(Mathf.Sin(angleRad),3)) * outerRadius;
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
                SpawnGridObject<Cell>(circleStartPosition);
            } else {
                int hexesInCurrentCircle = HEX_CORNERS_AMOUNT * i;
                int hexesOnCurrentSideMax = i + 1;
                int hexesOnCurrentSide = 0;
                int offsetIndex = 0;
                
                circleStartPosition += offsetToNewCirclePosition;
                Vector3 circleGenerationOffset = Vector3.zero;
                
                for (int j = 0; j < hexesInCurrentCircle; j++) {
                    Vector3 hexPosition = circleStartPosition + circleGenerationOffset;
                    SpawnGridObject<Cell>(hexPosition);
                    
                    hexesOnCurrentSide++;
                    if (hexesOnCurrentSide >= hexesOnCurrentSideMax) {
                        hexesOnCurrentSide = 1;
                        offsetIndex++;
                        // if (offsetIndex >= cellGenerationOffsetArray.Length) {
                        //     offsetIndex = 0;
                        // }
                    }
                    
                    circleGenerationOffset += cellGenerationOffsetArray[offsetIndex];
                }
            }
        }
    }
    
    private void SpawnGridObject<T>(Vector3 position) where T: Building {
        GridObject gridObject = new GridObject(transform, outerRadius, position, typeof(T));
        buildingsHashSet.Add(gridObject);

        if (typeof(T) == typeof(Cell)) {
            for (int i = 0; i < HEX_CORNERS_AMOUNT; i++) {
                SpawnGridObject<Crossing>(position + crossingsPositionArray[i]);
                SpawnGridObject<Road>(position + roadsPositionArray[i]);
            }
            
            gridObject.GetBuilding().ChangeBuildingSO(defaultCellSO);//
        }
    }
    

    public static int GetCellsAmount() => cellsAmount;

    public static GridObject? GetNearestToMousePositionGridObject(Type buildingType) {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Camera.main.transform.position.y - gridPosition.y;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        return buildingsHashSet
            .Where(x => x.IsTypeOfBuilding(buildingType))
            .Where(x => Vector3.Distance(x.GetPosition(), worldPosition) <= outerRadius)
            .OrderBy(x => Vector3.Distance(x.GetPosition(), worldPosition))
            .FirstOrDefault();
    }

}
