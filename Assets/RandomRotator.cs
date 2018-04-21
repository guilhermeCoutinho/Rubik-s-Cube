using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {
	// Update is called once per frame
	public float speed;
	public Vector3 rotationFactor;
	Vector3 rotation;
	void Update () {
		float sin = Mathf.Sin (Time.time);
		rotation = sin * rotationFactor;
		transform.Rotate (rotation * speed * Time.deltaTime);
	}
}
