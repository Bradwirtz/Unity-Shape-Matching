using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Deformer : MonoBehaviour {
	[SerializeField] private float dampen;
	[SerializeField] private float min_force;
	[SerializeField] private int cooldown_frames;
	[SerializeField] private GameObject particles_container;


	private GameObject [] particles;
	private Vector3 [] new_positions;
	private Rigidbody rigidbody;
	private int cooldown;
	private Vector3 prev_velocity;
	public bool updated = false;


	void OnCollisionEnter (Collision collision){

		if(collision.impulse.magnitude > min_force && cooldown == 0){

			updated = true;

			foreach (ContactPoint contact in collision.contacts) {

					for(int i = 0; i < particles.Length; i++){
						float d = (distance(contact.point , particles[i].transform.position).magnitude);
						Vector3 displacement = collision.impulse/((1 + d * d * d) * dampen * collision.contacts.Length) + (prev_velocity * Time.deltaTime)/2;
						new_positions[i] += displacement;
						
					}
			
			}

			cooldown = cooldown_frames;
			

		}
	}

	Vector3 distance(Vector3 collision, Vector3 point){
		return point - collision;
	}



	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		Transform [] temp = particles_container.GetComponentsInChildren<Transform>();
		new_positions = new Vector3[temp.Length];
		particles = new GameObject[temp.Length];
		for(int i = 0; i < temp.Length; i++){
			particles[i] = temp[i].parent.gameObject;
			new_positions[i] = temp[i].position;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(cooldown > 0) cooldown--;
		prev_velocity = rigidbody.velocity;
	}

	void LateUpdate(){
		if(updated == true) updated = false; 
	}

}
