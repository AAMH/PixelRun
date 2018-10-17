using UnityEngine;
using System.Collections;

public class wayLight : MonoBehaviour {
	
	Transform tr;
	// Use this for initialization
	void Start () {
		tr = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		tr.Rotate(new Vector3(0,5,0));
	}
}
