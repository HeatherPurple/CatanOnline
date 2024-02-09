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
    [SerializeField] private GameObject cellPrefab; //no need
    [Tooltip("Distance between opposite hex edges")] [SerializeField] private float cellSize;
    [Tooltip("Amount of 'circles' on a map, where =1 is just a hex in a middle")] [SerializeField] private int mapSize;

    [SerializeField] private Transform cellsContainer;

    private static int cellsAmount;
    
    private float xOffsetToUpperCell;
    private float zOffsetToUpperCell;
    private float halfCellSize;
    
    private Vector3[] cellGenerationOffsetArray;
    private readonly Vector3[] crossingsPositionArray = new Vector3[6];
    private readonly Vector3[] roadsPositionArray = new Vector3[6];
    
    private HashSet<GridObject> buildingsHashSet = new HashSet<GridObject>();

    private void Awake() { 
        for (int i = 0; i < mapSize; i++) {
            if (i == 0) {
                cellsAmount = 1;
            }
            cellsAmount += 6 * i;
        }
        
        halfCellSize = cellSize / 2;
        xOffsetToUpperCell = (float) Math.Round(Mathf.Cos(Mathf.Deg2Rad * 30f),3) * halfCellSize;
        zOffsetToUpperCell = 0.75f * cellSize;
        
        CalculateCellGenerationOffset();
        CalculateCrossingsCoordinates();
        CalculateRoadsCoordinates();
    }
    
    private void Start() {
        CreateGrid();
        Debug.Log(buildingsHashSet.Count);
    }
    
    private void CalculateCellGenerationOffset() {
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
    
    private void CalculateCrossingsCoordinates() {
        for (int i = 0; i < 6; i++) {
            float angleRad = Mathf.PI / 3 * i + Mathf.PI / 6;
            
            crossingsPositionArray[i] = new Vector3((float) Math.Round(Mathf.Cos(angleRad),3), 
                0f, (float) Math.Round(Mathf.Sin(angleRad),3)) * halfCellSize;
        }
    }
    
    private void CalculateRoadsCoordinates() {
        for (int i = 0; i < 6; i++) {
            int j = i + 1;
            if (j == 6) {
                j = 0;
            }
            roadsPositionArray[i] = (crossingsPositionArray[i] + crossingsPositionArray[j]) / 2;
        }
    }
    

    private void CreateGrid() {
        Vector3 circleStartPosition = Vector3.zero;
        Vector3 offsetToNewCirclePosition = new Vector3(-xOffsetToUpperCell, 0f, zOffsetToUpperCell);
        
        for (int i = 0; i < mapSize; i++) {
            //iterating through circles
            
            if (i == 0) {
                //spawning middle hex
                SpawnBuilding<Cell>(circleStartPosition);
            } else {
                int hexesInCurrentCircle = 6 * i;
                int hexesOnCurrentSideMax = i+1;
                int hexesOnCurrentSide = 0;
                int offsetIndex = 0;
                
                circleStartPosition += offsetToNewCirclePosition;
                Vector3 circleGenerationOffset = Vector3.zero;
                
                for (int j = 0; j < hexesInCurrentCircle; j++) {
                    //iterating through hexes in a circle

                    Vector3 hexPosition = circleStartPosition + circleGenerationOffset;
                    SpawnBuilding<Cell>(hexPosition);
                    
                    hexesOnCurrentSide++;
                    if (hexesOnCurrentSide >= hexesOnCurrentSideMax) {
                        hexesOnCurrentSide = 1;
                        offsetIndex++;
                        if (offsetIndex >= cellGenerationOffsetArray.Length) {
                            offsetIndex = 0;
                        }
                    }
                    
                    circleGenerationOffset += cellGenerationOffsetArray[offsetIndex];
                }
            }
        }
    }
    
    private void SpawnBuilding<T>(Vector3 position) where T: Building{
        GridObject gridObject = new GridObject(position, typeof(T));
        buildingsHashSet.Add(gridObject);

        if (typeof(T) == typeof(Cell)) {
            const int HEX_CORNERS_AMOUNT = 6;
            Color color = Random.ColorHSV();
            for (int i = 0; i < HEX_CORNERS_AMOUNT; i++) {
                
                SpawnBuilding<Crossing>(position + crossingsPositionArray[i]);
                SpawnBuilding<Road>(position + roadsPositionArray[i]);
            }
            
            GameObject cellObject = Instantiate(cellPrefab, position, Quaternion.identity, transform);
        
            cellObject.transform.localScale *= halfCellSize;
        }
        
        
    }
    

    public static int GetCellsAmount() => cellsAmount;

    public GridObject GetNearestToMousePositionGridObject<T>() where T: Building{
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

        return buildingsHashSet
            .Where(x => x.IsTypeOfBuilding<T>())
            .OrderBy(x => Vector3.Distance(x.GetPosition(), mousePosition))
            .FirstOrDefault();
    }

}
