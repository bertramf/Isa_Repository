using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFire : MonoBehaviour {

    private PlayerMovement playerMovementScr;
    private PlayerCombat playerCombatScr;

    private void Start(){
        playerMovementScr = transform.GetComponentInParent<PlayerMovement>();
        playerCombatScr = transform.GetComponentInParent<PlayerCombat>();
    }

    public void AttackAnimStart(){
        playerMovementScr.canTurn = false;
    }

    public void ActivateSword(){
        playerCombatScr.swordActivated = true;
    }

    public void AttackAnimEnd(){
        playerMovementScr.canTurn = true;
    }

    
}
