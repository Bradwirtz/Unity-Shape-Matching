using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsV2 : MonoBehaviour {

	[SerializeField] private GameObject particles_container;
	[SerializeField] private double dampen;
	[SerializeField] private int cooldown_frames;


	private Vector3 [] original_positions;
	private Vector3 [] new_positions;
	private Rigidbody rigidbody;
	private Transform [] particle_transforms;
	private Particle [] particles;
	private int cooldown_remaining = 0;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		particles = particles_container.GetComponentsInChildren<Particle>();
	}

	void OnCollisionEnter (Collision collision){
		if(cooldown_remaining > 0){
			foreach (ContactPoint contact in collision.contacts) {
				for(int i = 0; i < particles.Length; i++){
					float d = ((particles[i].transform.position - contact.point).magnitude);
					Vector3 force = MatrixFunctions.vector_scalar(collision.impulse,Time.fixedDeltaTime*(1 +  (d * d * d)));
					particles[i].set_force(force);
				}	
			}
			cooldown_remaining = cooldown_frames;
		}
		else{
			cooldown_remaining--;
		}
	
	}

}
