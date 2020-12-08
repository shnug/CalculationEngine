using System;

namespace CalcEngine.Phrase
{
	/// <summary>
	/// ¥ ¿‡–Õ
	/// </summary>
	public enum PhraseType:int
	{
		unknown=0,
		ln=1,
		lg=2,
		log=3,
		pow=4,		//a^b
		cbrt=6,		//a^-0.5
		sbrt=7,		//a^-1/3
		fact=8,
		sin=10,
		cos=11,
		asin=12,
		acos=13,
		tg=14,
		ctg=15,
		atg=16,
		actg=17,
		plus=18,
		minus=19,
		mutiple=20,
		divide=21,
		mod=23,
		leftbracket=24,		//(
		rightbracket=25,	//)
		ans=26,		//variable ans
		sto=27,		//save to var
		clr=28,		//clear vars
		e=35,
		pi=36,
		number=37,
		sharp=38,
        negative=39,    //negative
        positive=40,     //positive
        variable=41,
        dataprovider=42,
        fieldname=43,    //dataprovider field id
	}
}
