using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Latice_Creator : MonoBehaviour {

	[SerializeField] private GameObject collider_prefab;


	private Mesh mesh;

	private ArrayList particles = new ArrayList();
	private ArrayList collider_parents = new ArrayList();
	private List<int[]> edges = new List<int[]>();
	private int [] triangles;
	private int[] triangles_copy;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		triangles = mesh.triangles;
		triangles_copy = mesh.triangles;
		create_particles();
		create_edges();
		create_colliders();

		for(int i = 0; i < triangles.Length; i++){
			print(triangles_copy[i]);
		}
		
	}

	void create_particles(){
		Vector3 [] vertices = mesh.vertices;
		int count = 0;
		foreach(Vector3 vertex in vertices){
			bool unique =  true;

			Vector3 global_vert = gameObject.transform.TransformPoint(vertex);
			if(particles.Count > 0){
				int i = 0;
				foreach(GameObject particle in particles){
					if(global_vert == particle.transform.position){
						unique = false;
						change_triangle_array(count,i);
						break;
					}
					i++;
				}
			}

			if(unique){
					GameObject p = new GameObject("Point"+ particles.Count);
					p.transform.position = global_vert;
					p.transform.parent = gameObject.transform;
					change_triangle_array(count,particles.Count);
					particles.Add(p); 
					
					//print("added particle to scene" + particles.Count +  "at position" + p.transform.position);
			}
			
			count++;
		}
	}

	void change_triangle_array(int original_index, int new_index){
		for(int i = 0; i < triangles.Length; i++){
			if(triangles[i] == original_index){
				triangles_copy[i] = new_index;
			}
		}
	}

	void create_edges(){	
		for(int i = 0; i < triangles_copy.Length/3; i++){
			int [][] current_triangle = new int[][] {
				new int[]{triangles_copy[(i*3)], triangles_copy[(i*3)+1]},
				new int[]{triangles_copy[(i*3)+1], triangles_copy[(i*3)+2]},
				new int[]{triangles_copy[(i*3)], triangles_copy[(i*3)+2]}
			};

			for(int j = 0; j < 3; j++){
				if(unique_edge(current_triangle[j])){
					edges.Add(current_triangle[j]);
				}
			}
			
		}
	}

	void create_colliders(){
		for(int i = 0; i < edges.Count; i++){
			GameObject rod = Instantiate(collider_prefab, Vector3.zero, Quaternion.identity);
			rod.GetComponent<Capsule_Controller>().point1 = (GameObject)particles[edges[i][0]];
			rod.GetComponent<Capsule_Controller>().point2 = (GameObject)particles[edges[i][1]];
		}
	}

	bool unique_edge(int[] edge){
		bool unique = true; 
		for(int i = 0; i < edges.Count; i++){
			if((edges[i][0] == edge[0] && edges[i][1] == edge[1]) || (edges[i][1] == edge[0] && edges[i][0] == edge[1])){
				unique = false;
				break;
			}
		}
		return unique;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
