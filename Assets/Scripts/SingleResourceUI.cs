using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SingleResourceUI : MonoBehaviour {

    [SerializeField] private ResourceSO resourceSO;
    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI resourceNameText;
    [SerializeField] private TextMeshProUGUI resourceAmountText;

    private void Awake() {
        resourceImage.sprite = resourceSO.icon;
        resourceNameText.text = resourceSO.resourceName;
    }

    private void Start() {
        ResourceManager.OnResourceValuesChanged += ResourceManager_OnResourceValuesChanged;
        
        UpdateResourceAmount();
    }

    private void ResourceManager_OnResourceValuesChanged() {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount() {
        resourceAmountText.text = ResourceManager.GetResourceAmount(resourceSO).ToString();
    }

    private void OnDestroy() {
        ResourceManager.OnResourceValuesChanged -= ResourceManager_OnResourceValuesChanged;
    }
}
