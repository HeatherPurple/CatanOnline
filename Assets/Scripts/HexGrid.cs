using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [SerializeField] private GameObject hexGridCellPrefab;
    [SerializeField] private float cellSize;
    [SerializeField] private int mapSize;

    private const float VERTICAL_OFFSET = 0.75f;
    private const float HORIZONTAL_OFFSET = 0.4330127f;
    
    private readonly Vector3[] offsetArray = new Vector3[] {
        new Vector3(2 * HORIZONTAL_OFFSET,0f,0f),
        new Vector3(HORIZONTAL_OFFSET,0f, -VERTICAL_OFFSET),
        new Vector3( -HORIZONTAL_OFFSET,0f, -VERTICAL_OFFSET),
        new Vector3(-1 * 2 * HORIZONTAL_OFFSET,0f,0),
        new Vector3( -HORIZONTAL_OFFSET,0f, VERTICAL_OFFSET),
        new Vector3( HORIZONTAL_OFFSET,0f, VERTICAL_OFFSET),
    };

    private void Awake() {
        CreateGrid(hexGridCellPrefab);
    }

    private void CreateGrid(GameObject hexGridCellPrefab) {
        Vector3 newCircleStartPosition = Vector3.zero;
        Vector3 offsetVector = Vector3.zero;
        
        for (int i = 0; i < mapSize; i++) {
            //iterating through circles
            
            if (i == 0) {
                Vector3 position = newCircleStartPosition;
                GameObject cellPrefab = Instantiate(hexGridCellPrefab, position, Quaternion.identity, transform);
                cellPrefab.GetComponent<HexGridCell>().Init(cellSize);
            } else {
                int hexesAmount = 6 * i;
                int oneSideHexesAmount = i+1;
                int currentHex = 0;
                int offsetIndex = 0;
                
                newCircleStartPosition += new Vector3(-1f * HORIZONTAL_OFFSET * cellSize / 2 , 0f,VERTICAL_OFFSET * cellSize/ 2);
                offsetVector = Vector3.zero;
                
                for (int j = 0; j < hexesAmount; j++) {
                    //iterating through hexes in a circle

                    Vector3 position = newCircleStartPosition + offsetVector;
                    GameObject cellPrefab = Instantiate(hexGridCellPrefab, position, Quaternion.identity, transform);
                    cellPrefab.GetComponent<HexGridCell>().Init(cellSize);

                    currentHex++;
                    if (currentHex >= oneSideHexesAmount) {
                        currentHex = 1;
                        offsetIndex++;
                        if (offsetIndex >= offsetArray.Length) {
                            offsetIndex = 0;
                        }
                    }
                    
                    offsetVector += offsetArray[offsetIndex] * cellSize / 2;
                }
            }
            
            
        }
    }



}
