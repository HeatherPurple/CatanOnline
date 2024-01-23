using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourceManagerUI : MonoBehaviour {
    
    [SerializeField] private GameObject resourceManagingPanel;
    [SerializeField] private GameObject receiveResourcesPanel;
    [SerializeField] private GameObject loseResourcesPanel;
    
    private void Awake() {
        ResourceManager.OnResourceManagingStarted += ResourceManager_OnResourceManagingStarted;
        GameHandler.OnGameStateChanged += GameHandler_OnGameStateChanged;
        
        HideResourceManagingPanel();
    }

    private void GameHandler_OnGameStateChanged() {
        if (GameHandler.GetCurrentGameState() != GameHandler.GameState.ManagingResources) {
            HideResourceManagingPanel();
        }
    }

    private void ResourceManager_OnResourceManagingStarted(ResourceManager.ResourceManagingOperation operation) {
        ShowResourceManagingPanel();
        
        if (operation == ResourceManager.ResourceManagingOperation.Loss) {
            receiveResourcesPanel.SetActive(false);
            loseResourcesPanel.SetActive(true);
        } else {
            loseResourcesPanel.SetActive(false);
            receiveResourcesPanel.SetActive(true);
        }
    }
    
    private void HideResourceManagingPanel() => resourceManagingPanel.SetActive(false);
    private void ShowResourceManagingPanel() => resourceManagingPanel.SetActive(true);
    
    private void OnDestroy() {
        ResourceManager.OnResourceManagingStarted -= ResourceManager_OnResourceManagingStarted;
        GameHandler.OnGameStateChanged -= GameHandler_OnGameStateChanged;
    }
    
}
