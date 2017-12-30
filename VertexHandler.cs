using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexHandler : MonoBehaviour {

	[SerializeField] private Cluster cluster;
	private Mesh mesh;
	private Vector3 [] vertices;

	// Use this for initialization
	
	
	// Update is called once per frame
	void Start () {
		mesh = gameObject.GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
	}
	public void updatePositions () {
		double [,] t = cluster.getTransformation();
		for(int i = 0; i < vertices.Length; i++){
			vertices[i] = MatrixFunctions.matrix_multiply_1x3_by_3x3(vertices[i],t);
		}

		mesh.vertices = vertices;
	}
}
