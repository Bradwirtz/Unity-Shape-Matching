using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class MatrixFunctions:MonoBehaviour {

	static int info;
    static alglib.matinvreport rep;

	static void matrixSquareRoot3x3(ref double [,] a){
		//a = new double [,] {{4,0,0}, {0,4,0},{0,0,0}};
		//Rotation_test.print_matrix(a);
		double [] eigenvalues;
		double [,] eigenvectors;

		alglib.smatrixevd(a,3,1,false,out eigenvalues,out eigenvectors);

		// print("Eigenvalues: ");
		// for(int i = 0; i < eigenvalues.Length; i++){
		// 	print(i + ":  " + eigenvalues[i]);
		// }

		double [] squares = new double [eigenvalues.Length];
		for(int i = 0; i < eigenvalues.Length; i++){
			squares[i] = Mathf.Sqrt((float)eigenvalues[i]);
		}

		double [,] ev = identity(squares);
		//Rotation_test.print_matrix(ev);
		double [,] t = transpose3x3(eigenvectors);
		a = matrixMultiply3x3(matrixMultiply3x3(eigenvectors,ev),t);
		
	} 

	public static double [,] vector3_covariance(Vector3 a, Vector3 b){
		//CHECKED
		double [,] result = new double [,]{
			{a[0] * b[0], a[0] * b[1], a[0]* b[2]},
			{a[1] * b[0], a[1] * b[1], a[1]* b[2]},
			{a[2] * b[0], a[2] * b[1], a[2]* b[2]}
		};

		return result;
	}

	public static void matrix_add_3x3(ref double [,] a, double [,] b){
		//CHECKED
		for(int i = 0; i < a.GetLength(0); i++){
			for(int j = 0; j < a.GetLength(1); j++){
				a[i,j] += b[i,j];
			}
		}
	}


	public static void matrixInverse3x3(ref double [,] a){
		alglib.rmatrixinverse(ref a, out info, out rep);
		
	
	}

	public static Vector3 vector_scalar(Vector3 v, double s){
		v.x *= (float)s;
		v.y *= (float)s;
		v.z *= (float)s;

		return v;
	}

	private void inverseHelper(){

	}

	public static double[,] matrixInverse3x3(double [,] a){
		
		alglib.rmatrixinverse(ref a, out info, out rep);
		return a;
	}

	public static void matrixInverseSquareRoot(ref double [,] a){
		matrixSquareRoot3x3(ref a);
		//Rotation_test.print_matrix(a);
		//Square root of matrix is correct, but the matrix achieved is non invertible
		matrixInverse3x3(ref a);
	}

	public static void scalar_multiply(ref double [,] a, double scalar){
		for(int i = 0; i < a.GetLength(0); i++){
			for(int j = 0; j < a.GetLength(1); j++){
				a[i,j] *= scalar;
			}
		}
	}
	

	public static double [,] matrixMultiply3x3(double [,] a, double [,] b){
		//CHECKED
		double [,] result = new double[,]{
			{a[0,0]*b[0,0] + a[0,1]*b[1,0] + a[0,2]* b[2,0],
			a[0,0]*b[0,1] + a[0,1]*b[1,1] + a[0,2]* b[2,1],
			a[0,0]*b[0,2] + a[0,1]*b[1,2] + a[0,2]* b[2,2]},

			{a[1,0]*b[0,0] + a[1,1]*b[1,0] + a[1,2]* b[2,0],
			a[1,0]*b[0,1] + a[1,1]*b[1,1] + a[1,2]* b[2,1],
			a[1,0]*b[0,2] + a[1,1]*b[1,2] + a[1,2]* b[2,2]},

			{a[2,0]*b[0,0] + a[2,1]*b[1,0] + a[2,2]* b[2,0],
			a[2,0]*b[0,1] + a[2,1]*b[1,1] + a[2,2]* b[2,1],
			a[2,0]*b[0,2] + a[2,1]*b[1,2] + a[2,2]* b[2,2]},
		};
	
		return result;
	}

	public static double [,] transpose3x3(double[,] a){
		//CHECKED
		double [,] result = new double[,]{
			{a[0,0], a[1,0], a[2,0]},
			{a[0,1], a[1,1], a[2,1]},
			{a[0,2], a[1,2], a[2,2]}
		};

		return result;
	}

	public static double [,] identity(double[] a){
		for(int i = 0; i < a.Length; i++){
			if(Double.IsNaN(a[i])) a[i] = 0;
		}
		double [,] result = new double [,]{
			{a[0],0,0},
			{0,a[1],0},
			{0,0,a[2]}
		};

		return result;
	}

	public static Vector3 matrix_multiply_1x3_by_3x3(Vector3 v, double [,] a){
		//FIXED
		Vector3 result = new Vector3(
			(float)(v.x *  a[0,0] + v.y * a[0,1] + v.z * a[0,2]),
			(float)(v.x *  a[1,0] + v.y * a[1,1] + v.z * a[1,2]),
			(float)(v.x *  a[2,0] + v.y * a[2,1] + v.z * a[2,2])
		);

		return result;
	}

}
