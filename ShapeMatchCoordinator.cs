using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMatchCoordinator : MonoBehaviour {

	[SerializeField] Cluster cluster;
	[SerializeField] GameObject particle_container;
	[SerializeField] int frames;
	[SerializeField] float minForce = 0;
	private Particle [] particles;
	private Vector3 [] displacements;
	private Mesh mesh;
	private Vector3 [] vertices;
	private Vector3 [] original_vertices;
	private Rigidbody rigidbody;
	private Vector3 prev_velocity;

	
	private int deformation_frames;

	// Use this for initialization
	void Start () {
		deformation_frames = 0;
		particles = particle_container.GetComponentsInChildren<Particle>();
		displacements = new Vector3 [particles.Length];
		mesh = gameObject.GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		original_vertices = (Vector3 [])vertices.Clone();
		for(int i = 0; i < original_vertices.Length; i++){
			original_vertices[i] = particle_container.transform.InverseTransformPoint(original_vertices[i]);
		}

		rigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(deformation_frames > 0){

			for(int i = 0; i < particles.Length; i++){
				particles[i].displace(displacements[i]);
				displacements[i] /= 4;
			}
			cluster.recalculate();

			for(int i = 0; i < particles.Length; i++){
				particles[i].movePoints(cluster.origin_cm);
			}

			for(int i = 0; i < vertices.Length; i++){
				Vector3 x = particle_container.transform.TransformPoint(original_vertices[i] - cluster.origin_cm);
				vertices[i] = MatrixFunctions.matrix_multiply_1x3_by_3x3(x,cluster.getTransformation());
			}

			gameObject.transform.position = cluster.cm;

			rigidbody.centerOfMass = cluster.cm;
			mesh.vertices = vertices;

			deformation_frames--;
		}

		prev_velocity = rigidbody.velocity;

		
	}

	
	void OnCollisionEnter (Collision collision){
		if(collision.impulse.magnitude > minForce){
			foreach (ContactPoint contact in collision.contacts) {
				for(int i = 0; i < particles.Length; i++){
					float d = ((particles[i].transform.position - contact.point).magnitude);
					Vector3 displacement = collision.impulse/(float)(1 + (d * d * d)) + (prev_velocity * Time.fixedDeltaTime)/2;
					displacements[i] = displacement;
				}	
			}

			deformation_frames = frames;
		}
		
	}
	
}
