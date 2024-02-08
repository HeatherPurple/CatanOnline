using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndConstructionButton : BaseButton
{
    protected override void OnClick() {
        GameHandler.ChangeGameState(GameHandler.GameState.DiceRolling);
    }
}
