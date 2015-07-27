using UnityEngine;
using System.Collections;


[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshFilter))]
public class Stage : MonoBehaviour
{
	Mesh mesh;
	MeshFilter meshFilter;
	
	Vector3[] vertices;
	int[] triangles;
	Vector2[] uvs;
	
	// Use this for initialization
	void Start ()
	{
		mesh = new Mesh();
		meshFilter = (MeshFilter)GetComponent("MeshFilter");
	}
	
	// Update is called once per frame
	void Update ()
	{
		mesh.Clear();
		
		vertices = new Vector3[4];
		vertices[0] = new Vector3 (0, 0, 0);	// 左上
		vertices[1] = new Vector3 (0, -1, 0);	// 左下
		vertices[2] = new Vector3 (1, -1, 0);	// 右下
		vertices[3] = new Vector3 (1, 0, 0);	// 右上
		
		triangles = new int[6];
		triangles[0] = 0;
		triangles[1] = 2;
		triangles[2] = 1;
		triangles[3] = 0;
		triangles[4] = 3;
		triangles[5] = 2;
		
		uvs = new Vector2[4];
		uvs[0] = new Vector2 (0, 0);
		uvs[1] = new Vector2 (1, 1);
		uvs[2] = new Vector2 (0, 1);
		uvs[3] = new Vector2 (1, 0);
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		//mesh.Optimize();
		
		meshFilter.sharedMesh = mesh;
		meshFilter.sharedMesh.name = "StageMesh";
	}
}