using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBombRangeExtension : PowerUpInterface
{
    protected override void OnPickUp(PlayerController playerController)
    {
        playerController.extraBombRange += 1;
    }
}