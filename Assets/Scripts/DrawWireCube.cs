using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWireCube : MonoBehaviour {

    public Color wireCubeColor;
    [Range (1, 10)]
    public int numberOfLines;

    private void OnDrawGizmos() {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        float xScale = transform.localScale.x;
        float yScale = transform.localScale.y;
		Vector3 gizmoPosition = new Vector3(xPos, yPos, 1);
        Vector3 gizmoSize = new Vector3(xScale, yScale, 1);
        Vector3 gizmoSize2 = new Vector3(xScale - 0.01f, yScale - 0.01f, 1);
        Vector3 gizmoSize3 = new Vector3(xScale - 0.02f, yScale - 0.02f, 1);
        Vector3 gizmoSize4 = new Vector3(xScale - 0.03f, yScale - 0.03f, 1);

        Gizmos.color = wireCubeColor;

        for(int i = 0; i < numberOfLines; i++) {
            Gizmos.DrawWireCube(gizmoPosition, new Vector3((gizmoSize.x - (i * 0.02f)), (gizmoSize.y - (i * 0.02f)), 1f));
        }
    }
}
