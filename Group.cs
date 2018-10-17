using System;
using System.Collections;

namespace MyNameSpace
{
	public class Group
	{
		protected int id;
		protected int length_min, length_max;
		protected int tilesSameAsFloor;
		protected int end_pat_rep;
		protected float tileSize;
		protected int GlowTilePercentage;
		protected int HalfTilePercentage;
		protected int halfTileDistFromFirst;
		protected int halfTileDistFromLast;
		protected int ControllingItemPercentage;
		protected int ControllingItemDuration;
		protected int OrbPercentage;
		protected int wayLightPercentage;
		protected int wayLightDuration;
		protected int SpikePercentage;
		protected int SpikeFrom;
		protected int SpikeUpto;
		protected int glowingTileBeforeSpike;
		protected int CoinPercentage;
		
		
		protected ArrayList colors = new ArrayList();
		
		public Group(){}
		
		public Group (int a,int lmin,int lmax,int tsf,int epr,float ts,ArrayList s,int gtp,int htp,int htf,int htu,int cip,int cid,int op,int wlp,int wld,int spp,int spf, int spu,int gtbsp,int cp){
			this.id = a;
			this.length_min = lmin;
			this.length_max = lmax;
			this.tilesSameAsFloor = tsf;
			this.end_pat_rep = epr;
			this.tileSize = ts;
			this.GlowTilePercentage = gtp;
			this.HalfTilePercentage = htp;
			this.halfTileDistFromFirst = htf;
			this.halfTileDistFromLast = htu;
			this.ControllingItemPercentage = cip;
			this.ControllingItemDuration = cid;
			this.OrbPercentage = op;
			this.wayLightPercentage = wlp;
			this.wayLightDuration = wld;
			this.SpikePercentage = spp;
			this.SpikeFrom = spf;
			this.SpikeUpto  =spu;
			this.glowingTileBeforeSpike = gtbsp;
			this.CoinPercentage = cp;
			
			for(int i = 0;i<s.Count;i++)
				this.colors.Add(s[i]);
		}
		
		public ArrayList getColors() {
			return this.colors;
		}
	
		public float getTileSize() {
			return this.tileSize;
		}
	
		public int getLength_min() {
			return this.length_min;
		}
	
		public int getLength_max() {
			return this.length_max;
		}
	
		public int getTilesSameAsFloor() {
			return this.tilesSameAsFloor;
		}
	
		public int getPatternRep() {
			return this.end_pat_rep;
		}
		
		public int getID() {
			return this.id;
		}
		
		public int getGlowingTilePercentage(){
			return this.GlowTilePercentage;
		}
		
		public int getHtp(){
			return this.HalfTilePercentage;
		}
		
		public int getHtf(){
			return this.halfTileDistFromFirst;
		}
		
		public int getHtu(){
			return this.halfTileDistFromLast;
		}
		
		public int getCip(){
			return this.ControllingItemPercentage;
		}
		
		public int getCid(){
			return this.ControllingItemDuration;
		}
		
		public int getOp(){
			return this.OrbPercentage;
		}
		
		public int getWlp(){
			return this.wayLightPercentage;
		}
		
		public int getWld(){
			return this.wayLightDuration;
		}
		
		public int getSpp(){
			return this.SpikePercentage;
		}
		
		public int getSpf(){
			return this.SpikeFrom;
		}
		
		public int getSpu(){
			return this.SpikeUpto;
		}
		
		public int getGtbsp(){
			return this.glowingTileBeforeSpike;
		}
		
		public int getCp(){
			return this.CoinPercentage;
		}
	}
}