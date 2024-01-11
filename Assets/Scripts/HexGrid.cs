using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid {

    private const float VERTICAL_OFFSET = 0.75f;
    private const float HORIZONTAL_OFFSET = 0.5f;
    
    private readonly Vector3[] offsetArray = new Vector3[] {
        new Vector3(1f,0f,0f),
        new Vector3(0.5f,0f, -0.75f),
        new Vector3( -0.5f,0f, -0.75f),
        new Vector3(-1,0f,0),
        new Vector3( -0.5f,0f, 0.75f),
        new Vector3( 0.5f,0f, 0.75f),
    };

    private int cellSize;
    private int mapSize;

    public HexGrid(int cellSize, GameObject pointPrefab, int mapSize = 2) {

        this.cellSize = cellSize;
        this.mapSize = mapSize;
        
        CreateGrid(pointPrefab);
    }

    private void CreateGrid(GameObject pointPrefab) {
        Vector3 newCircleStartPosition = Vector3.zero;
        Vector3 offsetVector = Vector3.zero;
        
        for (int i = 0; i < mapSize; i++) {
            //iterating through circles
            
            if (i == 0) {
                Vector3 position = newCircleStartPosition;
                GameObject.Instantiate(pointPrefab, position, Quaternion.identity);
                DrawHexagon(position, Color.green);
            } else {
                int hexesAmount = 6 * i;
                int oneSideHexesAmount = i+1;
                int currentHex = 0;
                int offsetIndex = 0;
                
                newCircleStartPosition += new Vector3(-1f * HORIZONTAL_OFFSET * cellSize, 0f,VERTICAL_OFFSET * cellSize);
                offsetVector = Vector3.zero;
                
                for (int j = 0; j < hexesAmount; j++) {
                    //iterating through hexes in a circle

                    Vector3 position = newCircleStartPosition + offsetVector;
                    GameObject.Instantiate(pointPrefab, position, Quaternion.identity);
                    DrawHexagon(position, Color.green);

                    currentHex++;
                    if (currentHex >= oneSideHexesAmount) {
                        currentHex = 1;
                        offsetIndex++;
                        if (offsetIndex >= offsetArray.Length) {
                            offsetIndex = 0;
                        }
                    }
                    
                    offsetVector += offsetArray[offsetIndex] * cellSize;
                }
            }
            
            
        }
    }

    private void DrawHexagon(Vector3 position, Color color) {
        Debug.DrawLine(
            new Vector3(position.x - (float)cellSize / 2, 0f, position.z + (float)cellSize/4),
            new Vector3(position.x, 0f, position.z + (float)cellSize/2), color, 100f);
        Debug.DrawLine(
            new Vector3(position.x, 0f, position.z + (float)cellSize/2),
            new Vector3(position.x + (float)cellSize / 2, 0f, position.z + (float)cellSize/4), color, 100f);
        Debug.DrawLine(
            new Vector3(position.x + (float)cellSize / 2, 0f, position.z + (float)cellSize/4),
            new Vector3(position.x + (float)cellSize / 2, 0f, position.z - (float)cellSize/4), color, 100f);
        Debug.DrawLine(
            new Vector3(position.x + (float)cellSize / 2, 0f, position.z - (float)cellSize/4),
            new Vector3(position.x, 0f, position.z - (float)cellSize/2), color, 100f);
        Debug.DrawLine(
            new Vector3(position.x, 0f, position.z - (float)cellSize/2),
            new Vector3(position.x - (float)cellSize / 2, 0f, position.z - (float)cellSize/4), color, 100f);
        Debug.DrawLine(
            new Vector3(position.x - (float)cellSize / 2, 0f, position.z - (float)cellSize/4),
            new Vector3(position.x - (float)cellSize / 2, 0f, position.z + (float)cellSize/4), color, 100f);
        
    }



}
