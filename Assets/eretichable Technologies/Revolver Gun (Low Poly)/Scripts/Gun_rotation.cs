using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun_rotation : MonoBehaviour
{
	public float RotateSpeed = 25.0f;
	public Transform cylinderRotation;
	public void rotateCylinder() {
		if(cylinderRotation != null) {

			cylinderRotation.Rotate(Vector3.up, RotateSpeed);

		}
	}
}
