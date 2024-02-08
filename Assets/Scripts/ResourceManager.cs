using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    
    private const int BANDITS_NUMBER = 7;
    
    public static event Action<ResourceManagingOperation> OnResourceManagingStarted;
    public static event Action OnResourceValuesChanged;

    public enum ResourceManagingOperation {
        Receiving,
        Loss,
    }
    

    private static readonly Dictionary<ResourceSO, int> currentResourcesDictionary = new Dictionary<ResourceSO, int>();
    private static readonly List<ResourceSO> newResourcesToClaim = new List<ResourceSO>();
    
    private static int lostResourcesAmount;
    private static ResourceManagingOperation currentOperation;
    
    private void Awake() {
        DiceRoller.OnDiceRolled += DiceRoller_OnDiceRolled;
    }

    private static void DiceRoller_OnDiceRolled(int value) {
        if (value == BANDITS_NUMBER) {
            CalculateLostResources();
        } else {
            CalculateReceivedResources();
        }
    }
    
    private static void CalculateLostResources() {
        lostResourcesAmount = 0;
        if (CountCurrentResources() > BANDITS_NUMBER) {
            int halfTheResources = CountCurrentResources() / 2;
            lostResourcesAmount = halfTheResources;
            
            OnResourceManagingStarted?.Invoke(ResourceManagingOperation.Loss);
        } else {
            GameHandler.ChangeGameState(GameHandler.GameState.BanditsTurn);
        }
    }

    private static void CalculateReceivedResources() {
        newResourcesToClaim.Clear();
  
        //
        newResourcesToClaim.Add(new ResourceSO(){name = "Rock"});
        newResourcesToClaim.Add(new ResourceSO(){name = "Rock"});
        //
        
        if (newResourcesToClaim.Count > 0) {
            OnResourceManagingStarted?.Invoke(ResourceManagingOperation.Receiving);
        } else {
            GameHandler.ChangeGameState(GameHandler.GameState.Construction);
        }
    }

    private static int CountCurrentResources() => currentResourcesDictionary.Sum(keyValuePair => keyValuePair.Value);

    private static void AddResourceToCurrent(ResourceSO resourceSO) {
        if (currentResourcesDictionary.ContainsKey(resourceSO)) {
            currentResourcesDictionary[resourceSO] += 1;
        } else {
            currentResourcesDictionary.Add(resourceSO, 1);
        }
    }
    
    private void OnDestroy() {
        DiceRoller.OnDiceRolled -= DiceRoller_OnDiceRolled;
    }
    
    public static int GetResourceAmount(ResourceSO resourceSO) => 
        currentResourcesDictionary.ContainsKey(resourceSO) ? currentResourcesDictionary[resourceSO] : 0;
    
    public static void ClaimResources() {
        foreach (var resource in newResourcesToClaim) {
            AddResourceToCurrent(resource);
        }
        
        OnResourceValuesChanged?.Invoke();
        
        GameHandler.ChangeGameState(GameHandler.GameState.Construction);
    }

    public void LoseResources() {
        //losingResources
        
        OnResourceValuesChanged?.Invoke();
        
        GameHandler.ChangeGameState(GameHandler.GameState.BanditsTurn);
    }
    
    

   
    
    
}

