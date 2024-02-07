using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [Tooltip("Distance between opposite hex edges")] [SerializeField] private float cellSize;
    [Tooltip("Amount of 'circles' on a map, where =1 is just a hex in a middle")] [SerializeField] private int mapSize;

    private static int cellsAmount;
    
    private float xOffsetToUpperCell;
    private float zOffsetToUpperCell;
    private Vector3[] cellGenerationOffsetArray;

    //private Vector3[] vertices;
    // private List<Cell> cellList = new List<Cell>();
    // private HashSet<Cell.Crossing> crossingHashSet = new HashSet<Cell.Crossing>();

    private void Awake() {
        for (int i = 0; i < mapSize; i++) {
            if (i == 0) {
                cellsAmount = 1;
            }
            cellsAmount += 6 * i;
        }
        
        float halfCellSize = cellSize / 2;
        xOffsetToUpperCell = halfCellSize * Mathf.Cos(Mathf.Deg2Rad * 30f);
        zOffsetToUpperCell = 0.75f * cellSize;
        
        
        //CalculateVertices();
        CalculateCellGenerationOffset();
    }
    
    private void Start() {
        CreateGrid();
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

    // private void CalculateVertices() {
    //    vertices = new Vector3[] {
    //         new Vector3(0f, 0f, halfCellSize),
    //         new Vector3(offsetToTheCenterOfRightCell, 0f, halfCellSize / 2),
    //         new Vector3(offsetToTheCenterOfRightCell, 0f, -halfCellSize / 2),
    //         new Vector3(0f, 0f, -halfCellSize),
    //         new Vector3(-offsetToTheCenterOfRightCell, 0f, halfCellSize / 2),
    //         new Vector3(-offsetToTheCenterOfRightCell, 0f, -halfCellSize / 2),
    //     };
    // }

    

    // private void Update() {
    //     //Debug.Log(crossingHashSet.Count);
    // }

    private void CreateGrid() {
        Vector3 circleStartPosition = Vector3.zero;
        Vector3 offsetToNewCirclePosition = new Vector3(-xOffsetToUpperCell, 0f, zOffsetToUpperCell);
        
        for (int i = 0; i < mapSize; i++) {
            //iterating through circles
            
            if (i == 0) {
                //spawning middle hex
                SpawnCell(circleStartPosition);
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
                    SpawnCell(hexPosition);
                    
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
    
    private void SpawnCell(Vector3 position) {
        GameObject cellObject = Instantiate(cellPrefab, position, Quaternion.identity, transform);

        float outerRadius = cellSize / 2;
        cellObject.transform.localScale *= outerRadius;
        
        //Cell cell = cellObject.GetComponent<Cell>();
        //cell.Init(cellSize); 
        //, cellCrossings
        //cellList.Add(cell);
        
        //List<Cell.Crossing> cellCrossings = SpawnCrossings(position);
        
        
        
    }

    // private List<Cell.Crossing> SpawnCrossings(Vector3 cellPosition) {
    //
    //     List<Cell.Crossing> cellCrossings = new List<Cell.Crossing>();
    //     
    //     foreach (var point in vertices) {
    //         Cell.Crossing crossing = new Cell.Crossing() { position = cellPosition + point * halfCellSize, isFilled = false };
    //         cellCrossings.Add(crossing);
    //         crossingHashSet.Add(crossing);
    //     }
    //
    //     return cellCrossings;
    // }

    public static int GetCellsAmount() => cellsAmount;

}
