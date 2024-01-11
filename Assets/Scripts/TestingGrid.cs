
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestingGrid : MonoBehaviour {
    private void Start() {
        InputManager.Instance.OnPointerClickPerformed += OnPointerClickPerformed;
    }

    private void OnPointerClickPerformed(object sender, EventArgs e) {
        ChangeCellColor();
    }

    private void ChangeCellColor() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            hit.transform.GetComponent<HexGridCell>().ChangeCellColor(Random.ColorHSV());
        }
    }

    private void OnDestroy() {
        InputManager.Instance.OnPointerClickPerformed -= OnPointerClickPerformed;
    }
}
