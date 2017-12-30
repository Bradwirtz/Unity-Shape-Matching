using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule_Controller : MonoBehaviour {

	[SerializeField] private float radius;
	[SerializeField] public GameObject point1;
	[SerializeField] public GameObject point2;

	private CapsuleCollider collider;

	private Vector3 point1_prev;
	private Vector3 point2_prev;

	private Vector3 center;
	private float length;



	// Use this for initialization
	void Start () {
		collider = GetComponent<CapsuleCollider>();
		collider.radius = radius;
		update_position();
		point1_prev = collider.transform.InverseTransformPoint(point1.transform.position);
		point2_prev = collider.transform.InverseTransformPoint(point2.transform.position);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(collider.transform.InverseTransformPoint(point1.transform.position) != point1_prev || collider.transform.InverseTransformPoint(point2.transform.position) != point2_prev){
			update_position();
			point1_prev = collider.transform.InverseTransformPoint(point1.transform.position);
			point2_prev = collider.transform.InverseTransformPoint(point2.transform.position);
		}
	}

	void update_position(){
		center = (point1.transform.position + point2.transform.position)/2;
		gameObject.transform.position = center;
		rotate_capsule();
		collider.height = (point1.transform.position - point2.transform.position).magnitude;
	}

	void rotate_capsule(){
		Vector3 new_rotation_vector = point1.transform.position - point2.transform.position;
		gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, new_rotation_vector);
	}

	void set_point_1(GameObject point){ point1 = point;}
	GameObject get_point1(){return point1;}
	void set_point_2(GameObject point){ point2 = point;}
	GameObject get_point2(){return point2;}
}
