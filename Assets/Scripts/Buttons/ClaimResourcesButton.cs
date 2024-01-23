using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimResourcesButton : BaseButton {
    protected override void OnClick() {
        ResourceManager.ClaimResources();
    }
}
