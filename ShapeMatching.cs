using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Things to check:
//Locations of new and original points before entry into shape matching, possible problem with using local/global positions in inappropriate locations


public class ShapeMatching{

	// Use this for initialization
	private GameObject particles;

	private Vector3 [] original_positions;
	private Vector3 cm0 = Vector3.zero;
	private Vector3 cm = Vector3.zero;
	public double [,] aqq;

	public double [,] apq;
	public double [,] s;

	public ShapeMatching(GameObject points){
		//Instantiation method for shapematching object

		//Stores duplicates of points passed in to object as an array of original positions to be used in calculation
		particles = points;
		Transform [] original_transforms = particles.GetComponentsInChildren<Transform>();
		original_positions = new Vector3[original_transforms.Length - 1];
		for(int i = 1; i < original_transforms.Length; i++){ //i starts at one in order to avoid origin being added to positions
			original_positions[i - 1] = particles.transform.InverseTransformPoint(original_transforms[i].position);
			cm0 += original_positions[i - 1];
		}
		cm0 /= original_positions.Length;

		//Precalculates Aqq from original positions
		aqq = new double [,] {{0,0,0},{0,0,0},{0,0,0}};
		for(int i = 0; i < original_positions.Length; i++){
			original_positions[i] -= cm0;
			MatrixFunctions.matrix_add_3x3(ref aqq, MatrixFunctions.vector3_covariance(original_positions[i],original_positions[i]));
		}
		

		//performs inverse operation on Aqq matrix as stated in Muller paper
		MatrixFunctions.matrixInverse3x3(ref aqq);
	}


	public double [,] generate_rotation_matrix(Vector3 [] new_positions){
		//3x3 matrix of zeros used for summation
		apq = new double [,] {{0,0,0},{0,0,0},{0,0,0}};
		for(int i = 0; i < original_positions.Length; i++){
			MatrixFunctions.matrix_add_3x3(ref apq, MatrixFunctions.vector3_covariance(new_positions[i],original_positions[i]));
			//CHECKED: new positions are different from old ones, seem consitent
		}
		//CHECKED THROUGH THIS POINT
		s = MatrixFunctions.matrixMultiply3x3(MatrixFunctions.transpose3x3(apq), apq);
	
		MatrixFunctions.matrixInverseSquareRoot(ref s);
	
		double [,] r = MatrixFunctions.matrixMultiply3x3(apq,s);
		
		return r;

	}

	public void abs(ref double[,] a){
		for(int i = 0; i < 3; i++){
			for(int j = 0; j < 3; j++){
				if(a[i,j] < 0){
					a[i,j] *= -1;
				}
			}
		}
	}

	public double [,] generate_linear_matrix(Vector3 [] new_positions, double beta){
		double [,] r = generate_rotation_matrix(new_positions);
		double [,] a = MatrixFunctions.matrixMultiply3x3(apq, aqq);
		//Rotation_test.print_matrix(aqq);
		
		
		double determinant = alglib.rmatrixdet(a);
		determinant = Mathf.Pow((float)determinant,1.0f/3.0f);
		MatrixFunctions.scalar_multiply(ref a,beta/determinant);
		MatrixFunctions.scalar_multiply(ref r, 1 - beta);

		MatrixFunctions.matrix_add_3x3(ref a,r);

		return a;
	}

}
