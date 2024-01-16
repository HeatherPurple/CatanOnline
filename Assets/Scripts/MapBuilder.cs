using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapBuilder : MonoBehaviour {
    
    [SerializeField] private List<CellSO> cellSOList = new List<CellSO>();

    [CanBeNull] private Cell currentCell;
    
    private void Start() {
        InputManager.Instance.OnPointerClickPerformed += OnPointerClickPerformed;
    }

    private void Update() {
        SelectCell();
    }
    
    private void SelectCell() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100f)) {
            if (hit.transform.TryGetComponent<Cell>(out Cell cell)) {
                if (cell == currentCell) return;
                
                currentCell?.UnselectCell();
                currentCell = cell;
                cell.SelectCell();
            }
        } else {
            currentCell?.UnselectCell();
            currentCell = null;
        }
    }

    

    private void BuildCell(CellSO cellSO) {
        currentCell?.ChangeCellSO(cellSOList[Random.Range(0,2)]);
    }
    
    private void OnPointerClickPerformed(object sender, EventArgs e) {
        BuildCell(cellSOList[0]);
    }

    private void OnDestroy() {
        InputManager.Instance.OnPointerClickPerformed -= OnPointerClickPerformed;
    }
    
    
}
