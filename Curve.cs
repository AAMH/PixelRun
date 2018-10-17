using System;
using System.Collections;

namespace MyNameSpace
{
	public class Curve
	{
		
		private int id;
		public ArrayList curve = new ArrayList();
		public float acc = 0.0f;
		
		public Curve (int b,ArrayList list, float a)
		{
			this.id = b;
			for(int i = 0;i<list.Count;i++)
				curve.Add(list[i]);
			this.acc = a;
		}
		
		public int getID(){
			return this.id;
		}
		
	}
}

