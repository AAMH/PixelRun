using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace MyNameSpace{
	
	public class CAM : MonoBehaviour {
		
		
		Transform trans;
		Vector3 OriginalPosition;
		Quaternion OriginalRotation;
		Quaternion tempQuat;
		Quaternion tempQuatt;
		Stopwatch w;
		Stopwatch ww;
		Vector3 pos;
		Vector3 oldAngle;
		Vector3 tempPos;
		static String lastKeyPressed;
		static bool anyKeyPressed;
		static bool piloted;
		float distance;
		int upOrDown;
		int movingDirection;
		/// directions
		///  + z : 1   - Z : -1
		///  + x : 2   - x : -2
		
		public GameObject tiles;
		public GameObject orbPrefab;
		public GameObject controlItemPrefab;
		public GameObject wayLightPrefab;
		public GameObject coinPrefab;
		public GameObject CharacterPrefab;
		
		GameObject g;
		static GameObject pg;
		int i1, i2;
		int z1 = 0;
		int z2 = 200;
		int counter;
		int LCounter;
		int Hcounter;
		int state;
		/// 0 : start
		/// 1 : moving
		/// 2 : getComamnd
		/// 3 : rotate
		
		int LosingState;
		int qwerty = 0;
		int[] B_index =  new int[10];
		int[] B_show_index = new int[10];
		private ArrayList numbersImg = new ArrayList(10);
		private ArrayList buttonsImg = new ArrayList(20);
		private Rect[] buttonsPos = new Rect[20];
		private ArrayList groups = new ArrayList();	
		private ArrayList[] ExistingCorridor = new ArrayList[2] { new ArrayList(),new ArrayList() };
		private ArrayList curves = new ArrayList();
		private ArrayList curveSeq = new ArrayList();
		private static ArrayList AutoPilotResluts = new ArrayList();
		private Group BonusGroup;
		private ArrayList BonusGroups = new ArrayList();
		private ArrayList TutorialGroups = new ArrayList();
		public static ArrayList GlowingTiles = new ArrayList();
		public static ArrayList halfTiles = new ArrayList();
		private ArrayList Coins = new ArrayList();
		private ArrayList CoinsObjects = new ArrayList();
		public static int Spike = -1;
		
		private Curve currentCurve;
		private Group currentGroup;
		private int CurrentGroupIndex = -1;
		private int CurrentCurveIndex = 0;
		
		private int runningMode;
		
		private int collectedCoins = 0;
		private Int32 PlayerScore = 0;
		private Int32 HighScore = 0;
		
		private static float tileSize = 2.5f;
		public static float RunnerSpeed = 0.15f;
		
		public static int CorridorLength = 21;
		public static int tileSame = 0;
		public static int endRep = 0;
		
		public static bool noFile;
		string txt = " ";
		
		Boolean gameOver = false;
		Boolean GameIsPaused = false;
		static Boolean AutoPilot = false;
		Boolean WayLightMode = false;
		Boolean WayLight = false;
		int wayLightDur = 0;
		int autoPilotDur = 0;
		
		
		static GameObject currentOrb;
		static GameObject currentControlItem;
		static GameObject currentWayLight;
		public static float OrbDistance = 5;
		float xx = 0;
		string lastGroup = " ";
		string creditText = " ";
		string creditText2 = " ";
		
		private bool shouldSlide = false;
		private bool shouldGoGlow = false;
		private bool Shaking = false; 
		private float ShakeDecay;
		private float ShakeIntensity;   
		private Vector3 OriginalPos;
		private Quaternion OriginalRot;
		
		private float Xbound = 0;
		private float YBound = 0;
		private float Xbound2 = 0;
		private float YBound2 = 0;
		Color guiC;
		
		int tempLS = 0;
		public static int TutorialCurrentCorr = 0;
		int StartState = -8;
		int TutMsgX = 0;
		int TutMsgY = Screen.height * 4 / 6;
		string TutMsg = "";

		float SoundVolume = 1f;
		float MusicVolume = 1f;
		float tempSliderValue = 0.0f;
		float tempSliderValue2 = 0.0f;

		private bool swiping = false;
		private Vector2 lastPosition;

		private bool highScoreSet = false;
		public static bool TutorialMode = false;
		private bool TutCorrPassed = false;
		private bool showGLmsg = false;

		private ArrayList splashImg = new ArrayList(30);
		private Texture2D StoryTex;
		private Texture2D StartTex;
		private Texture2D CreditTex;
		private Texture2D background;
		private Texture2D tikkTexture;

		Font font;

		System.Random SoundRandom = new System.Random();

		public AudioClip RunMusic;
		public AudioClip MenuMusic;
		public AudioClip[] SFXs = new AudioClip[5];
		public AudioClip[] FootStepSFXs = new AudioClip[8];
		public AudioClip[] HumanVoiceSFXs = new AudioClip[7];

		public AudioSource SFX;
		public GUISkin MySkin;


		private static readonly Dictionary<String, String> REVMOB_APP_IDS = new Dictionary<String, String>() {
			{ "Android", "53ee50693ed4a2d406cab21f"},
			{ "IOS", "53ee56946c19a99b47cf3c4e" }
		};
		private RevMob revmob;
		
		void Awake() {
			revmob = RevMob.Start(REVMOB_APP_IDS, "Main Camera");
		}


		void OnGUI() {
		
		//	GUI.skin.label.wordWrap = true;
			string s = "Safe Mode!";
			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.black;
			style.fontStyle = FontStyle.Italic;
			style.wordWrap = true;
			style.font = font;
			GUI.skin.label = style;

			/*	if(!noFile)
				s = "Group: "+currentGroup;
	        GUI.Label(new Rect(10, 10, 200, 20),s );
			s = "Collected Coins : "+collectedCoins;
			GUI.Label(new Rect(10, 30, 200, 20),s );*/
			Event currentEvent = Event.current;

			switch(state){

			case -8:
				GUI.color = guiC;
				GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height),background);
				GUI.DrawTexture( new Rect(Xbound,YBound,Screen.width-Xbound2,Screen.height-YBound2),StartTex);
				break;
			case -7:
				GUI.color = guiC;
				style.fontSize = 38 + (Screen.width/502) * 20;
				style.normal.textColor = Color.cyan;
				style.alignment = TextAnchor.UpperLeft;
				GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height),background);
				GUI.Label(new Rect(Xbound,YBound,Screen.width,Screen.height/6),"For the best exprience ",style);
				GUI.Label(new Rect(Xbound2,YBound2,Screen.width,Screen.height/6),"play with your headphones ON",style);
			//	GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height/6),"ON",style);
				break;
			case -6:
				GUI.color = guiC;
				GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height),background);
				GUI.DrawTexture( new Rect(Xbound,YBound,Screen.width-Xbound2,Screen.height-YBound2),StartTex);
				break;
			case -5:
				GUI.color = guiC;
				StoryTex = Resources.Load("story") as Texture2D;
				GUI.DrawTexture( new Rect(Xbound,0,StoryTex.width,Screen.height),StoryTex,ScaleMode.StretchToFill);

				style.fontSize = 18 + (Screen.width/502) * 20;
				style.normal.textColor = Color.white;
				style.alignment = TextAnchor.LowerCenter;
				GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height * 2/6),"-- Tap to Skip --",style);

				break;
			case -4:
				Texture2D texture1 = Resources.Load("start a") as Texture2D;
				GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height),texture1);

				style.fontSize = 28+ (Screen.width/502) * 10;
				style.normal.textColor = Color.white;
				s = "Tap To Start";
				GUI.Label(new Rect(Screen.width/2 + Screen.width / 30,Screen.height/4,Screen.width/3,Screen.height/5),s,style);

				Texture2D tikTex = Resources.Load("tik") as Texture2D;
				Texture2D tikkTex = Resources.Load("tikk") as Texture2D;

				Rect rect = new Rect(Screen.width * 4 / 6+ Screen.width * 1 / 12,Screen.height - Screen.height/16,Screen.width/13,Screen.height/16);
				style.fontSize = 50+ (Screen.width/502) * 10;
				style.normal.textColor = Color.black;
				style.alignment = TextAnchor.MiddleCenter;
				s = "Tutorial";
				GUI.Label(new Rect(Screen.width * 4 / 6 + Screen.width * 1 / 12+Screen.width/12,Screen.height - Screen.height/16,Screen.width * 1 / 6,Screen.height/16),s,style);


				TutorialMode = GUI.Toggle(rect,TutorialMode,"",style);

				if(GUI.Toggle(rect,TutorialMode,"",style)){
					tikkTexture = tikkTex;
				}
				else{
					tikkTexture = tikTex;
					showGLmsg = false;
				}

				GUI.DrawTexture(rect,tikkTexture);
		//		TutorialMode = GUI.Toggle(,TutorialMode,"",style);
		/*		style.fontSize = 18 + (Screen.width/502) * 10;
				s = "Exit Game";
				Rect R = new Rect(1,Screen.height - Screen.height/10 - 1,Screen.width/5,Screen.height/10);
				GUI.Label(R,s,style);
				currentEvent = Event.current;
				if(R.Contains(currentEvent.mousePosition)){
					if(Input.GetMouseButtonDown(0)){}
						else
					{
						Application.Quit();
					}
				}
				s = "Credit";
				R = new Rect(50 + Screen.width/5,Screen.height - Screen.height/10 - 1,Screen.width/5,Screen.height/10);
				GUI.Label(R,s,style);
				currentEvent = Event.current;
				if(R.Contains(currentEvent.mousePosition)){
					if(Input.GetMouseButtonDown(0)){}
					else
					{

						state = 10;
					}
				}
		*/
				break;
			case -3:
				Texture2D texture2 = Resources.Load("start b") as Texture2D;
				GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height),texture2);
				break;
			case -2:
				Texture2D texture3 = Resources.Load("start c") as Texture2D;
				GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height),texture3);
				break;
			case -1:
				GUI.color = guiC;
				Texture2D texture4 = Resources.Load("start d") as Texture2D;
				GUI.DrawTexture( new Rect(Xbound,YBound,Screen.width-Xbound2,Screen.height-YBound2),texture4);
				break;

			case 10:
				GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height),CreditTex);
				style.fontSize = 38 + (Screen.width/502) * 20;
				GUI.Label(new Rect(Screen.width/3+Screen.width/20,Screen.height/8,Screen.width/3,Screen.height/5),creditText,style);
				style.fontSize = (Screen.width/502) * 20;
				GUI.Label(new Rect(Screen.width/3 + Screen.width/11,Screen.height/5+ Screen.height/10,Screen.width/3,Screen.height/3),creditText2,style);
				break;

			case 20:
				style.fontSize = 38 + (Screen.width/502) * 20;
				style.normal.textColor = Color.white;
				style.alignment = TextAnchor.UpperCenter;


				switch(TutorialCurrentCorr){

				case 1:
					GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height * 2/6),"Turn left, right and up by swiping in that direction. If you don't swipe in these 3 directions you will automatically go straight.",style);
					break;
				case 2:
					GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height * 2/6),"You have to swipe in the direction of the wall that has the same color as the ground to go to the next corridor.",style);
					break;
				case 3:
					GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height * 2/6),"When the front wall has the same color as the ground, do not swipe and you will go straight through the front wall.",style);
					break;
				case 4:
					GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height * 2/6),TutMsg,style);
					break;
				case 6:
					style.alignment = TextAnchor.MiddleCenter;
					GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height * 2/6),"Swipe down to pass this obstacle.",style);
					break;
				case 7:
					style.alignment = TextAnchor.MiddleCenter;
					GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height * 2/6),"Tiles that have a distinct and unique color lead to a Bonus corridor. If you get coins in there, they will add 100 points to your score.",style);
					break;
				}
				break;
			
			}

		
			if (ww.ElapsedMilliseconds >= 2000)
			if(LosingState == 1){

				if(TutorialMode){
					TutCorrPassed = false;
					TutorialCurrentCorr-=2;
					int a = TutorialCurrentCorr;
					StartState = 0;
					RestartGame();
					TutorialCurrentCorr = a;
					makeTiles(pos);
				}
				Texture2D texture = Resources.Load("end screen") as Texture2D;
				GUI.DrawTexture( new Rect(0,0,Screen.width,Screen.height),texture);

				Rect r = new Rect(Screen.width / 2 - Screen.width/10 ,Screen.height / 2 + Screen.height/10+20,Screen.width/5,Screen.height/5);
				GUI.DrawTexture( r,buttonsImg[B_show_index[2]] as Texture2D);

				currentEvent = Event.current;

				if (currentEvent.isMouse)
				{
					if(r.Contains(currentEvent.mousePosition))
							if(Input.GetMouseButtonDown(0))
							{
								B_show_index[2] = 2 * 2;
							}
							else
							{
								B_show_index[2] = 2 * 2 + 1;
								DoButtonFunc(2);
							}
				}

				style.normal.textColor = Color.red;
				style.fontStyle = FontStyle.Italic;
				style.fontSize = 48 + (Screen.width/502) * 10;
				s = "Score : "+ PlayerScore.ToString();
				GUI.Label(new Rect(Screen.width * 2 / 5,Screen.height * 1 / 5+Screen.height/9,Screen.width/3,Screen.height/5),s,style);

				if(highScoreSet)
				{
					Texture2D text = Resources.Load("high score") as Texture2D;
					GUI.DrawTexture(new Rect(Screen.width * 1 / 4,Screen.height * 1 / 5+Screen.height * 1.5f / 9 ,Screen.width * 2 / 4,Screen.height/4),text);
				}
				else
				{
					style.normal.textColor = Color.black;
					s = "HighScore : "+ HighScore.ToString();
					GUI.Label(new Rect(Screen.width * 2 / 5,Screen.height * 1 / 5+Screen.height* 2/9,Screen.width/3,Screen.height/5),s,style);
				}
												
			}
			/*
			if(state!= -6 && state != -5 && state != -4){
				s = (this.currentCurve.getID()).ToString()+"  "+(this.currentGroup.getID()).ToString();
				GUI.Label(new Rect(10, Screen.height - 70, 200, 20),s);
				
				s = "Speed : "+RunnerSpeed;
				GUI.Label(new Rect(10, Screen.height - 50, 200, 20),s);
			}
			*/
			s = " ";
			if(WayLightMode)
				s = "wayLight is On";
			if(AutoPilot)
				s = "AutoPilot is On";
			GUI.Label(new Rect(100, 10, 200, 20),s);
			
			if(AutoPilot){
				Texture2D tex = Resources.Load("asd") as Texture2D;
				
				GUI.DrawTexture( new Rect(0,z1 - 60,1000,60),tex);
				GUI.DrawTexture( new Rect(0,z2 - 60,1000,60),tex);
				
				z1 = (z1 + 1) % (Screen.height + 80);
				z2 = (z2 + 1) % (Screen.height + 80);
			}
			
			if(WayLight)	
				if(AutoPilotResluts.Count > 0)
			{
				Texture2D texture = Resources.Load("arr") as Texture2D;
				Rect rec = new Rect(200,200,50,50);
				
				for(int i = 0;i<AutoPilotResluts.Count;i++)
				{
					if(AutoPilotResluts[i] as string == "right")
					{
						texture = Resources.Load("arr") as Texture2D;
						rec = new Rect(Screen.width/2+25,60,50,50);
					}
					else if(AutoPilotResluts[i] as string == "left")
					{
						texture = Resources.Load("arrL") as Texture2D;
						rec = new Rect(Screen.width/2-75,60,50,50);
					} 
					else if(AutoPilotResluts[i] as string == "up")
					{
						texture = Resources.Load("arrU") as Texture2D;
						rec = new Rect(Screen.width/2-25,10,50,50);
					}
					else if(AutoPilotResluts[i] as string == "endTile")
					{
						texture = Resources.Load("arrF") as Texture2D;
						rec = new Rect(Screen.width/2-25,80,50,25);
					}
					
					GUI.DrawTexture( rec,texture);
				}
			}

			if(!GameIsPaused && ! gameOver)
			{
				int a = PlayerScore;
				int c = 0;
				while(a != 0)
				{
					GUI.DrawTexture(new Rect(Screen.width- Screen.width/16 - c* Screen.width/33 - 7,Screen.height/35,Screen.width/33,Screen.height/13),numbersImg[a%10] as Texture2D);
					a /= 10;
					c++;	
				}
			}
			
			currentEvent = Event.current;

			if(TutorialMode && state >= 0 && state <10){
				GUI.DrawTexture(buttonsPos[0],buttonsImg[B_index[2]] as Texture2D);
				if (currentEvent.isMouse) {
					if(buttonsPos[0].Contains(currentEvent.mousePosition))
					{
						if(Input.GetMouseButtonDown(0))
						{
							B_index[2] = 4;
						}
						else
						{
							SFX.PlayOneShot(SFXs[0],SoundVolume/2);
							B_index[2] = 5;
							DoButtonFunc(2);
							
						}
						
					}
				}
			}

			if(!GameIsPaused && state >= 0 && state <10 && ! gameOver && !TutorialMode){
				GUI.DrawTexture(buttonsPos[0],buttonsImg[B_index[0]] as Texture2D);

			if(showGLmsg){

				if(state == 1 && !w.IsRunning)
					w.Start();

				if(w.ElapsedMilliseconds > 3000){
					showGLmsg = false;
					w.Reset();
				}
				style.fontSize = 35 + (Screen.width/502) * 20;
				style.normal.textColor = Color.white;
				style.alignment = TextAnchor.MiddleCenter;
				GUI.Label(new Rect(TutMsgX,TutMsgY,Screen.width,Screen.height * 2/6),"Good Luck!",style);
			}
			
		
			if (currentEvent.isMouse) {
				if(buttonsPos[0].Contains(currentEvent.mousePosition))
				{
					if(Input.GetMouseButtonDown(0))
					{
						B_index[0] = 0;
					}
					else
					{
						SFX.PlayOneShot(SFXs[0],SoundVolume/2);
						B_index[0] = 1;
						GameIsPaused = true;
						if(audio.isPlaying)
							audio.Pause();
						
					}
					
				}
			}

			}
			if(GameIsPaused && !TutorialMode)
			{
				Texture2D texture = Resources.Load("pause screen") as Texture2D;
				GUI.DrawTexture( new Rect(Screen.width/5,Screen.height * 0.5f/5,Screen.width*3/5,Screen.height*4/5),texture);

				style.normal.textColor = Color.red;
				style.fontStyle = FontStyle.Italic;
				style.fontSize = 38 + (Screen.width/502)  * 10;
				s = "Score : "+ PlayerScore.ToString();
				GUI.Label(new Rect(Screen.width * 1 / 5+Screen.width * 2 /25,Screen.height * 0.5f / 5+Screen.height/9,Screen.width/3,Screen.height/5),s,style);
				
				style.normal.textColor = Color.black;
				style.fontSize = 36 + (Screen.width/502) * 10;
				s = "Sound Volume : ";
				GUI.Label(new Rect(Screen.width * 1 / 5+Screen.width * 2/25,Screen.height * 2.5f / 5 + Screen.height* 2 /35,Screen.width /5,Screen.height/5),s,style);
				tempSliderValue = SoundVolume;
				SoundVolume = GUI.HorizontalSlider(new Rect(Screen.width * 2 / 5 +Screen.width * 2/25,Screen.height * 2.5f / 5 ,Screen.width * 1 / 4,Screen.height/5), SoundVolume, 0.0F, 1F,MySkin.horizontalSlider,MySkin.horizontalSliderThumb);
				if(tempSliderValue != SoundVolume)
					writeSettingsFile();

				s = "Music Volume : ";
				GUI.Label(new Rect(Screen.width * 1 / 5+Screen.width * 2/25,Screen.height * 3.5f / 5 + Screen.height* 2 /35,Screen.width /5,Screen.height/5),s,style);
				tempSliderValue2 = MusicVolume;
				MusicVolume = GUI.HorizontalSlider(new Rect(Screen.width * 2 / 5 +Screen.width * 2/25,Screen.height * 3.5f / 5 ,Screen.width * 1 / 4,Screen.height/5), MusicVolume, 0.0F, 1F,MySkin.horizontalSlider,MySkin.horizontalSliderThumb);
				if(tempSliderValue2 != MusicVolume){
					writeSettingsFile();
					audio.volume = MusicVolume;
				}


				currentEvent = Event.current;
				if (currentEvent.isMouse)
				{
					for(int i = 1;i<3;i++){
						if(buttonsPos[i].Contains(currentEvent.mousePosition))
							if(Input.GetMouseButtonDown(0))
						{
							B_show_index[i] = i * 2;
						}
						else
						{
							B_show_index[i] = i * 2 + 1;
							Input.ResetInputAxes();
							DoButtonFunc(i);
						}
					}
				}
				else{
					for(int i = 1;i<3;i++)
						GUI.DrawTexture(buttonsPos[i],buttonsImg[B_show_index[i]] as Texture2D);

				}
			}
			
			
			
		}
		
		void DoButtonFunc(int a){

			SFX.PlayOneShot(SFXs[0],SoundVolume/2);


			switch(a){
			case 0:
				break;
			case 1:
				GameIsPaused = !GameIsPaused;
				if(!audio.isPlaying)
					audio.Play();
				break;
			case 2:

				audio.Stop();
				w.Reset();
				Input.ResetInputAxes();
				StartState = -4;
				RestartGame();
				break;
			}
		}

		void writeSettingsFile(){
			using (StreamWriter outfile = new StreamWriter(Application.persistentDataPath+"/setts"))
			{
				outfile.WriteLine(HighScore.ToString());
				outfile.WriteLine(SoundVolume.ToString());
				outfile.WriteLine (MusicVolume.ToString());
			}
		}

		void readSettingsFile(){

			if (!File.Exists(Application.persistentDataPath+"/setts")) 
				TutorialMode = true;
			else
			try{

				using (StreamReader infile = new StreamReader(Application.persistentDataPath+"/setts"))
				{
					HighScore = System.Int32.Parse(infile.ReadLine());
					SoundVolume = float.Parse(infile.ReadLine());
					MusicVolume = float.Parse(infile.ReadLine());
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}
		}

		void ReadFile(){
		
			StringReader reader = null; 	 
			TextAsset setdata = (TextAsset)Resources.Load("settings", typeof(TextAsset));
			reader = new StringReader(setdata.text);
			
			if ( reader == null )
			{
				print("settings.txt not found or not readable");
				noFile = true;
			}


/*
			FileInfo theSourceFile = null;
			StreamReader reader = null;
			
			theSourceFile = new FileInfo (Application.dataPath + "/settings.txt");
			if ( theSourceFile != null && theSourceFile.Exists )
				reader = theSourceFile.OpenText();
			if ( reader == null )
			{
				print("settings.txt not found or not readable");
				noFile = true;
			}
*/
			else
			{
				
				noFile = false;
				string[] words;
				txt = reader.ReadLine();
				words = txt.Split(' ');
				this.runningMode = Int32.Parse(words[0]);
				int RunningModetemp = 0;
				if(runningMode != 0)
					RunningModetemp = Int32.Parse(words[1]);
				
				RunnerSpeed = 	float.Parse(reader.ReadLine().Split('=')[1]);
				
				txt = reader.ReadLine();
				words = txt.Split('-');
				
				if(txt != null)
					for(int i = 0;i<words.Length;i++)
						this.curveSeq.Add(Int32.Parse(words[i]));
				
				
				
				float acc = 0f;
				int curveTempID = 0;
				ArrayList curveTemp = new ArrayList();
				
				while(!txt.Contains("Group")){
					txt = reader.ReadLine();
					if(txt.Contains("Curve")){
						curveTemp.Clear();
						curveTempID = Int32.Parse(txt.Split(' ')[1]);
						
						words = (reader.ReadLine().Split('=')[1]).Split('-');
						for(int i = 0;i<words.Length;i++)
							curveTemp.Add(Int32.Parse(words[i]));
						acc =  float.Parse(reader.ReadLine().Split('=')[1]);
						
						this.curves.Add(new Curve(curveTempID,curveTemp,acc));
						txt = reader.ReadLine();
					}
					
				}
				
				int groupTempID = 0;
				int lmin = 0,lmax = 0,tsf = 0,epr = 0,gtp = 50,htp = 0,htf = 5,htu = 5,cip = 0,cid = 0,op = 0,wlp = 0,wld = 0,spp = 0,spf = 0,spu = 0,gtbsp = 0,cp = 0;
				float ts = 0;
				bool isBonus = false;
				int chnc = 0;
				ArrayList color = new ArrayList();
				
				while(txt != null){
					if(txt.Contains("Group")){
						if( !(txt.Split(' ')[1]).Contains("Bonus") )
							groupTempID = Int32.Parse(txt.Split(' ')[1]);
						else
						{
							isBonus = true;
							groupTempID = 1000 + Int32.Parse((txt.Split(' ')[1]).Substring(5));
							//	print ((txt.Split(' ')[1]).Substring(5));
						}
						if(isBonus)
							chnc = Int32.Parse(reader.ReadLine().Split('=')[1]);
						lmin = Int32.Parse(reader.ReadLine().Split('=')[1]);
						lmax = Int32.Parse(reader.ReadLine().Split('=')[1]);
						tsf  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						epr  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						ts   = float.Parse(reader.ReadLine().Split('=')[1]);
						gtp  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						htp  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						htf  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						htu  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						cip  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						cid  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						op   = Int32.Parse(reader.ReadLine().Split('=')[1]);
						wlp  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						wld  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						spp  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						spf  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						spu  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						gtbsp  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						cp  = Int32.Parse(reader.ReadLine().Split('=')[1]);
						
						txt = reader.ReadLine();
						words = txt.Split('=');
						
						while(words[0] == "color"){
							color.Add(words[1]);
							txt = reader.ReadLine();
							words = txt.Split('=');
						}
						
						if(!isBonus)
							this.groups.Add( new Group(groupTempID,lmin,lmax,tsf,epr,ts,color,gtp,htp,htf,htu,cip,cid,op,wlp,wld,spp,spf,spu,gtbsp,cp) );
						else
						{
							isBonus = false;
							this.BonusGroups.Add( new BonusGroup(chnc,groupTempID,lmin,lmax,tsf,epr,ts,color,gtp,htp,htf,htu,cip,cid,op,wlp,wld,spp,spf,spu,gtbsp,cp) );
						}
						color.Clear();
					}
					txt = reader.ReadLine();	
				}		
				
				if(runningMode == 1)
					this.currentGroup = groups[getIndexInAllGroups(RunningModetemp)] as Group;
				else if(runningMode == 2){
					this.currentCurve = curves[getIndexInAllCurves(RunningModetemp)] as Curve;
					this.currentGroup = groups[0] as Group;
				}
				else if(runningMode == 0){
					this.currentCurve = curves[0] as Curve;
					this.currentGroup = groups[0] as Group;
				}
				
			}
			
		}
		
		void Start() {

			GameObject.FindGameObjectsWithTag("Respawn");
			ReadFile();
			readSettingsFile();
			trans = transform;
			OriginalPosition = trans.position;
			OriginalRotation = trans.rotation;
			w = new Stopwatch();
			ww = new Stopwatch();
			distance = 0;
			movingDirection = 1;
			upOrDown = 0;
			state = StartState;
			LosingState = 0;
			counter = 0;
			LCounter = 0;
			Hcounter = 0;
			i1 = 1;
			i2 = 0;
			lastKeyPressed = "qqwe";
			anyKeyPressed = false;
			piloted = false;
			pos = new Vector3(-4.5f,-2.5f,0);
			tempPos = new Vector3(-4.5f, -2.5f,-16f);
			oldAngle = new Vector3(0,0,0);	   
			g = new GameObject();
			pos = setPosition();
			
			for(int i = 0;i<10;i++){
				numbersImg.Add( Resources.Load(i.ToString()) as Texture2D);
				
				Screen.orientation = ScreenOrientation.LandscapeLeft;
			}

			StoryTex = Resources.Load("story") as Texture2D;
			CreditTex = Resources.Load("start a") as Texture2D;
			StartTex = Resources.Load("logo") as Texture2D;

			for(int i = 0;i<10;i++)
				splashImg.Add ( Resources.Load ("splash01") as Texture2D);
			for(int i = 0;i<10;i++)
				splashImg.Add ( Resources.Load ("splash02") as Texture2D);
			for(int i = 0;i<10;i++)
				splashImg.Add ( Resources.Load ("splash03") as Texture2D);


			background = new Texture2D(Screen.width,Screen.height);
			Color[] pix = background.GetPixels();
			for(int i = 0;i < pix.Length;i++)
				pix[i] = Color.black;
			background.SetPixels(pix);
			background.Apply();
			
			buttonsImg.Add( Resources.Load ("pause_button") as Texture2D);
			buttonsImg.Add( Resources.Load ("pause_button2") as Texture2D);
			
			buttonsImg.Add( Resources.Load ("play_button") as Texture2D);
			buttonsImg.Add( Resources.Load ("play_button2") as Texture2D);
			
			buttonsImg.Add( Resources.Load ("replay_button") as Texture2D);
			buttonsImg.Add( Resources.Load ("replay_button2") as Texture2D);
			
			buttonsImg.Add( Resources.Load ("settings_button") as Texture2D);
			buttonsImg.Add( Resources.Load ("settings_button2") as Texture2D);
			
			buttonsImg.Add( Resources.Load ("objective_button") as Texture2D);
			buttonsImg.Add( Resources.Load ("objective_button2") as Texture2D);
			
			buttonsPos[0] = new Rect(Screen.height/70,Screen.height/70,Screen.height/7,Screen.height/7);
			buttonsPos[1] = new Rect(Screen.width * 3 / 5 -Screen.width/20,Screen.height * 1.5f / 5,Screen.width/5,Screen.height/5);
			buttonsPos[2] = new Rect(Screen.width * 1 / 5 +Screen.width/20,Screen.height * 1.5f / 5,Screen.width/5,Screen.height/5);
	//		buttonsPos[3] = new Rect(Screen.width * 3 / 5-10,Screen.height * 2 / 5,Screen.width/5,Screen.height/5);
	//		buttonsPos[4] = new Rect(Screen.width * 1 / 5+11,Screen.height * 2 / 5,Screen.width/5,Screen.height/5);

			B_index[0] = 1; B_show_index[0] = 1;
			B_index[1] = 3;	B_show_index[1] = 3;
			B_index[2] = 5;	B_show_index[2] = 5;
			B_index[3] = 7;	B_show_index[3] = 7;
			B_index[4] = 9;	B_show_index[4] = 9;
			//	Prepare();

			ArrayList colorTemp = new ArrayList();
			colorTemp.Add("yellow");
			TutorialGroups.Add(new Group(9000,20,20,4,5,0.0f,colorTemp,0,0,0,0,0,0,0,0,0,0,0,0,0,0));
			colorTemp.Add("red");
			TutorialGroups.Add(new Group(9001,20,20,1,2,0.0f,colorTemp,0,0,0,0,0,0,0,0,0,0,0,0,0,0));
			colorTemp.Clear();
			colorTemp.Add("green");
			colorTemp.Add("yellow");
			TutorialGroups.Add(new Group(9002,20,20,1,3,0.0f,colorTemp,0,0,0,0,0,0,0,0,0,0,0,0,0,0));
			colorTemp.Clear();
			colorTemp.Add("red");
			colorTemp.Add("yellow");
			TutorialGroups.Add(new Group(9002,20,20,3,4,0.0f,colorTemp,0,0,0,0,0,0,0,0,0,0,0,0,0,0));
			colorTemp.Clear();
			colorTemp.Add("blue");
			colorTemp.Add("red");
			TutorialGroups.Add(new Group(9002,20,20,3,4,0.0f,colorTemp,0,0,0,0,0,0,0,0,0,0,0,0,0,0));
			colorTemp.Clear();
			colorTemp.Add("red");
			colorTemp.Add("yellow");
			TutorialGroups.Add(new Group(9003,20,20,3,4,0.0f,colorTemp,0,100,10,8,0,0,0,0,0,0,0,0,0,0));
			colorTemp.Clear();
			colorTemp.Add("green");
			colorTemp.Add("white");
			TutorialGroups.Add(new Group(9004,20,20,2,4,0.0f,colorTemp,100,0,0,0,0,0,0,0,0,0,0,0,0,0));
			colorTemp.Clear();
			colorTemp.Add("white");
			TutorialGroups.Add(new Group(9005,20,20,2,4,0.0f,colorTemp,0,0,0,0,0,0,0,0,0,0,0,0,0,100));

			tikkTexture = Resources.Load("tik") as Texture2D;

			font = (Font)Resources.Load("font", typeof(Font));

			MySkin.horizontalSliderThumb.fixedWidth = 50 * (Screen.width / 482) ;
			MySkin.horizontalSlider.fixedHeight = 50 * (Screen.width / 482) ;
			MySkin.horizontalSliderThumb.fixedHeight = 55 * (Screen.width / 482) ;

			audio.clip = RunMusic;
			audio.loop = true;
			audio.volume = MusicVolume;

	//		revmob.SetTestingMode(RevMob.Test.WITH_ADS);
	//		revmob.SetTestingMode(RevMob.Test.WITHOUT_ADS);

	//		ButtonSound =  (GetComponents<AudioSource>())[0];



	//		revmob.ShowFullscreen();

		}

		void RestartGame(){

			DestroyTiles(i1);
			DestroyTiles(i2);
			ww.Reset();
			w.Reset();
			trans.position = OriginalPosition;
			trans.rotation = OriginalRotation;
			AutoPilotResluts = new ArrayList();
			GlowingTiles = new ArrayList();
			halfTiles = new ArrayList();
			Coins = new ArrayList();
			CoinsObjects = new ArrayList();

			Spike = -1;
			CurrentGroupIndex = -1;
			CurrentCurveIndex = 0;
			collectedCoins = 0;
			PlayerScore = 0;
			tileSize = 2.5f;
			RunnerSpeed = 0.15f;
			
			qwerty = 0;
			txt = " ";
			
			gameOver = false;
			GameIsPaused = false;
			highScoreSet = false;
			AutoPilot = false;
			WayLightMode = false;
			WayLight = false;
			shouldSlide = false;
			shouldGoGlow = false;
			Shaking = false; 
			swiping = false;

			xx = 0;
			lastGroup = " ";
		
			Xbound = 0;
			YBound = 0;
			Xbound2 = 0;
			YBound2 = 0;
			
			tempLS = 0;
			

			z1 = 0;
			z2 = 200;

			distance = 0;
			movingDirection = 1;
			upOrDown = 0;
			state = StartState;
			LosingState = 0;
			counter = 0;
			LCounter = 0;
			Hcounter = 0;
			i1 = 1;
			i2 = 0;
			lastKeyPressed = "qqwe";
			anyKeyPressed = false;
			piloted = false;
			pos = new Vector3(-4.5f,-2.5f,0);
			tempPos = new Vector3(-4.5f, -2.5f,-16f);
			oldAngle = new Vector3(0,0,0);
	//		g = new GameObject();
			pos = setPosition();

			if(TutorialMode)
				TutorialCurrentCorr = 0;
	//		makeTiles(pos);

	//		revmob.ShowFullscreen();
		}

		void MoveOnTheCurve(){
			if(runningMode == 1)
			{		
			}
			else if(runningMode == 2)
			{
				CurrentGroupIndex = (++CurrentGroupIndex) % currentCurve.curve.Count;
				currentGroup = groups[getIndexInAllGroups((int)currentCurve.curve[CurrentGroupIndex])] as Group;
			}
			else{
				
				int temp = (int)curveSeq[CurrentCurveIndex];
				CurrentGroupIndex++;
				
				if (CurrentGroupIndex == currentCurve.curve.Count)
				{
					//	RunnerSpeed -= (curves[getIndexInAllCurves((int)curveSeq[CurrentCurveIndex])] as Curve).acc;
					if(CurrentCurveIndex < curveSeq.Count - 1)
						CurrentCurveIndex++;
					CurrentGroupIndex = 0;
				}
				
				
				currentCurve = curves[getIndexInAllCurves((int)curveSeq[CurrentCurveIndex])] as Curve;
				currentGroup = groups[getIndexInAllGroups((int)currentCurve.curve[CurrentGroupIndex])] as Group;
				
				if( (int)curveSeq[CurrentCurveIndex] != temp || (CurrentCurveIndex == curveSeq.Count - 1 && CurrentGroupIndex == 0)){
					RunnerSpeed += currentCurve.acc;
					if(RunnerSpeed > 0.29f)
						RunnerSpeed = 0.29f;
				}
				
			}
		}
		
		int getIndexInAllGroups(int a){
			for(int i = 0;i<groups.Count;i++)
				if(((Group) groups[i]).getID() == a)
					return i;
			return -1;
		}	
		
		int getIndexInAllCurves(int a){
			for(int i = 0;i<curves.Count;i++)
				if(((Curve) curves[i]).getID() == a)
					return i;
			return -1;
		}	
		
		void makeBonusGroup(){
			System.Random R = new System.Random();
			ArrayList randSpace = new ArrayList();
			for(int i = 0;i<BonusGroups.Count;i++)
				for(int j = 0;j<(BonusGroups[i] as BonusGroup).getOccurChance() / 10;j++)
					randSpace.Add((BonusGroups[i] as BonusGroup).getID());
			int RandInt = R.Next(randSpace.Count);
			for(int k = 0;k<BonusGroups.Count;k++)
				if((BonusGroups[k] as BonusGroup).getID() == (int)randSpace[RandInt])
			{
				BonusGroup = BonusGroups[k] as BonusGroup;
				break;
			}
			
		}
		
		void Prepare(){

			Group gr;
			if(TutorialCurrentCorr == 8)
			{
				TutorialMode = false;
				TutorialCurrentCorr = 0;
				PlayerScore = 0;
				showGLmsg = true;
			}

			if(TutorialMode)
			{
				gr = TutorialGroups[TutorialCurrentCorr++] as Group;
				TutCorrPassed = false;
			}
			else
			{
				if((qwerty == ((ArrayList)ExistingCorridor[i1]).Count || qwerty == -1) && currentGroup.getID() < 1000)
				{
					MoveOnTheCurve();
					gr = currentGroup;
					lastGroup = "MyNameSpace.Group";
				}
				else
				{
					makeBonusGroup();
					gr = BonusGroup;
					lastGroup = "MyNameSpace.BonusGroup";
				}

			}
				System.Random r = new System.Random();
				int min = gr.getLength_min();
				int max = gr.getLength_max();
				CorridorLength = r.Next(min,max+1);
				tileSame = gr.getTilesSameAsFloor();
				endRep = gr.getPatternRep();
				setHalfTiles(gr);
				if(currentOrb == null)
					setSpikes(gr);
				tile.setValidColors(gr.getColors());
				piloted = false;
			
		}
		
		void setGlowingTiles(int x,int Limit_max,int Limit_min){
			System.Random R = new System.Random();
			
			int RandInt = 0;
			GlowingTiles.Clear();
			//	print ("spike : "+(Limit_max+2));
			
			while(x/100 != 0){
				RandInt = R.Next(Limit_min,Limit_max + 1); // tu ye tile bshtar az yeki ham mitune glowtile dashte bashim
				//	while(GlowingTiles.Contains(RandInt))
				//		RandInt = R.Next(Limit_min,Limit_max + 1);
				GlowingTiles.Add(RandInt);
				x = (x / 100 - 1) * 100 +  x % 100;
			}
			RandInt = R.Next(1,101);
			if(RandInt < x){
				RandInt = R.Next(Limit_min,Limit_max + 1);
				while(GlowingTiles.Contains(RandInt))
					RandInt = R.Next(Limit_min,Limit_max + 1);
				GlowingTiles.Add(RandInt);
			}
			
		}
		
		void setHalfTiles(Group gg){
			System.Random R = new System.Random();
			int x =	gg.getHtp();

			int Limit_min = gg.getHtf();
			int Limit_max = CorridorLength - gg.getHtu();

		//	print( "min : "+Limit_min +" max : "+Limit_max); 
			int RandInt = 0;
			halfTiles.Clear();
			if(x >= 1000){
				RandInt = R.Next(1,101);
				if(RandInt < ((x - 1000) / 100 ) * 10)
				{
					RandInt = R.Next(Limit_min,Limit_max + 1);
					halfTiles.Add(RandInt);
					Spike = RandInt + (x % 100);
					setGlowingTiles(100,Spike - 1,RandInt + 3);
				}
			}
			else
			{
				while(x/100 != 0){
					RandInt = R.Next(Limit_min,Limit_max + 1);
					while(halfTiles.Contains(RandInt))
						RandInt = R.Next(Limit_min,Limit_max + 1);
					halfTiles.Add(RandInt);
					x = (x / 100 - 1) * 100 +  x % 100;
				}
				RandInt = R.Next(1,101);
				if(RandInt < x){
					RandInt = R.Next(Limit_min,Limit_max + 1);
					while(halfTiles.Contains(RandInt) )
						RandInt = R.Next(Limit_min,Limit_max + 1);
					halfTiles.Add(RandInt);
				}
			}
			/*
			if(halfTiles.Count > 1)
			if( Math.Abs((int)halfTiles[0] - (int)halfTiles[1]) < 5 &&  Math.Abs((int)halfTiles[0] - (int)halfTiles[1]) > 0 ){
				if((int)halfTiles[1] > (int)halfTiles[0])
					halfTiles[1] += 5 - (Math.Abs((int)halfTiles[0] - (int)halfTiles[1]));
				else
					halfTiles[0] += 5 - (Math.Abs((int)halfTiles[0] - (int)halfTiles[1]));
			}*/
			//	int temp = 0;
			//	for(int i = 0; i< halfTiles.Count;i++)
			//			for(int j = 0; j< halfTiles.Count;j++){
			//		temp = Math.Abs(halfTiles[i] - halfTiles[j]);
			//		if(temp  < 5 && temp > 0 )
			//		}
		}
		
		void setSpikes(Group gg){
			System.Random R = new System.Random();
			int x =	gg.getSpp();
			int Limit_min = gg.getSpf();
			int Limit_max = gg.getSpu();
			
			int RandInt = 0;
			
			if(gg.getHtp() < 1000)
				Spike = -1;
			RandInt = R.Next(1,101);
			if(RandInt < x){
				RandInt = R.Next(Limit_min,Limit_max + 1);
				Spike = RandInt;
				setGlowingTiles(gg.getGtbsp()*100,Spike - 1,Spike - 4);
			}
			else if(gg.getGlowingTilePercentage() != 0)
				setGlowingTiles(gg.getGlowingTilePercentage(),(int) Math.Ceiling((double)CorridorLength / 4 * 3),(int) Math.Ceiling((double)CorridorLength / 4 * 1));
			else if(gg.getGlowingTilePercentage() == 0)
				GlowingTiles.Clear();
		}
		
		void setCoins(Group gg){
			System.Random R = new System.Random();
			int x = gg.getCp();
			int q = (int) Math.Ceiling(CorridorLength / 2d);
			int RandInt = 0;
			Coins.Clear();
			for(int i = 0;i<CoinsObjects.Count;i++)
				Destroy(CoinsObjects[i] as GameObject);
			CoinsObjects.Clear();
			
			RandInt = R.Next(1,101);
			if(RandInt < x){
				for(int i = 1;i<=q;i++){
					RandInt = R.Next(2,CorridorLength - 2);
					//			while(Coins.Contains(RandInt))
					//				RandInt = R.Next(2,CorridorLength - 2);
					Coins.Add(RandInt);
				}
				instansiateCoin();
			}
		}
		
		void instansiateCoin(){
			Vector3 pos = new Vector3(0,0,0);
			for(int i = 0;i<Coins.Count;i++){
				pos = ((GameObject)ExistingCorridor[i1][(int)Coins[i]]).transform.Find("bottom").position;
				pos.y += 1f;
				CoinsObjects.Add(Instantiate(coinPrefab, pos , Quaternion.identity) as GameObject);
				if (movingDirection == 1 || movingDirection == -1)
					((GameObject)CoinsObjects[i]).transform.Rotate(new Vector3(0,90,-90));
				else if (movingDirection == 2 || movingDirection == -2)
					((GameObject)CoinsObjects[i]).transform.Rotate(new Vector3(0,0,-90));
			}
		}
		
		void generateItems(Group gg){
			System.Random R = new System.Random();
			int RandInt = 0;
			
			RandInt = R.Next(1,101);
			if(RandInt < gg.getCip())
			{
				instansiateControlItem(gg.getCid());
			} else
			{
				RandInt = R.Next(1,101);
				if(RandInt < gg.getOp())
				{
					instansiateOrb();
					
				} 
				else
				{
					RandInt = R.Next(1,101);
					if(RandInt < gg.getWlp())
					{
						instansiateWayLight(gg.getWld());
					}
				}
			}
			
			
		}
		
		void instansiateOrb(){
			
			if(currentOrb == null){
				OrbDistance = 4;
				trans.Translate(Vector3.forward * 20f);
				trans.Translate(Vector3.up * -1.5f);
				currentOrb = (GameObject)Instantiate(orbPrefab, trans.position , trans.rotation);
				trans.Translate(Vector3.forward * -20f);
				trans.Translate(Vector3.up * 1.5f);
			}
			
		}
		
		void instansiateControlItem(int a){
			autoPilotDur = a;
			trans.Translate(Vector3.forward * 20f);
			trans.Translate(Vector3.up * -0.5f);
			currentControlItem = (GameObject)Instantiate(controlItemPrefab, trans.position , Quaternion.identity);
			trans.Translate(Vector3.forward * -20f);
			trans.Translate(Vector3.up * 0.5f);
		}
		
		void instansiateWayLight(int a){
			wayLightDur = a;
			trans.Translate(Vector3.forward * 20f);
			trans.Translate(Vector3.up * -0.5f);
			currentWayLight = (GameObject)Instantiate(wayLightPrefab, trans.position , Quaternion.identity);
			Rotate2();
			currentWayLight.transform.rotation = g.transform.rotation;
			if (movingDirection == 1)
				currentWayLight.transform.Rotate(new Vector3(0,90,0));
			else if (movingDirection == 2)
				currentWayLight.transform.Rotate(new Vector3(0,0,0));
			else if (movingDirection == -1)
				currentWayLight.transform.Rotate(new Vector3(0,0,0));
			else if (movingDirection == -2)
				currentWayLight.transform.Rotate(new Vector3(0,180,0));
			
			trans.Translate(Vector3.forward * -20f);
			trans.Translate(Vector3.up * 0.5f);
		}
		
		bool CheckWin(){
			
			GameObject g1 = ((GameObject)ExistingCorridor[i1][qwerty - 1]).transform.Find("bottom").gameObject;
			string s,s2;
			if(!anyKeyPressed)
				s = "endTile";
			else if(lastKeyPressed == "right")
				s = "right";				
			else if (lastKeyPressed == "left")						
				s = "left";
			else 
				s = "up";
			
			if(s == "up")
				s2 = "top";
			else
				s2 = s;
			GameObject g2 = ((GameObject)ExistingCorridor[i1][qwerty - 1]).transform.Find(s2).gameObject;
			if(g2.renderer.material.color.ToString() == g1.renderer.material.color.ToString())						
				gameOver = false;
			else{
				gameOver = true;
				WayLight = false;


				if(PlayerScore>HighScore){
					HighScore = PlayerScore;
					highScoreSet = true;
					writeSettingsFile();
				}
				ww.Start();
			}
			
			if(currentOrb != null)
				if(s != Orb.dir)
					Destroy(currentOrb);

			if(gameOver)
				if(audio.isPlaying)
					audio.Stop();

			return !gameOver;
		}
		
		bool GlowCheck(){
			
			string s=" ",s3=" ";
			
			GameObject g3 = ((GameObject)ExistingCorridor[i1][qwerty - 1]).transform.gameObject;
			
			if(g3.transform.Find("right").gameObject.renderer.material.shader == Shader.Find("Self-Illumin/Specular"))
				s3 = "right";
			else if(g3.transform.Find("left").gameObject.renderer.material.shader == Shader.Find("Self-Illumin/Specular"))
				s3 = "left";
			else if(g3.transform.Find("top").gameObject.renderer.material.shader == Shader.Find("Self-Illumin/Specular"))
				s3 = "up";
			
			if(lastKeyPressed == "right")
				s = "right";				
			else if (lastKeyPressed == "left")						
				s = "left";
			else 
				s = "up";
			
			if( s3 == s)
				return true;
			else
				return false;
			
		}
		
		void Update () {

			if (Input.GetKeyDown(KeyCode.Escape))
				Application.Quit(); 

			if(!gameOver && !GameIsPaused || (GameIsPaused && TutorialMode))
				switch (state)
			{

				case -8:

				if(!w.IsRunning){
					w.Start();
					Xbound =  Screen.width/5;
					YBound =  Screen.height/4;
					Xbound2 = Screen.width*2/5;
					YBound2 = Screen.height*2/4;
					guiC = GUI.color;
					guiC = Color.black; 
				}
				if(w.ElapsedMilliseconds >= 1000){
					if(guiC.r < 1)
						guiC.r += 0.005f;
					if(guiC.g < 1 )
						guiC.g += 0.005f;
					if(guiC.b < 1)
						guiC.b += 0.005f;
				}
				if(w.ElapsedMilliseconds >= 4000){
					if(guiC.r > 0)
						guiC.r -= 0.01f;
					if(guiC.g > 0 )
						guiC.g -= 0.01f;
					if(guiC.b > 0)
						guiC.b -= 0.01f;
				}

				if(w.ElapsedMilliseconds >= 7000){
					w.Reset();
					state = -7;
				}

				break;
				case -7:

				if(!w.IsRunning){
					w.Start();
					guiC = GUI.color;
					guiC = Color.white; 
					Xbound = Screen.width;
					Xbound2 = -1 *Screen.width;
					YBound = Screen.height / 2;
					YBound2 = Screen.height / 2 + Screen.height / 10;
					TutMsgX = Screen.width;
					TutMsgY = Screen.height / 2 + Screen.height * 2 / 10; 
				}

				if(Xbound > Screen.width * 4 / 8 )
					Xbound -= 12 * (Screen.width/482);
				else if(Xbound < Screen.width * 4 / 8 && Xbound > Screen.width * 3 / 8)
					Xbound -= 0.2f * (Screen.width/482);
				else if(Xbound < Screen.width * 3 / 8)
					Xbound -= 12 * (Screen.width/482);

				if(Xbound2 < Screen.width * 4 / 8 )
					Xbound2 += 24 * (Screen.width/482);
				else if(Xbound2 > Screen.width * 4 / 8 && Xbound2 < Screen.width * 5 / 8)
					Xbound2 += 0.2f * (Screen.width/482);
				else if(Xbound2 > Screen.width * 5 / 8)
					Xbound2 += 24 * (Screen.width/482);


	//			if(Xbound2 < Screen.width * 4 / 8 )
	//				Xbound2 += 5 * (Screen.width/482);


	/*			if(w.ElapsedMilliseconds > 7000){

					if(Xbound > -1 * Screen.width )
						Xbound -= 6 * (Screen.width/482);
					
					if(Xbound2 < Screen.width)
						Xbound2 += 6 * (Screen.width/482);

				}
				if(w.ElapsedMilliseconds >= 1000){
					if(guiC.r < 1)
						guiC.r += 0.005f;
					if(guiC.g < 1 )
						guiC.g += 0.005f;
					if(guiC.b < 1)
						guiC.b += 0.005f;
				}
				if(w.ElapsedMilliseconds >= 4500){
					if(guiC.r > 0)
						guiC.r -= 0.01f;
					if(guiC.g > 0 )
						guiC.g -= 0.01f;
					if(guiC.b > 0)
						guiC.b -= 0.01f;
				}
	*/			
				if(w.ElapsedMilliseconds >= 8000){
					w.Reset();
					state = -6;
				}
	

				break;
				case -6:

				if(!w.IsRunning){
					w.Start();
					guiC = GUI.color;
					guiC = Color.white; 
					Xbound =  0;
					YBound =  0;
					Xbound2 = 0;
					YBound2 = 0;
					TutMsgX = 0;
					TutMsgY = Screen.height * 4 / 6;
				}

				StartTex = splashImg[(++counter)%30] as Texture2D;

				if(w.ElapsedMilliseconds >= 2000)
					if(Input.touches.Length > 0 || Input.GetMouseButtonDown(0)){
						counter = 0;
						w.Reset();
						state = -5;
						guiC = GUI.color;
						guiC = Color.black; 
					}

				if(w.ElapsedMilliseconds >= 5000){
					counter = 0;
					w.Reset();
					state = -5;
					guiC = GUI.color;
					guiC = Color.black; 
				}


				break;
				case -5:
				if(Xbound == 0){
					if(!w.IsRunning){
						w.Start();
					}
					if(w.ElapsedMilliseconds >= 1000){
						if(guiC.r < 1)
							guiC.r += 0.005f;
						if(guiC.g < 1 )
							guiC.g += 0.005f;
						if(guiC.b < 1)
							guiC.b += 0.005f;
					}
					if(w.ElapsedMilliseconds >= 6000){
						w.Reset();
						Xbound = 0.00001f;
					}
				}
				else{
					if(StoryTex.width+Xbound >=Screen.width)
						Xbound -=0.5f*(Screen.width/502)*(StoryTex.width/1024);
					if(StoryTex.width+Xbound <=Screen.width+2 && StoryTex.width+Xbound >=Screen.width-2 )
						if(!w.IsRunning)
							w.Start();
					if(w.ElapsedMilliseconds >= 2000){
						state = -4;
						w.Reset();
					}
				}

				if(Input.GetMouseButtonDown(0)){
						state = -4;
						w.Reset();
				}

				break;
				case -4:

				if(!w.IsRunning)
				{
					//	Rect R = new Rect(1,Screen.height - Screen.height/10 - 1,Screen.width/5,Screen.height/10);
					//	Rect R2 = new Rect(50 + Screen.width/5,Screen.height - Screen.height/10 - 1,Screen.width/5,Screen.height/10);
					Rect R3 =  new Rect(Screen.width * 4 / 6+ Screen.width * 1 / 12,Screen.height - Screen.height/16,Screen.width/13,Screen.height/16);

					Event currentEvent = Event.current;
					if (currentEvent.isMouse) {
						if(!R3.Contains(currentEvent.mousePosition))
						{
							if(Input.GetMouseButtonDown(0))
							{
								makeTiles(pos);
								w.Start();
								guiC = GUI.color;
							}
							else
							{
						//		makeTiles(pos);
						//		w.Start();
						//		guiC = GUI.color;
							}
							
						}
					}

			//		else if(Input.anyKeyDown)
			//		{
			//			w.Start();
			//			guiC = GUI.color;
			//		}

	/*					
					Rect R = new Rect(1,Screen.height - Screen.height/10 - 1,Screen.width/5,Screen.height/10);
					Rect R2 = new Rect(50 + Screen.width/5,Screen.height - Screen.height/10 - 1,Screen.width/5,Screen.height/10);
					if(Input.touchCount > 0){
						if(!R3.Contains(Input.GetTouch(0).position))
						{
							w.Start();
							makeTiles(pos);
							guiC = GUI.color;
						}
					}
					else if(Input.anyKeyDown){
						w.Start();
						makeTiles(pos);
						guiC = GUI.color;
					}
	*/

				
				
				}
				if(w.ElapsedMilliseconds >= 1000)
					state = -3;
				break;
				case -3:
				if(w.ElapsedMilliseconds >= 1400)
					state = -2;
				break;
				case -2:
				if(w.ElapsedMilliseconds >= 1500){
					state = -1;
					Xbound = 0;
				}
				break;
				case -1: 

				if(w.ElapsedMilliseconds <= 6000 && w.ElapsedMilliseconds >= 2500){
					Xbound -=3f * (Screen.width/502); Xbound2 -=4.4f * (Screen.width/502) ;
					YBound -=0.5f * (Screen.height/352); YBound2 -=4.4f * (Screen.height/352);
				}
				if(w.ElapsedMilliseconds <= 6000 && w.ElapsedMilliseconds >= 3500){
					if(guiC.r > 0)
						guiC.r -= 0.01f;
					if(guiC.g > 0 )
						guiC.g -= 0.01f;
					if(guiC.b > 0)
						guiC.b -= 0.01f;
					//	guiC.a += 0.01f;
				}
				if(w.ElapsedMilliseconds > 6000){
					state = 0;
					w.Reset();
				}
				
				break;
				case 0:
				
				state = 1;
				int x = i1;
				i1 = i2;
				i2 = x;
				qwerty = ((ArrayList)ExistingCorridor[i1]).Count;
				
				tempQuat = trans.rotation;
				pg = (GameObject)ExistingCorridor[i1][CorridorLength - 1]; 
				
				//if(!AutoPilot && !WayLightMode)
				if(lastGroup != "MyNameSpace.BonusGroup"){
					generateItems(currentGroup);
					setCoins(currentGroup);
				}
				else{
					generateItems(BonusGroup);
					setCoins(BonusGroup);
				}
				if(TutorialMode && TutorialCurrentCorr == 8)
					setCoins(TutorialGroups[7] as Group);

				if(currentOrb != null)
					OrbDistance --;
				
				if(AutoPilot)
					if(--autoPilotDur == 0)
				{
					AutoPilot = false;
					RunnerSpeed -=0.06f;
				}
				if(WayLightMode)
					if(--wayLightDur == 0)
						WayLightMode = false;
				
				if(WayLight)
					WayLight = false;
				
				break;
				case 1:

				if(!audio.isPlaying)
					audio.Play();

				PlayerScore ++;
				distance += RunnerSpeed;
				
				counter++;
				if(counter <= 30){
					trans.Rotate(Vector3.up / 7.5f);
					if(counter <= 15)
						trans.Rotate(Vector3.right / 7.5f);
					else
						trans.Rotate(-1 * Vector3.right / 7.5f);
				}
				else if(counter <60 && counter >30){
					trans.Rotate(-1 *Vector3.up / 7.5f);
					if(counter <= 45)
						trans.Rotate(Vector3.right / 7.5f);
					else
						trans.Rotate(-1 * Vector3.right / 7.5f);
				}
				if(counter == 30)
					SFX.PlayOneShot(FootStepSFXs[SoundRandom.Next(0,4)],SoundVolume/5);
				if(counter == 60){
					counter = 0;
					SFX.PlayOneShot(FootStepSFXs[SoundRandom.Next(4,8)],SoundVolume/5);
				}


				if(currentOrb!= null)
					checkDist();
				
				//	print ("distance : "+distance+" min : "+tileSize*(CorridorLength) * 4/5);
				if (distance > tileSize*(CorridorLength - 5)){
					if(!AutoPilot){
						if(Input.touches.Length <= 0)
						{
							if (Input.GetKeyDown("up") || Input.GetKeyDown("right") || Input.GetKeyDown("left"))
							{
								anyKeyPressed = true;
								SetRotDir();
							}
						}
						else{

							if (getSwipeDirection() == 1)
								anyKeyPressed = false;

						}
					}
				
					else if(!piloted)
						Pilot();
					
				}
				
				if (distance > tileSize*(CorridorLength) * 4/5)
					if(WayLightMode)
						if(!WayLight)
					{
						WayLight = true;
						Pilot();
					}


				if (distance > tileSize*(CorridorLength) - 2)
				{
					state = 2;
					qwerty = ((ArrayList)ExistingCorridor[i1]).Count;
					w.Start();
					for (int i = 1; i < Math.Ceiling(2 / RunnerSpeed) + 1; i++)
						trans.Translate(new Vector3(0, 0, 1) * RunnerSpeed *2);
					tempPos = trans.position;
					for (int i = 1; i < Math.Ceiling(2 / RunnerSpeed) + 1; i++)
						trans.Translate(new Vector3(0, 0, -1) * RunnerSpeed *2);
					trans.Translate(new Vector3(0, 0, 1) * 1f);
				}


				if(TutorialMode && !TutCorrPassed){
					if(TutorialCurrentCorr<=3)
					{
						if (distance > tileSize*(CorridorLength) - 4 ){
							GameIsPaused = true;
							state = 20;
						}
					}
					else if(TutorialCurrentCorr==4)
					{
						if (distance > tileSize*(CorridorLength / 2) ){
							GameIsPaused = true;
							state = 20;
						}
					}
					else if(TutorialCurrentCorr==6)
					{
						if(distance < tileSize * (int) halfTiles[0] - 1 && distance > tileSize * ((int) halfTiles[0] - 2) )
						{
							GameIsPaused = true;
							state = 20;
						}
					}
					else if(TutorialCurrentCorr==7)
					{
						if(distance < tileSize * (int) GlowingTiles[0] - 1 && distance > tileSize * ((int) GlowingTiles[0] - 2) )
						{
							GameIsPaused = true;
							state = 20;
							qwerty = (int) GlowingTiles[0];
						}
					}
				}




				if(!AutoPilot){			
					if(GlowingTiles.Count > 0)
					{
						for(int j = 0;j< CAM.GlowingTiles.Count;j++){

							if(distance < tileSize * (int) GlowingTiles[j] && distance > tileSize * ((int) GlowingTiles[j] - 3) )
								if(Input.touches.Length <= 0){
									if (Input.GetKeyDown("up") || Input.GetKeyDown("right") || Input.GetKeyDown("left")){
									shouldGoGlow = true;
									qwerty = (int) GlowingTiles[j];
									//	print ("qerty "+ qwerty);
									//         anyKeyPressed = true;
									SetRotDir();
									//		state = 2;
									}
								}
								else{
									shouldGoGlow = true;
									qwerty = (int) GlowingTiles[j];
									getSwipeDirection();

							}

						}

					}
					
					if(shouldGoGlow)
					if(GlowCheck()){
						if(distance < tileSize * qwerty && distance > tileSize * (qwerty - 1) ){
							anyKeyPressed = true;
							state = 2;
							for (int i = 1; i < Math.Ceiling(2 / RunnerSpeed) + 1; i++)
								trans.Translate(new Vector3(0, 0, 1) * RunnerSpeed *2);
							tempPos = trans.position;
							for (int i = 1; i < Math.Ceiling(2 / RunnerSpeed) + 1; i++)
								trans.Translate(new Vector3(0, 0, -1) * RunnerSpeed *2);
							trans.Translate(new Vector3(0, 0, 1) * 1f);
						}
					}
					else{
						qwerty = ((ArrayList)ExistingCorridor[i1]).Count;
						anyKeyPressed = false;
						shouldGoGlow = false;
					}
					
				}
				
				

					for(int j = 0;j< CAM.halfTiles.Count;j++){
						if(distance < tileSize * (int) halfTiles[j] && distance > tileSize * ((int) halfTiles[j] - 1) ){
							if(shouldSlide){
								anyKeyPressed = true;
								state = 4;
							}
						}
						if(distance < tileSize * ((int) halfTiles[j] + 1)+ 1 && distance > tileSize * ((int) halfTiles[j] + 1) - 1 ){
							if(!shouldSlide){
								gameOver = true;
								if(PlayerScore>HighScore){
									HighScore = PlayerScore;
									highScoreSet = true;
									writeSettingsFile();
								}
								ww.Start();
							}
						}
					}	

				if(halfTiles.Count > 0){
					for(int j = 0;j< CAM.halfTiles.Count;j++)
						if(distance < tileSize * (int) halfTiles[j] - 1 && distance > tileSize * ((int) halfTiles[j] - 3) )
						if(!AutoPilot){
							
							if(Input.touches.Length <= 0)
							{
								if (Input.GetKeyDown("down"))
								{
									shouldSlide = true;
								}

							}
							else{

								if (getSwipeDirection() == 1)
									shouldSlide = true;
							}



						}
					else state = 4;
					
				}
				
				tempQuatt = trans.rotation;
				trans.rotation = tempQuat;
				trans.Translate(new Vector3(0, 0, 1) * RunnerSpeed*2);
				trans.rotation = tempQuatt;
				
				break;
				case 2:
				counter = 0;
				trans.rotation = tempQuat;
				if (!anyKeyPressed)
				{
					
					if (w.ElapsedMilliseconds >= 300 && w.ElapsedMilliseconds <= 320)
						if(!shouldGoGlow)
							CheckWin();
					if (w.ElapsedMilliseconds <= 400)
					{
						state = 2;
						trans.Translate(new Vector3(0, 0, 1) * RunnerSpeed *2);
					}
					else
					{
						shouldGoGlow = false;
						w.Reset();
						DestroyTiles(i1);
						state = 0;
						qwerty = -1;
						pos = setPosition();
						makeTiles(pos);
						distance = 0f;
						trans.Translate(new Vector3(0, 0, 1) * -3.4f);
						SFX.PlayOneShot(SFXs[4],SoundVolume/2);
					}
				}
				else
				{
					
					state = 3;
					w.Reset();
					setDirection();
					makeTiles(setPosition2());
					Rotate2();
					for(int i = 1;i <= ((ArrayList)ExistingCorridor[i2]).Count; i++)
						((GameObject)ExistingCorridor[i2][i-1]).transform.rotation = g.transform.rotation;
					
				}    
				
				break;
				case 3:
				
				counter++;
				if (counter == 22)
					if(!shouldGoGlow){
						if(CheckWin())
							SFX.PlayOneShot(SFXs[4],SoundVolume/2);
					}
					else
						SFX.PlayOneShot(SFXs[4],SoundVolume/2);


				if (counter == 46)
				{
					state = 0;
					counter = 0;
					distance = 4f;
					Correct();
					anyKeyPressed = false;
					DestroyTiles(i1);
				}
				else
					RotateAndMove();
				
				break;
				case 4:
				if(Hcounter == 0){
					trans.rotation = tempQuat;
					//	Vector3 tt = trans.position;
					//	tt.y = trans.position.y - 1.7f;
					//	tt.z = trans.position.z - 3.76f;
					//		((GameObject)Instantiate(CharacterPrefab,tt,CharacterPrefab.transform.rotation)).transform.parent = trans;
				}
				distance += 0.1f;
				Hcounter++;
				
				trans.Rotate(-xx,0,0);
				trans.Translate(Vector3.forward * 0.2f);


				if(Hcounter <= 34){
					xx -= 2;
					trans.Rotate(xx,0,0);
					trans.Translate(Vector3.down * 0.07f);
				}
				
				else{
					xx += 2;
					trans.Rotate(xx,0,0);
					trans.Translate(Vector3.up * 0.07f);
				}
				
				if(Hcounter == 68)
				{
					state = 1;
					Hcounter = 0;
					shouldSlide = false;
					anyKeyPressed = false;
				}
				
				break;
				case 20:

				if(!w.IsRunning)
				{
					w.Start();
				}

			//	TutMsgX -= 5 * (Screen.width/502);

			
				if(TutorialCurrentCorr != 4 && TutorialCurrentCorr != 3 && TutorialCurrentCorr !=6 )
				if(w.ElapsedMilliseconds >= 3000)
				{
					if(Input.touches.Length <= 0)
					{
						if (Input.GetKeyDown("up") || Input.GetKeyDown("right") || Input.GetKeyDown("left"))
						{
							anyKeyPressed = true;
							SetRotDir();
							if(TutorialCurrentCorr != 7)
							{
								if(CheckWin())
									FinalizeTutorialStage();
							}
							else if (TutorialCurrentCorr == 7)
								if(GlowCheck())
								{
									shouldGoGlow = true;
									FinalizeTutorialStage();
								}
										
						}
					}
					else{
						if (getSwipeDirection() == 1)
							anyKeyPressed = false;

						if(TutorialCurrentCorr != 7)
						{
							if(CheckWin())
								FinalizeTutorialStage();
						}
						else if(TutorialCurrentCorr == 7)
						{
							if(GlowCheck()){
								shouldGoGlow = true;
								FinalizeTutorialStage();
							}
						}
					}
				}



				if(TutorialCurrentCorr == 6)
				if(w.ElapsedMilliseconds >= 2000)
				{
					if(Input.touches.Length <= 0)
					{
						if (Input.GetKeyDown("down"))
						{
							shouldSlide = true;
							FinalizeTutorialStage();
						}
						
					}
					else{
						if (getSwipeDirection() == 1)
						{
							shouldSlide = true;
							FinalizeTutorialStage();
						}
					}
				}

				if(TutorialCurrentCorr == 7)
					if(w.ElapsedMilliseconds >=15000)
					{
						FinalizeTutorialStage();
						TutorialMode = false;
						TutorialCurrentCorr = 0;
						PlayerScore = 0;
						showGLmsg = true;
					}

				if(TutorialCurrentCorr == 4){
				//	if(w.ElapsedMilliseconds >=8000)
				//		FinalizeTutorialStage();
					if(Input.GetMouseButtonDown(0))
					{
						if(TutMsg.Equals("Sometimes there are more than one wall with the same color as the ground. You can swipe in the direction of every wall with that color"))
							TutMsg = "(or just go straight without swiping if the front wall has the same color as the ground).";
						else
							FinalizeTutorialStage();
					}

				}

				if(TutorialCurrentCorr == 3)
					if(w.ElapsedMilliseconds >=7000)
						FinalizeTutorialStage();

				if(TutorialCurrentCorr == 1)
					if(w.ElapsedMilliseconds >=15000)
						if(CheckWin())
							FinalizeTutorialStage();

			//	if(w.ElapsedMilliseconds >= 10000 && w.ElapsedMilliseconds <=10100)
			//		TutMsgX = 0;

				break;
				case 10:

				if(!w.IsRunning)
					w.Start();


				if(w.ElapsedMilliseconds >= 1000)
					CreditTex = Resources.Load("start b") as Texture2D;

				if(w.ElapsedMilliseconds >= 1400)
					CreditTex = Resources.Load("credit b") as Texture2D;

				if(w.ElapsedMilliseconds >= 1700){
					CreditTex = Resources.Load("credit a") as Texture2D;
					creditText = "Pixel Run";
				}


				if(w.ElapsedMilliseconds >= 4000)
					CreditTex = Resources.Load("credit b") as Texture2D;

				if(w.ElapsedMilliseconds >= 4300){
					CreditTex = Resources.Load("credit a") as Texture2D;
					creditText = "Designers:";
					creditText2 = "Kasra Rahimi  Shayan Amiri";
				}


				if(w.ElapsedMilliseconds >= 6600)
					CreditTex = Resources.Load("credit b") as Texture2D;

				if(w.ElapsedMilliseconds >= 6900){
					CreditTex = Resources.Load("credit a") as Texture2D;
					creditText = "2D Artist:";
					creditText2 = "Erfan Malek Hoseyni";
				}


				if(w.ElapsedMilliseconds >= 9200)
					CreditTex = Resources.Load("credit b") as Texture2D;

				if(w.ElapsedMilliseconds >= 9500){
					CreditTex = Resources.Load("credit a") as Texture2D;
					creditText = "3D Artist:";
					creditText2 = "Elnaz Mirzaei";
				}


				if(w.ElapsedMilliseconds >= 11800)
					CreditTex = Resources.Load("credit b") as Texture2D;

				if(w.ElapsedMilliseconds >= 12100){
					CreditTex = Resources.Load("credit a") as Texture2D;
					creditText = "Music & Sound:";
					creditText2 = "Navid Nazarzadeh";
				}


				if(w.ElapsedMilliseconds >= 14400)
					CreditTex = Resources.Load("credit b") as Texture2D;

				if(w.ElapsedMilliseconds >= 14700){
					CreditTex = Resources.Load("credit a") as Texture2D;
					creditText = "Programmer:";
					creditText2 = "AMH";
				}

				if(w.ElapsedMilliseconds >= 17000)
					CreditTex = Resources.Load("credit b") as Texture2D;
				
				if(w.ElapsedMilliseconds >= 17100){
					CreditTex = Resources.Load("start b") as Texture2D;
					creditText = " ";
					creditText2 = " ";
				}

				if(w.ElapsedMilliseconds >= 17300){
					CreditTex = Resources.Load("start a") as Texture2D;
					w.Reset();
					state = -4;

				}


				break;
			}
			
			else if (gameOver)
			{
				switch(LosingState){
				case 0:
					
					LCounter++;
					if(!anyKeyPressed)
					{
						if(LCounter <= 1)
							trans.Translate(new Vector3(0, 0, 1) * -3f);
					}
					else if(lastKeyPressed == "up")
					{
						if(LCounter <= 7)
						{
							trans.Translate(new Vector3(0, 0, 1) * -0.4f);
							trans.Translate(new Vector3(0, 1, 0) * -0.2f);
							trans.Rotate( new Vector3(1,0,0) * -5f);
						}
					}
					
					else if(lastKeyPressed == "right")
					{
						if(LCounter <= 1)
						{
							trans.Translate(new Vector3(0, 0, 1) * -2f);
							trans.Translate( Vector3.right * -0.5f);
						}
						if(LCounter <= 7)
							trans.Rotate( new Vector3(0,1,0) * 3f);
						trans.Rotate(new Vector3(0,0,1) * 4f);
					}
					
					else if(lastKeyPressed == "left")
					{
						if(LCounter <= 1)
						{
							trans.Translate(new Vector3(0, 0, 1) * -2f);
							trans.Translate( Vector3.right * 0.5f);
						}
						if(LCounter <= 7)
							trans.Rotate( new Vector3(0,1,0) * -3f);
						trans.Rotate(new Vector3(0,0,1) * -4f);
					}
					
					
					if(LCounter == 1)
					{
						DoShake(0.05f,0.02f);
						tempLS = 0;
						LosingState = 2;
						SFX.PlayOneShot(SFXs[3],SoundVolume/2);
						//SFX.PlayOneShot(HumanVoiceSFXs[SoundRandom.Next(4,7)],SoundVolume/2);
					}
					
					if(LCounter <= 7)
						trans.Translate(new Vector3(0, 1, 0) * -0.3f);
					else
					{
						LosingState = 2;
						DoShake(0.6f,0.02f);
						tempLS = 1;
						SFX.PlayOneShot(SFXs[2],SoundVolume);
						SFX.PlayOneShot(HumanVoiceSFXs[SoundRandom.Next(0,4)],SoundVolume/2);
					}
					
					break;
				case 1:
					
			//		if(Input.GetKeyDown(KeyCode.Space) || Input.touches.Length >0)
			//		{
			//			StartState = -4;
			//			RestartGame();
					//	Application.LoadLevel(Application.loadedLevelName);
					//	state = -4;
					//	ww.Reset();
					//	gameOver = false;
			//		}
					
					break;
				case 2 :
					
					if(ShakeIntensity > 0)
					{
						trans.position = OriginalPos + UnityEngine.Random.insideUnitSphere * ShakeIntensity;
						trans.rotation = new Quaternion(OriginalRot.x + UnityEngine.Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
						                                OriginalRot.y + UnityEngine.Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
						                                OriginalRot.z + UnityEngine.Random.Range(-ShakeIntensity, ShakeIntensity)*.2f,
						                                OriginalRot.w + UnityEngine.Random.Range(-ShakeIntensity, ShakeIntensity)*.2f);
						
						ShakeIntensity -= ShakeDecay;
					}
					else if (Shaking)
					{
						Shaking = false;  
						LosingState = tempLS;
					}
					
					break;
				}		
			}
			
		}

		private void FinalizeTutorialStage(){
			state = 1;
			TutCorrPassed = true;
			GameIsPaused = false;
			w.Reset();

			if(TutorialCurrentCorr == 3)
				TutMsg = "Sometimes there are more than one wall with the same color as the ground. You can swipe in the direction of every wall with that color";
		}
		private int getSwipeDirection(){


			if(Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Began)){
				if (swiping == false){
					swiping = true;
					lastPosition = Input.GetTouch(0).position;
				}
			}
			
			if(Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Moved)){

				Vector2 direction = Input.GetTouch(0).position - lastPosition;
				anyKeyPressed = true;
				
				if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
					if (direction.x > 0) 
						lastKeyPressed = "right";
					else
						lastKeyPressed = "left";
				}
				else{
					if (direction.y > 0)
						lastKeyPressed = "up";
						else
						return 1;
				}
			}
