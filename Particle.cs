using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

	private Vector3 goal_position;
	private double [,] transformation_matrix = new double [,] {{1,0,0},{0,1,0},{0,0,1}};
	private Vector3 v = Vector3.zero;
	private Vector3 force = Vector3.zero;
	private Vector3 position;
	private Vector3 original_position;
	private double stiffness = 1;
	


	// Use this for initialization
	void Start () {
		original_position = gameObject.transform.parent.transform.InverseTransformPoint(gameObject.transform.position);
		position = original_position;
		
	}
	
	// Update is called once per frame
	public void simulate () {
		goal_position = gameObject.transform.position;

		Vector3 new_velocity = v + (gameObject.transform.parent.transform.TransformPoint(goal_position) - position)*(float)(stiffness/Time.deltaTime) + force;
		Vector3 new_position = position + new_velocity*Time.deltaTime;
		
		v = new_velocity;
		position = new_position;


		force = Vector3.zero;
	}

	public void displace(Vector3 displacement){
		position = gameObject.transform.position;
		position += displacement;
	}

	public void movePoints(Vector3 cm0){
		Vector3 x = gameObject.transform.parent.transform.TransformPoint(original_position - cm0);
		gameObject.transform.position = MatrixFunctions.matrix_multiply_1x3_by_3x3(x,transformation_matrix);
	}


	public void set_force(Vector3 f){
		force = f;
	}

	public void set_transformation(double [,] t){
		transformation_matrix = t;
	}

	public Vector3 get_position(){
		return position;
	}
	
	


	
}
