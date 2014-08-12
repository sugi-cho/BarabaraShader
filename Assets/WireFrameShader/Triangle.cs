using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triangle
{
	public static Triangle[] GetTriangles (Mesh m)
	{
		Vector3[] vertices = m.vertices;
		int[] triangles = m.triangles;
		
		List<Triangle> tList = new List<Triangle> ();
		
		for (int i = 0; i < triangles.Length/3; i++) {
			int
			vIndex1 = triangles [i * 3],
			vIndex2 = triangles [i * 3 + 1],
			vIndex3 = triangles [i * 3 + 2];
			
			Triangle t = new Triangle (vertices [vIndex1], vertices [vIndex2], vertices [vIndex3]);
			t.mesh = m;
			t.index [0] = vIndex1;
			t.index [1] = vIndex2;
			t.index [2] = vIndex3;
			tList.Add (t);
		}
		return tList.ToArray ();
	}
	
	Vector3 point1, point2, point3;
	
	public float area;
	public Vector3 center, normal;
	public int[] index = new int[3];
	public Mesh mesh;
	
	public Triangle (Vector3 p1, Vector3 p2, Vector3 p3)
	{
		point1 = p1;
		point2 = p2;
		point3 = p3;
		
		Vector3 cross = Vector3.Cross (point2 - point1, point3 - point1);
		area = cross.magnitude / 2f;
		
		center = (point1 + point2 + point3) / 3f;
		normal = cross.normalized;
		
	}
	
	public Vector3 RandomPoint ()
	{
		Vector3
		p1 = Vector3.Lerp (point1, point2, Mathf.Sqrt (Random.value)),
		p2 = Vector3.Lerp (point1, point3, Mathf.Sqrt (Random.value));
		return Vector3.Lerp (p1, p2, Random.value);
	}
}