/*			
			float xDif = touchEnd.position.x - touchBegin.position.x;
			float yDif = touchEnd.position.y - touchBegin.position.y;
			
			float ang = (float) (Math.Atan2(yDif,xDif) * 180.0 / Math.PI);

			
			if( ang > 315 && ang <= 45)
				lastKeyPressed = "right";
			else if( ang > 45 && ang <= 135)
				lastKeyPressed = "up";
			else if( ang > 135 && ang <= 225)
				lastKeyPressed = "left";
			else if( ang > 225 && ang <= 315)
				return 1;
*/			
			return 0;
			
		}
		
		public static string Pilot(){
			piloted = true;
			GameObject pass_top = pg.transform.Find("top").gameObject;
			GameObject pass_bottom = pg.transform.Find("bottom").gameObject;
			GameObject pass_left = pg.transform.Find("left").gameObject;
			GameObject pass_right = pg.transform.Find("right").gameObject;
			GameObject pass_end = pg.transform.Find("endTile").gameObject;
			
			AutoPilotResluts.Clear();
			if(pass_bottom.renderer.material.color.ToString() == pass_top.renderer.material.color.ToString())
				AutoPilotResluts.Add("up");
			if(pass_bottom.renderer.material.color.ToString() == pass_left.renderer.material.color.ToString())
				AutoPilotResluts.Add("left");
			if(pass_bottom.renderer.material.color.ToString() == pass_right.renderer.material.color.ToString())
				AutoPilotResluts.Add("right");
			if(pass_bottom.renderer.material.color.ToString() == pass_end.renderer.material.color.ToString())
				AutoPilotResluts.Add("endTile");
			
			int a = (new System.Random()).Next(AutoPilotResluts.Count);
			
			if(AutoPilot){
				anyKeyPressed = true;
				
				if(((String) AutoPilotResluts[a]) != "endTile")
					lastKeyPressed = AutoPilotResluts[a] as string;
				else
					anyKeyPressed = false;
			}
			
			return AutoPilotResluts[a] as string;
		}
		
		private void checkDist(){
			if(Orb.state != 2){
				if(Vector3.Distance(trans.position,currentOrb.transform.position) > (OrbDistance / 3) * 14f){
					if (Orb.speed > Orb.speedThrL)
						Orb.speed /= 2;
				}
				else if (Orb.speed < Orb.speedThr)
					Orb.speed *= 2;
			}
			
			if(Vector3.Distance(trans.position,currentOrb.transform.position) <= 5)
				Destroy(currentOrb);
		}
		
		void OnTriggerEnter(Collider collider){
			if(collider.name == "ControlItem(Clone)"){
				AutoPilot = true;
				RunnerSpeed +=0.06f;
				Destroy(currentControlItem);
			}
			if(collider.name == "Orb(Clone)"){
				Destroy(currentOrb);
			}
			if(collider.name == "wayLight(Clone)"){
				WayLightMode = true;
				Destroy(currentWayLight);
			}
			if(collider.name == "COIN"){
				collectedCoins ++;
				PlayerScore += 100;
				SFX.PlayOneShot(SFXs[1],SoundVolume/2);
				Destroy(collider.gameObject.transform.parent.gameObject);
			}
		}
		
		private void DoShake(float f,float d)
		{
			
			OriginalPos = trans.position;
			OriginalRot = trans.rotation;
			
			ShakeIntensity = f;
			ShakeDecay = d;
			Shaking = true;
		}   
		
		private void Correct() {
			Vector3 angles = g.transform.rotation.eulerAngles;
			if (Math.Round(angles.x) != 0)
			{
				trans.Rotate(-1 * oldAngle);
				for(int i = 1;i <= ((ArrayList)ExistingCorridor[i2]).Count; i++)
					((GameObject)ExistingCorridor[i2][i-1]).transform.Rotate(-1 * oldAngle);
				tempPos = trans.position;
				corr(setPosition());
				trans.Translate(new Vector3(0, 0, 1) * 8f);
			}
		}
		
		private void corr(Vector3 v) {
			Vector3 post = new Vector3(v.x, v.y, v.z);
			for (int i = 1; i <=  ((ArrayList)ExistingCorridor[i2]).Count; i++)
			{
				if (upOrDown == 1)
					post.y += 5f;
				else if (upOrDown == 2)
					post.y -= 5f;
				else if (movingDirection == 1)
					post.z += 5f;
				else if (movingDirection == 2)
					post.x += 5f;
				else if (movingDirection == -1)
					post.z -= 5f;
				else if (movingDirection == -2)
					post.x -= 5f;
				
				((GameObject)ExistingCorridor[i2][i-1]).transform.position = post;
			} 
		}
		
		private void RotateAndMove() {
			Rotate();
			if (lastKeyPressed == "up")
			{
				trans.Translate(new Vector3(0, 0, 1) * 0.18f);
				trans.Translate(new Vector3(0, 1, 0) * 0.05f);
			}
			else if (lastKeyPressed == "right")
			{
				trans.Translate(new Vector3(0, 0, 1) * 0.18f);
				trans.Translate(new Vector3(1, 0, 0) * 0.04f);
			}
			else if (lastKeyPressed == "left")
			{
				trans.Translate(new Vector3(0, 0, 1) * 0.18f);
				trans.Translate(new Vector3(-1, 0, 0) * 0.035f);
			}
		}
		
		private void makeTiles(Vector3 v) {
			
			if(!noFile)
				Prepare();
			Vector3 post = new Vector3(v.x, v.y, v.z);
			for (int i = 1; i <= CorridorLength; i++)
			{
				if (upOrDown == 1)
					post.y += 2*tileSize; 
				else if (upOrDown == 2)
					post.y -= 2*tileSize; 
				else if (movingDirection == 1)
					post.z += 2*tileSize; 
				else if (movingDirection == 2)
					post.x += 2*tileSize; 
				else if (movingDirection == -1)
					post.z -= 2*tileSize; 
				else if (movingDirection == -2)
					post.x -= 2*tileSize; 
				
				ExistingCorridor[i2].Add((GameObject)Instantiate(tiles, post, Quaternion.identity));
				((GameObject)ExistingCorridor[i2][i-1]).transform.localScale = new Vector3(1f,1f,tileSize * 0.08f);
				((GameObject)ExistingCorridor[i2][i-1]).transform.rotation = trans.transform.rotation;
				((GameObject)ExistingCorridor[i2][i-1]).name = (i).ToString();
				
			}
			
			if (upOrDown == 1 || upOrDown == 2)
				upOrDown = 0;
		}
		
		private void DestroyTiles(int a)
		{
			for (int i = 1; i <=  ((ArrayList)ExistingCorridor[a]).Count; i++)
				Destroy((GameObject)ExistingCorridor[a][i-1]);
			((ArrayList)ExistingCorridor[a]).Clear();
		}
		
		private void SetRotDir() {
			if (Input.GetKeyDown("up"))
				lastKeyPressed = "up";
			else if (Input.GetKeyDown("down"))
				anyKeyPressed = false;
			else if (Input.GetKeyDown("right"))
				lastKeyPressed = "right";
			else if (Input.GetKeyDown("left"))
				lastKeyPressed = "left";
		}
		
		private void Rotate() {
			if (lastKeyPressed == "up" )
				trans.Rotate(-2, 0, 0);
			else if (lastKeyPressed == "down" )
				trans.Rotate(2, 0, 0);
			else if (lastKeyPressed == "right")
				trans.Rotate(0, 2, 0);
			else if (lastKeyPressed == "left" )
				trans.Rotate(0, -2, 0);
		}
		
		private void Rotate2() {
			g.transform.rotation = trans.rotation;
			
			if (lastKeyPressed == "up")
			{
				g.transform.Rotate(-90, 0, 0);
				oldAngle = new Vector3(-90, 0, 0);
			}
			else if (lastKeyPressed == "down")
			{
				g.transform.Rotate(90, 0, 0);
				oldAngle = new Vector3(90, 0, 0);
			}
			else if (lastKeyPressed == "right")
			{
				g.transform.Rotate(0, 90, 0);
				oldAngle = new Vector3(0, 90, 0);
			}
			else if (lastKeyPressed == "left")
			{
				g.transform.Rotate(0, -90, 0);
				oldAngle = new Vector3(0, -90, 0);
			}
			
		}
		
		private void setDirection() {
			upOrDown = 0;
			if (lastKeyPressed == "up")
				upOrDown = 1;
			else if (lastKeyPressed == "down")
				upOrDown = 2;
			else
			{
				switch (movingDirection)
				{
				case 1:
					if (lastKeyPressed == "right")
						movingDirection = 2;
					else
						movingDirection = -2;
					break;
				case -1:
					if (lastKeyPressed == "right")
						movingDirection = -2;
					else
						movingDirection = 2;
					break;
				case 2:
					if (lastKeyPressed == "right")
						movingDirection = -1;
					else
						movingDirection = 1;
					break;
				case -2:
					if (lastKeyPressed == "right")
						movingDirection = 1;
					else
						movingDirection = -1;
					break;
				}
			}
		}
		
		private Vector3 setPosition() {
			Vector3 v = new Vector3(0, 0, 0);
			if (movingDirection == 1)
				v = new Vector3(tempPos.x + 4.5f, tempPos.y + 2.5f, tempPos.z +1f);
			else if (movingDirection == 2)
				v = new Vector3(tempPos.x + 1f, tempPos.y + 2.5f, tempPos.z - 4.5f);
			else if (movingDirection == -1)
				v = new Vector3(tempPos.x - 4.5f, tempPos.y + 2.5f, tempPos.z -1f);
			else if (movingDirection == -2)
				v = new Vector3(tempPos.x - 1f, tempPos.y + 2.5f, tempPos.z + 4.5f);
			return v;
		}
		
		private Vector3 setPosition2()
		{
			//   print(lastKeyPressed + " " + movingDirection);
			Vector3 v = new Vector3(0, 0, 0);
			switch(movingDirection){
			case 1:
				if(lastKeyPressed == "up")
					v = new Vector3(tempPos.x + 4.5f, tempPos.y , tempPos.z - 1.2f);       
				else if (lastKeyPressed == "down")
					v = new Vector3(tempPos.x + 4.5f, tempPos.y + 0.1f, tempPos.z + 3.1f);
				else if (lastKeyPressed == "right")
					v = new Vector3(tempPos.x + 3.3f, tempPos.y + 2.5f, tempPos.z - 0.1f);
				else if (lastKeyPressed == "left")
					v = new Vector3(tempPos.x + 5.7f, tempPos.y + 2.5f, tempPos.z + 0.2f);
				
				break;
			case 2 :
				if (lastKeyPressed == "up")
					v = new Vector3(tempPos.x - 0.9f, tempPos.y , tempPos.z - 4.5f);       
				else if (lastKeyPressed == "down")
					v = new Vector3(tempPos.x + 3.7f, tempPos.y +0.3f, tempPos.z - 4.5f);  
				else if (lastKeyPressed == "right")
					v = new Vector3(tempPos.x + 0.2f, tempPos.y + 2.5f, tempPos.z - 3.6f);   
				else if (lastKeyPressed == "left")
					v = new Vector3(tempPos.x - 0.1f, tempPos.y + 2.5f, tempPos.z - 5.7f);
				
				break;
			case -1:
				if (lastKeyPressed == "up")
					v = new Vector3(tempPos.x - 4.5f, tempPos.y , tempPos.z + 1.1f);
				else if (lastKeyPressed == "down")
					v = new Vector3(tempPos.x - 4.5f, tempPos.y + 0.3f, tempPos.z - 3.5f);
				else if (lastKeyPressed == "right")
					v = new Vector3(tempPos.x - 3.4f, tempPos.y + 2.5f, tempPos.z + 0.1f); 
				else if (lastKeyPressed == "left")
					v = new Vector3(tempPos.x - 5.9f, tempPos.y + 2.5f, tempPos.z );   
				
				break;
			case -2:
				if (lastKeyPressed == "up")
					v = new Vector3(tempPos.x + 1.2f, tempPos.y , tempPos.z + 4.5f);
				else if (lastKeyPressed == "down")
					v = new Vector3(tempPos.x - 3.5f, tempPos.y + 0.25f , tempPos.z + 4.5f);
				else if (lastKeyPressed == "right")
					v = new Vector3(tempPos.x + 0.1f, tempPos.y + 2.5f, tempPos.z + 3.3f);
				else if (lastKeyPressed == "left")
					v = new Vector3(tempPos.x + 0.2f, tempPos.y + 2.5f, tempPos.z + 5.7f); 
				break;
			}
			return v;
		}
	}
}