using UnityEngine;
using System.Collections;

namespace MyNameSpace{
	public class Orb : MonoBehaviour {
	
		Transform trans;
		public static string dir;
		public static int state;
		public static float speed ;
		public static float speedThr ;
		public static float speedThrL ;

		int counter = 0;
	//	float angel = 0;
	//	int sign = 1;
		
		void Start () {
			trans = transform;
			state = 1;
			speed = CAM.RunnerSpeed;
			speedThr = CAM.RunnerSpeed;
			speedThrL = speedThr / 4;
		}
		
		void Update () {
		//	print(speed);
			switch(state){
			case 1:
			//	counter1++;
			//	if(counter1 == 1)
					
		//		trans.Rotate(0,-angel,0);
				trans.Translate(Vector3.forward * speed*2f );
		//		trans.Translate(Vector3.left * speed / 4*sign);
				
		//		if(counter == 60){
		//			sign = -sign;
		//			counter = 0;
		//		}
		//		angel += 10f;
		//		trans.Rotate(0,angel,0);
				
				break;
			case 2:
				counter++;
				if(counter == 72){
					state = 1;
					counter = 0;
					if(dir == "up"){
						trans.Rotate(90,0,0);
						trans.Translate(Vector3.up * -6.7f);
					}

				}

					trans.Translate(Vector3.forward * 0.3f );
				
				if(dir == "right")
					trans.Rotate(0,1.25f,0);
				else if(dir == "left")
					trans.Rotate(0,-1.25f,0);
				
				if(dir == "up")
					trans.Rotate(-1.25f,0,0);
				
				break;
			case 3:
				
				break;
			}
			
		}
		
		private void direct(string s){
			dir = s;
			if(dir != "endTile")
				state = 2;
			else
				state = 1;
		//	print(dir);
		}
		
		void OnTriggerEnter(Collider collider){
			if(collider.transform.parent.name == (CAM.CorridorLength - 3).ToString())
				direct(CAM.Pilot());	
		}
	}
}