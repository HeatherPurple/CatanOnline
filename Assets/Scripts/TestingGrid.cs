<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingGrid : MonoBehaviour {
    
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private int mapSize;
    void Start() {
        HexGrid hexGrid = new HexGrid(10, pointPrefab, mapSize);
        
    }

=======
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestingGrid : MonoBehaviour {
    private void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            hit.transform.GetComponent<HexGridCell>().ChangeCellColor(Random.ColorHSV());
            Debug.Log(hit.transform.name);
            Debug.Log("hit");
        }
    }
>>>>>>> Stashed changes
}
