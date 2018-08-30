using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycasts : MonoBehaviour {

	private PlayerStats playerStats;
	private Vector2 topLeft1;
	private Vector2 bottomRight1;
	private Vector2 topLeft2;
	private Vector2 bottomRight2;
	private Vector2 topLeft3;
	private Vector2 bottomRight3;

    public LayerMask groundLayer;
    public ContactFilter2D contactFilter;
    public LayerMask camBorderLayer;

	[Header("Raycast Values beforeWall")]
	public float horLength1 = 0.25f;
	public float verLength1 = 0.55f;
	public float horOffset1 = -0.1f;
	public float verOffset1 = -0.2f;

	[Header("Raycast Values grounded")]
	public float horLength2 = 0.25f;
	public float verLength2 = 0.1f;
	public float verOffset2 = -0.5f;

	[Header("Raycast Values beforeGrounded")]
	public float horLength3 = 0.35f;
	public float verLength3 = 0.7f;
	public float verOffset3 = -0.8f;

    [Header("Raycast Values cameraBoxes")]
    public float verLength4 = 3f;
    public float verOffset4 = -0.5f;

	private void Start(){
		playerStats = GetComponent<PlayerStats> ();
    }

    public void Raycasts(){
		topLeft1 = new Vector2(transform.position.x + ((horLength1 - horOffset1) * playerStats.lookDirection), transform.position.y + verLength1 / 2 + verOffset1);
		bottomRight1 = new Vector2(transform.position.x - (horOffset1 * playerStats.lookDirection), transform.position.y - verLength1 / 2 + verOffset1);
		playerStats.beforeWall = Physics2D.OverlapArea(topLeft1, bottomRight1, groundLayer);

		topLeft2 = new Vector2(transform.position.x - horLength2 / 2, transform.position.y + verLength2/ 2 + verOffset2);
		bottomRight2 = new Vector2(transform.position.x + horLength2 / 2, transform.position.y - verLength2 / 2 + verOffset2);
        playerStats.colliderGrounded = Physics2D.OverlapArea(topLeft2, bottomRight2, groundLayer);

		topLeft3 = new Vector2(transform.position.x - horLength3 / 2, transform.position.y + verLength3/ 2 + verOffset3);
		bottomRight3 = new Vector2(transform.position.x + horLength3 / 2, transform.position.y - verLength3 / 2 + verOffset3);
		playerStats.beforeGround = Physics2D.OverlapArea(topLeft3, bottomRight3, groundLayer);

        RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + verOffset4), Vector2.down, verLength4, camBorderLayer);
        if (hitInfo) {
            playerStats.hitsCamBorder = true;
            Collider2D camBorderCollider = hitInfo.collider;
            playerStats.yBorder = camBorderCollider.bounds.max.y;
        }
        else {
            playerStats.hitsCamBorder = false;
            playerStats.yBorder = 0;
        }
	}

	private void OnDrawGizmosSelected(){
		float playerDirection = 1f;
		if (Application.isPlaying) {
			playerDirection = playerStats.lookDirection;
		}
		Vector3 gizmoCenter1 = new Vector3(transform.position.x + ((horLength1 / 2 - horOffset1) * playerDirection), transform.position.y + verOffset1, 0);
		Vector3 gizmoSize1 = new Vector3(horLength1, verLength1, 0);

		Vector3 gizmoCenter2 = new Vector3(transform.position.x, transform.position.y + verOffset2, 0);
		Vector3 gizmoSize2 = new Vector3(horLength2, verLength2, 0);

		Vector3 gizmoCenter3 = new Vector3 (transform.position.x, transform.position.y + verOffset3, 0);
		Vector3 gizmoSize3 = new Vector3(horLength3, verLength3, 0);

		//BeforeWall = orange
		Gizmos.color = new Color(1, 0.5f, 0, 0.5f);
		Gizmos.DrawCube(gizmoCenter1, gizmoSize1);

		//Grounded = green
		Gizmos.color = new Color(0, 1, 0, 0.5f);
		Gizmos.DrawCube(gizmoCenter2, gizmoSize2);

		//BeforeGround = cyan
		Gizmos.color = new Color(0, 1, 1, 0.2f);
		Gizmos.DrawCube(gizmoCenter3, gizmoSize3);
    }

    private void OnDrawGizmos() {
        //HitsCamBorder = purple
        Gizmos.color = new Color(1, 0, 1, 1f);
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + verOffset4, transform.position.z), new Vector3(transform.position.x, transform.position.y + verOffset4 - verLength4, transform.position.z));
        Gizmos.DrawLine(new Vector3(transform.position.x - 0.02f, transform.position.y + verOffset4, transform.position.z), new Vector3(transform.position.x - 0.02f, transform.position.y + verOffset4 - verLength4, transform.position.z));
        Gizmos.DrawLine(new Vector3(transform.position.x + 0.02f, transform.position.y + verOffset4, transform.position.z), new Vector3(transform.position.x + 0.02f, transform.position.y + verOffset4 - verLength4, transform.position.z));
    }

}
