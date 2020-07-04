using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;
//-********
using MahjongDeffine;
using MJDefsHeader;
using GameDefsHeader;
using MJDialogHeader;
//-********

//-*****************
// Mjemnt.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		アルゴ － 手牌の面子思考ルーチン
*/

//#include "MahJongRally.h"								// Module interface definitions


//int evlmnt(/*MahJongRally * pMe,*/ int x, int w, int mc);

public int emshunt(/*MahJongRally * pMe,*/ int x, int w, int mc)
{
	int val;

	cntbuf[x]--; cntbuf[x+1]--; cntbuf[x+2]--;
	val=evlmnt(x, w, mc)+SubMj.Sv[x];
	cntbuf[x]++; cntbuf[x+1]++; cntbuf[x+2]++;
	return val;
}

public int emkotsu(/*MahJongRally * pMe,*/ int x, int w, int mc)
{
	int val, v;

	cntbuf[x]-=3;
	val=evlmnt(x, w, mc)+SubMj.Kv[x];
	cntbuf[x]+=3;
	if(cntbuf[x+1] != 0 && cntbuf[x+2] != 0)
		return (v=emshunt(x, w, mc))>val ? v : val;
	return val;
}

public int evlmnt(/*MahJongRally * pMe,*/ int x, int w, int mc)
{
	int v, val;

	if(--mc<0)
		return 0;
	do {
		while(cntbuf[x]==0)
			x++;
		if(cntbuf[x]>=3)
			return emkotsu(x, w, mc);
		else if(cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
			val=emshunt(x, w, mc);
			if((w-=cntbuf[x++])<0)
				return val;
			if(cntbuf[x]>=3)
				return (v=emkotsu(x, w, mc))>val ? v : val;
			else if(cntbuf[x+2] != 0){
				if((v=emshunt(x, w, mc))>val)
					val=v;
				if((w-=cntbuf[x++])<0)
					return val;
				if(cntbuf[x]>=3)
					return (v=emkotsu(x, w, mc))>val ? v : val;
				else if(cntbuf[x+2] != 0 && (v=emshunt(x, w, mc))>val)
					return v;
				return val;
			} else {
				if((w-=cntbuf[x++])<0 || cntbuf[x]<3)
					return val;
				cntbuf[x]-=3;
				if((v=evlmnt(x, w, mc)+SubMj.Kv[x])>val)
					val=v;
				cntbuf[x]+=3;
				return val;
			}
		}
	} while((w-=cntbuf[x++])>=0);
	return MJDefine.FAULT;
}

public int	EvalMnt(/*MahJongRally * pMe,*/ int x, int w, int mc)
{
	if(x<0x30)
		return evlmnt(x, w, mc);
	return cntbuf[x]>=3 ? SubMj.Kv[x] : 0;
}

/**************************************END OF FILE**********************************************/

//-*********************Mjemnt.j
}