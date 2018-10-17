using UnityEngine;
using System.Collections;

namespace MyNameSpace{
	
	public class tile : MonoBehaviour {
	
	    static GameObject pass_top;
	    static GameObject pass_bottom;
	    static GameObject pass_left;
	    static GameObject pass_right;
		static GameObject pass_end;
	  static GameObject light;
		Transform trans;
	
	
	    static Color[] c = new Color[5];
	    static Material[] m = new Material[5];
	   	static Material[] endPattern = new Material[5];
		static ArrayList validColors = new ArrayList();
	
		int endPatternRep = 0;
	    void Start()
	    {
	        trans = transform;
	      	light = trans.Find("light1").gameObject;
	        pass_top = trans.Find("top").gameObject;
	        pass_bottom = trans.Find("bottom").gameObject;
	        pass_left = trans.Find("left").gameObject;
	        pass_right = trans.Find("right").gameObject;
			pass_end = trans.Find("endTile").gameObject;
			
			endPatternRep = CAM.endRep;
	        setColors();
			if(CAM.halfTiles.Contains(System.Int32.Parse(this.name))){
				pass_end.transform.localScale = new Vector3(5f,3.5f,1f);
				pass_end.transform.Translate(Vector3.up * 0.8f);
				pass_end.renderer.material.SetColor("_Color",Color.black);
			}
			
			if(this.name != CAM.CorridorLength.ToString() 
				&& !CAM.halfTiles.Contains(System.Int32.Parse(this.name))
				&& this.name != (CAM.CorridorLength - 3).ToString()
		//		&& this.name != (6).ToString()
				)
				Destroy(pass_end);
		//	if(System.Int32.Parse(this.name) % 2 == 0)
		    	Destroy(light);
	    }
	
	    void Update()
	    {
	       
	
	    }
	
		public static void setValidColors(ArrayList list){
			validColors.Clear();
			foreach (string s in list){
				validColors.Add(s);
			}
		}
		
		private Color getColor(string s){
			if( s == "black")
				return Color.black;
			else if( s == "blue")
				return Color.blue;
			else if( s == "red")
				return Color.red;
			else if( s == "green")
				return Color.green;
			else if( s == "yellow")
				return Color.yellow;
			else if( s == "purple")
				return Color.magenta;
			return Color.white;
					
		}
	
	    private void makeOneTileGlow() {

		
			int counter = 0;
			for(int i = 0;i<CAM.GlowingTiles.Count;i++)
				if((int)CAM.GlowingTiles[i] == System.Int32.Parse(this.name))
					counter++;
			
	        ArrayList a = new ArrayList();
			int Rand = 0;
			for(int i = 0;i<counter;i++){
				Rand = (new System.Random()).Next(3);
				while(a.Contains(Rand))
					Rand = (new System.Random()).Next(3);
				a.Add(Rand);

				Color ctemp = pass_bottom.renderer.material.color;
				GameObject gotemp = pass_bottom;
				if(Rand == 0)
					gotemp = pass_right;
				else if(Rand == 1)
					gotemp = pass_top;
				else if(Rand == 2)
					gotemp = pass_left;
						
		        gotemp.renderer.material = new Material(Shader.Find("Self-Illumin/Specular"));
		        gotemp.renderer.material.SetColor("_Color",ctemp);
		        gotemp.renderer.material.SetColor("_SpecColor", new Color(255 / 255, (float)240 / 255, (float)215 / 255, 1f));
		        gotemp.renderer.material.SetColor("_ReflectColor", new Color((float)250 / 255, (float)200 / 255, (float)100 / 255, 1f));
			}
			
	        
	
	       
	    }
	
		public bool check(){
			for(int i = CAM.CorridorLength - endPatternRep;i<=CAM.CorridorLength;i++)
				if (this.name == i.ToString())
					return true;
			return false;
		}
			
