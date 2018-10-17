using System;
using System.Collections;

namespace MyNameSpace
{
	public class BonusGroup : Group
	{
		private int chance = 0;
		
		public BonusGroup (int chnc,int a,int lmin,int lmax,int tsf,int epr,float ts,ArrayList s,int gtp,int htp,int htf,int htu,int cip,int cid,int op,int wlp,int wld,int spp,int spf, int spu,int gtbsp,int cp){
			this.chance = chnc;
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
		
		public int getOccurChance(){
			return this.chance;
		}
	}
}

