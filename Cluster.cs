using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster : MonoBehaviour {

	[SerializeField] private GameObject paritcle_container;
	private Particle[] particles;
	private ShapeMatching shapeMatch;
	private double [,] transformation = new double [,] {{1,0,0},{0,1,0},{0,0,1}};
	public Vector3 origin_cm = Vector3.zero;
	public Vector3 cm = Vector3.zero;
	

	// Use this for initialization
	void Start () {
		particles = paritcle_container.GetComponentsInChildren<Particle>();
		shapeMatch = new ShapeMatching(paritcle_container);
		for(int i = 0; i < particles.Length; i++){
			origin_cm += paritcle_container.transform.InverseTransformPoint(particles[i].get_position());
		}
		origin_cm /= (float)particles.Length;
	}
	
	// Update is called once per frame
	public void recalculate () {
		Vector3 [] new_positions = new Vector3[particles.Length];
		cm = Vector3.zero;
		for(int i = 0; i < particles.Length; i++){	
			new_positions[i] = particles[i].get_position();
			cm += new_positions[i];
			//print(particles[i].get_position());
		}
		cm /= (float)particles.Length;
		// foreach(Vector3 p in new_positions){
		// 	print(p);
		// }
		for(int i = 0; i < new_positions.Length; i++){
			new_positions[i] -= cm;
		}

		transformation = shapeMatch.generate_linear_matrix(new_positions, 0.95);
		//Rotation_test.print_matrix(transformation);
		foreach(Particle particle in particles){
			particle.set_transformation(transformation);
		}

		
	}

	public double [,] getTransformation(){
		return transformation;
	}
}
