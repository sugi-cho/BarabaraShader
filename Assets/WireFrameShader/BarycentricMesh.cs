using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BarycentricMesh : MonoBehaviour
{
	#if UNITY_EDITOR
	[MenuItem("Custom/Create/Barycentric Mesh")]
	public static void CreateFromMenu ()
	{
		foreach (Mesh m in Selection.objects) {
			if (m != null) {
				Mesh newM = (Mesh)Instantiate (m);
				ConvertMesh (newM);
				AssetDatabase.CreateAsset (newM, "Assets/" + m.name + "_wire.asset");
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
			}
		}
		if (Selection.activeGameObject.GetComponent<MeshFilter> () != null) {
			Mesh newM = (Mesh)Instantiate (Selection.activeGameObject.GetComponent<MeshFilter> ().mesh);
			ConvertMesh (newM);
			AssetDatabase.CreateAsset (newM, "Assets/" + newM.name + "_wire.asset");
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
		}
	}
	#endif
	
	static void ConvertMesh (Mesh mesh)
	{
		var vertices0 = mesh.vertices;
		var triangles0 = mesh.triangles;
		var uv0 = mesh.uv;
		var vertices1 = new Vector3[triangles0.Length];
		var triangles1 = new int[triangles0.Length];
		var uv1 = new Vector2[triangles0.Length];

		for (var i = 0; i < triangles0.Length; i++) {
			var index = triangles0 [i];
			vertices1 [i] = vertices0 [index];
			uv1 [i] = uv0 [index];
			triangles1 [i] = i;
		}
		mesh.vertices = vertices1;
		mesh.triangles = triangles1;
		mesh.uv = uv1;

		
		var bary = new Color[vertices1.Length];
		var counter = 0;
		for (var i = 0; i < triangles0.Length; i+=3) {
			bary [counter++] = new Color (1, 0, 0, 1);
			bary [counter++] = new Color (0, 1, 0, 1);
			bary [counter++] = new Color (0, 0, 1, 1);
		}
		mesh.colors = bary;
		
		var normals0 = mesh.normals;
		if (normals0.Length > 0) {
			var normals1 = new Vector3[vertices1.Length];
			for (var i = 0; i < triangles0.Length; i++)
				normals1 [i] = normals0 [triangles0 [i]];
			mesh.normals = normals1;
		}
		
		var boneWeights0 = mesh.boneWeights;
		if (boneWeights0.Length > 0) {
			var boneWeights1 = new BoneWeight[triangles0.Length];
			for (var i = 0; i < triangles0.Length; i++)
				boneWeights1 [i] = boneWeights0 [triangles0 [i]];
			mesh.boneWeights = boneWeights1;
		}

		var tangents1 = new Vector4[vertices1.Length];
		for (var i = 0; i < triangles0.Length; i+=3) {
			Triangle tr = new Triangle (vertices1 [i], vertices1 [i + 1], vertices1 [i + 2]);
			tangents1 [i] = tr.center;
			tangents1 [i + 1] = tr.center;
			tangents1 [i + 2] = tr.center;
		}
		mesh.tangents = tangents1;

	}

	void Start ()
	{
		var mf = GetComponent<MeshFilter> ();
		if (mf != null)
			ConvertMesh (mf.mesh);
	}

}
