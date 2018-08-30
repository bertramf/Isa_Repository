using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour {

	private Rigidbody2D rb;
	private PlayerStats playerStats;
	private PlayerState playerState;
	private Transform playerVfx;
	private bool canDash = true;

	public ParticleSystem psDashSilhouette1;
	public ParticleSystem psDashSilhouette2;
	public ParticleSystem psDashSilhouette3;
	public float maxDashSpeed = 10f;
	public float dashTime = 0.2f;
	public float dashCooldown = 0.5f;
	public float RTDeadZone = 0.8f;

	private void Start(){
		rb = GetComponent<Rigidbody2D>();
		playerStats = GetComponent<PlayerStats> ();
		playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
		playerVfx = transform.Find ("PlayerVfx");
	}

	public void Dash(){
		float inputRT = Input.GetAxisRaw ("RT");
		if(playerState.state == CourageState.courage){
			if (inputRT > RTDeadZone) {
				if (canDash) {
					StartCoroutine (DashLoop ());
				}
			}
		}
	}

	private IEnumerator DashLoop(){
		StartCoroutine (DashVisuals());

		canDash = false;
		playerStats.isDashing = true;
		playerStats.canTurn = false;
		rb.isKinematic = true;
		playerStats.dashSpeed = maxDashSpeed;
        //Wwise stukje
        AkSoundEngine.PostEvent("Dash", gameObject);

		yield return new WaitForSeconds (dashTime);

		playerStats.isDashing = false;
		playerStats.canTurn = true;
		rb.isKinematic = false;
		playerStats.dashSpeed = 0f;

		yield return new WaitForSeconds (dashCooldown);

		canDash = true;
	}

	private IEnumerator DashVisuals(){
		float dashTimePartial = dashTime / 4;
		PlayDashParticle (psDashSilhouette1);

		yield return new WaitForSeconds (dashTimePartial);

		PlayDashParticle (psDashSilhouette2);

		yield return new WaitForSeconds (dashTimePartial);

		PlayDashParticle (psDashSilhouette3);
	}

	private void PlayDashParticle(ParticleSystem psDashSilhouette){
		//Unparent
		psDashSilhouette.transform.parent = null;
		//Set Scale goed
		Vector3 vfxScale = new Vector3(playerStats.lookDirection, 1, 1);
		psDashSilhouette.transform.localScale = vfxScale;
		//Play particle
		psDashSilhouette.Play ();
		StartCoroutine (ParentParticle (psDashSilhouette));
	}

	private IEnumerator ParentParticle(ParticleSystem psDashSilhouette){
		yield return new WaitForSeconds (dashTime + dashCooldown - 0.1f);
		psDashSilhouette.transform.SetParent (playerVfx);
		Vector3 localPositionPsDash = Vector3.zero;
		psDashSilhouette.transform.localPosition = localPositionPsDash;
	}

}
