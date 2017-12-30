using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation_test : MonoBehaviour {

	[SerializeField] GameObject marker;
	[SerializeField] GameObject duplicate;

	[SerializeField] float randomness;
	[SerializeField] float angle;

	[SerializeField] bool linear = false;
	[SerializeField] double beta;
	
	private ShapeMatching shape_match; 

	private Vector3[] vertices;
	private ArrayList particles = new ArrayList();
	private GameObject [] markers;

	// Use this for initialization
	void Start () {
		vertices = gameObject.GetComponent<MeshFilter>().mesh.vertices;
		create_particles();
		shape_match = new ShapeMatching(duplicate);
	}
	
	// Update is called once per frame
	void OnMouseDown(){
		run_test();
	}

	public void flipNormals(){
		Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
		Vector3[] normals = mesh.normals;
			for (int i=0;i<normals.Length;i++)
				normals[i] = -normals[i];
			mesh.normals = normals;
 
			for (int m=0;m<mesh.subMeshCount;m++)
			{
				int[] triangles = mesh.GetTriangles(m);
				for (int i=0;i<triangles.Length;i+=3)
				{
					int hemp = triangles[i + 0];
					triangles[i + 0] = triangles[i + 1];
					triangles[i + 1] = hemp;
				}
				mesh.SetTriangles(triangles, m);
			}
	}

	public static void print_matrix(double [,] a){
		for(int i = 0; i < a.GetLength(0); i++){
			for(int j = 0; j < a.GetLength(0); j++){
				print(a[i,j]);
			}
			print("\n");
		}
	}

	public static void print_vector(Vector3 v){
		for(int i = 0; i < 3; i++){
			print(v[i]);
		}
	}

	public static void print_string(string s){
		print(s);
	}

	private void destroy_markers(){
		if(markers != null){
			for(int i = 0; i < markers.Length; i++){
				Destroy(markers[i]);
			}
		}
		
	}

	void run_test(){
		//randomize rotation of control spheres
		Quaternion rot = Random.rotation;
		print(rot.eulerAngles);
		duplicate.transform.rotation = rot;

		//Stores positions of particles post rotation
		Vector3 [] new_positions;
		Transform [] temp = duplicate.GetComponentsInChildren<Transform>(); //Get components in children creates unwanted first element in array, needs compensation
		new_positions = new Vector3[temp.Length - 1];

		//Instantiates spheres at particle locations 
		destroy_markers();
		markers = new GameObject[temp.Length - 1];
		for(int i = 1; i < temp.Length; i++){ //i starts at one to compensate for problem descibed earlier
			new_positions[i - 1] =  duplicate.transform.TransformPoint(temp[i].position) + new Vector3(Random.Range(-randomness, randomness), Random.Range(-randomness, randomness), Random.Range(-randomness, randomness));;
			GameObject t = Instantiate(marker, new_positions[i - 1], Quaternion.identity);
			t.transform.parent = duplicate.transform; //Parents new objects under Duplicate_Points 
			
		}

		//ENTERY INTO SHAPE MATCH
		double [,] rotation_matrix;
		if (linear){
			rotation_matrix = shape_match.generate_linear_matrix(new_positions,beta);
		}
		else{
			rotation_matrix = shape_match.generate_rotation_matrix(new_positions);
		}
		
		//print_matrix(rotation_matrix);

		//Multiplies rotation matrix recieved by shape matching to mesh's vertices
		Vector3 [] new_vertices = new Vector3 [vertices.Length];

		for(int i = 0; i < vertices.Length; i++){
			new_vertices[i] = MatrixFunctions.matrix_multiply_1x3_by_3x3(vertices[i], rotation_matrix);
		}

		//Assigns altered vertice positions to object
		Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
		mesh.vertices = new_vertices;
		// flipNormals();
		mesh.RecalculateNormals();
		
	}


	void create_particles(){
		//CHECKED
		//This method called on start of program, takes vertices of referenced mesh and repeats duplicates to create control particles for shape matching engine
		int count = 0;
		foreach(Vector3 vertex in vertices){
			bool unique =  true;

			Vector3 global_vert = gameObject.transform.TransformPoint(vertex);
			if(particles.Count > 0){
				int i = 0;
				foreach(GameObject particle in particles){
					if(global_vert == particle.transform.position){
						unique = false;
						break;
					}
					i++;
				}
			}

			if(unique){
					GameObject p = new GameObject("Point "+ particles.Count);
					p.transform.position = global_vert;
					p.transform.parent = duplicate.transform;
					particles.Add(p); 
					
					//print("added particle " + particles.Count +  " to scene at position " + p.transform.position);
			}
			
			count++;
		}
	}
}
