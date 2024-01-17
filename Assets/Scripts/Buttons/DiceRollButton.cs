using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollButton : BaseButton {
    
    protected override void OnClick() {
        DiceRoller.Roll();
    }
}
