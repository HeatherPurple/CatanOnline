using UnityEngine;

public class BuildButton : BaseButton {
    [SerializeField] private BuildingSO buildingSO;
    
    protected override void OnClick() {
        MapBuilder.AddBuildingToBuildingQueue(buildingSO);
    }
}