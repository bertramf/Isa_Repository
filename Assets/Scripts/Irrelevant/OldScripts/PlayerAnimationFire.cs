using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFire : MonoBehaviour {

	private PlayerStats playerStats;

    private void Start(){
		playerStats = transform.GetComponentInParent<PlayerStats> ();
    }

    public void AttackAnimStart(){
		playerStats.inAttackAnim = true;
        playerStats.canTurn = false;
    }

    public void ActivateSword(){
		playerStats.swordActivated = true;
    }

    public void AttackAnimEnd(){
		playerStats.inAttackAnim = false;
        playerStats.canTurn = true;
    }
}
