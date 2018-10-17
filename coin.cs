using UnityEngine;
using System.Collections;

public class coin : MonoBehaviour {

	
	Transform tr;
	//int RandInt = 0;
	System.Random R;
	
	void Start () {
		tr = this.transform;
		R = new System.Random();
	}
	
	
	void Update () {
		tr.Rotate(new Vector3(R.Next(1,6),0,0));
	}
}