	    public void setColors() {
		
		    int index = -1;
			int aa = 5;
			int bb = 1;
			Color temp;
			string tempstr;
			ArrayList tempint = new ArrayList();
			System.Random rand = new System.Random();
			Shader shader = Shader.Find("Transparent/Bumped Diffuse");
		
			if(!CAM.noFile)
				if(!check())
			        for(int i = 0; i < 5; i++)
				{
					index = (int) (Random.value * 1000);
					index = index % validColors.Count;
					temp = getColor(validColors[index] as string);
					c[i] = new Color(temp.r,temp.g,temp.b,0.6f);
			        m[i] = new Material(shader);
		            m[i].SetColor("_Color", c[i]);
		        }
				else{
				if(validColors.Count != 1){
				if(this.name == (CAM.CorridorLength - endPatternRep).ToString())
				{
					index = (int) (Random.value * 1000);
					index = index % validColors.Count;
					temp = getColor(validColors[index] as string);
					c[0] = new Color(temp.r,temp.g,temp.b,0.6f);
			       	endPattern[0] = new Material(shader);
				    endPattern[0].SetColor("_Color", c[0]);
					tempstr = validColors[index] as string;
					validColors.Remove(validColors[index]);
					if(CAM.TutorialMode  && CAM.tileSame == 1 && CAM.TutorialCurrentCorr == 2)
						aa = 4;
					else if(CAM.TutorialMode  && CAM.tileSame == 1 && CAM.TutorialCurrentCorr == 3)
						{ bb = 4; aa = 5;}
					else
						aa = 5;

					for(int i = 1; i <= CAM.tileSame; i++){
						int e = rand.Next(bb,aa) ;
							while(tempint.Contains(e))
								e = rand.Next(1,aa) ;
						endPattern[e] = endPattern[0];
						tempint.Add(e);		
					}
					for(int q = 1;q<=4;q++)
						if(!tempint.Contains(q)){
							index = (int) (Random.value * 1000);
							index = index % validColors.Count;
							temp = getColor(validColors[index] as string);
							c[q] = new Color(temp.r,temp.g,temp.b,0.6f);
				            endPattern[q] = new Material(shader);
				            endPattern[q].SetColor("_Color",  c[q]);
						}
					
					validColors.Add(tempstr as string);
							
				}
				m[0] = endPattern[0];
				m[1] = endPattern[1];
				m[2] = endPattern[2];
				m[3] = endPattern[3];
				m[4] = endPattern[4];
				}
				else{
					temp = getColor(validColors[0] as string);
					c[0] = new Color(temp.r,temp.g,temp.b,0.6f);
				    m[0] = new Material(shader);
				    m[0].SetColor("_Color",  c[0]);
					m[1] = m[2] = m[3] = m[4] = m[0];
				}
					
						
				
						
						
				}
			else
			{
				float r, b, g;
		        for(int i = 0; i < 5; i++)
				{
	            	r = Random.value;
				    g = Random.value;
			        b = Random.value;
				    c[i] = new Color(r, g, b, 0.6f);
			        m[i] = new Material(shader);
				    m[i].color = c[i];
		        } 
			}
			
			pass_bottom.renderer.material = m[0];
		    pass_top.renderer.material = m[1];
			pass_right.renderer.material = m[2];
		    pass_left.renderer.material = m[3];
			if(this.name == CAM.CorridorLength.ToString())
				pass_end.renderer.material = m[4];
			if(this.name == (CAM.CorridorLength - 3).ToString() 
				//|| this.name == (6).ToString()
				){
				Color C = new Color(0.2f,0.3f,0.4f,0f);
				Material M = new Material(Shader.Find("Transparent/Specular"));
				M.color = C;
				pass_end.renderer.material = M;	
				pass_end.GetComponent<BoxCollider>().isTrigger = true;
			}
			if(this.name == CAM.Spike.ToString()){
				Color C = new Color(0f,0f,0f,1f);
				Material M = new Material(Shader.Find("Transparent/Specular"));
				M.color = C;
				pass_bottom.renderer.material = M;	
				pass_bottom.GetComponent<BoxCollider>().isTrigger = true;
			}
			if(CAM.GlowingTiles.Contains(System.Int32.Parse(this.name)))
		    	makeOneTileGlow();
		}		
	}
}