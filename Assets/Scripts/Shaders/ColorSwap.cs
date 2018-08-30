using UnityEngine;

[ExecuteInEditMode]
public class ColorSwap : MonoBehaviour {

	public Color[] swapColors = new Color[10];
	private Material mat;

	void Start () {
		mat = GetComponent<Renderer> ().sharedMaterial;
		mat.SetVectorArray ("_ColorMatrix", ColorMatrix ());
	}

	void Update () {
		if(!Application.isPlaying)
			mat.SetVectorArray ("_ColorMatrix", ColorMatrix ());
	}

	Vector4[] ColorMatrix(){
        Debug.Log("fill color matrix");
		Vector4[] matrix = new Vector4[swapColors.Length];
		for (int i = 0; i < swapColors.Length; i++) {
			matrix[i] = ColorToVec(swapColors[i]); 
		}

		return matrix;
	}

	Vector4 ColorToVec(Color c){
		return new Vector4 (c.r, c.g, c.b, c.a);
	}
}
