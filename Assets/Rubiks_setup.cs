using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubiks_setup : MonoBehaviour {

	// Use this for initialization; Ok
	void Start () {
	    for (int i = 0; i < transform.childCount; i++)
        {
            int x = i / 9;
            int y = (i - x * 9) / 3;
            int z = i % 3;
            x -= 1;
            y--;
            z--;
            Transform t = transform.GetChild(i);
            t.position = new Vector3(x * 2, y*2, z*2);
            t.name = "Cube "+i+":\t" + x + "," + y + "," + z;
        }	
	}
}
