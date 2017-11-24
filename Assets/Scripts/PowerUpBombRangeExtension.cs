using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBombRangeExtension : PowerUpInterface
{
    protected override void OnPickUp(PlayerController playerController)
    {
        base.OnPickUp(playerController);

        playerController.extraBombRange += 1;
    }
}