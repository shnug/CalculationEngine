using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;

namespace CalcEngine.Phrase
{
    /// <summary>
    /// 词法分析类
    /// </summary>
	public class PhraseAnalyzer
	{
        private static DFAState _prestate;			//DFA的前一个状态
        private static char[] _chArray = null;		//句子变量的字符串形式

        private static void SavePhrase(PhraseStorage _ps, int startpos, int endpos, DFAState prestate, DFAState curstate, string _sentence)
		{
			string temp=null;
            //处理'@'
            if (startpos == endpos && _chArray[startpos] == '@')
            {
                _ps.AddPhraseResult("@", PhraseType.negative);
                return;
            }
            //处理其他词元
			if(endpos>=0 && startpos>=0 && endpos>=startpos)
			{
                //trim()，以防止在startpos到endpos头尾出现空格
				temp=_sentence.Substring(startpos,endpos-startpos+1).Trim().ToLower();
                if (curstate == DFAState.S17)
                    _ps.AddPhraseResult(temp, PhraseType.dataprovider);
                else if (curstate == DFAState.S18)
                    _ps.AddPhraseResult(temp, PhraseType.fieldname);
                else if (curstate == DFAState.S16)
                    _ps.AddPhraseResult(temp, PhraseType.variable);
                else
                    _ps.AddPhraseResult(temp, PhraseAnalyzer.StrToType(temp));
			}
		}
        
		public static PhraseType StrToType(string str)
		{
			switch(str)
			{
				case "sin": return PhraseType.sin;
				case "cos": return PhraseType.cos;
				case "ln": return PhraseType.ln;
				case "lg": return PhraseType.lg;
				case "log": return PhraseType.log;
				case "^": return PhraseType.pow;
				case "cbrt": return PhraseType.cbrt;
				case "sbrt": return PhraseType.sbrt;
				case "asin": return PhraseType.asin;
				case "acos": return PhraseType.acos;
				case "!": return PhraseType.fact;
				case "tg": return PhraseType.tg;
				case "ctg": return PhraseType.ctg;
				case "atg":return PhraseType.atg;
				case "actg":return PhraseType.actg;
				case "+":return PhraseType.plus;
				case "-":return PhraseType.minus;
				case "*":return PhraseType.mutiple;
				case "/":return PhraseType.divide;
				case "%":return PhraseType.mod;
				case "(":return PhraseType.leftbracket;
				case ")":return PhraseType.rightbracket;
				case "#":return PhraseType.sharp;
				case "ans":return PhraseType.ans;
				case "sto":return PhraseType.sto;
				case "clr":return PhraseType.clr;
				case "e": return PhraseType.e;
				case "pi": return PhraseType.pi;
                case "@": return PhraseType.negative;
				default: 
                    double a;
                    bool isnum=double.TryParse(str,out a);
                    if (isnum)
                        return PhraseType.number;

                    throw new PhraseException(string.Format("Unrecognizable symbol: {0}",str));    
			}
		}
        public static string TypeToStr(PhraseType type)
        {
            switch (type)
            {
                case PhraseType.sin:
                case PhraseType.cos:
                case PhraseType.ln:
                case PhraseType.lg:
                case PhraseType.log:
                case PhraseType.cbrt:
                case PhraseType.sbrt:
                case PhraseType.asin:
                case PhraseType.acos:
                case PhraseType.tg:
                case PhraseType.ctg:
                case PhraseType.atg:
                case PhraseType.actg:
                case PhraseType.ans:
                case PhraseType.sto:
                case PhraseType.clr:
                case PhraseType.e:
                case PhraseType.pi:
                    return type.ToString();

                case PhraseType.pow: return "^";
                case PhraseType.fact: return "!";
                case PhraseType.plus: return "+";
                case PhraseType.minus: return "-";
                case PhraseType.mutiple: return "*";
                case PhraseType.divide: return "/";
                case PhraseType.mod: return "%";
                case PhraseType.leftbracket: return "(";
                case PhraseType.rightbracket: return ")";
                case PhraseType.sharp: return "#";
                case PhraseType.negative: return "- (negative)";
                default:
                    return null;
            }
        }
        /// <summary>
        /// 字符串类型检查
        /// </summary>
        /// <param name="startpos">字符串开始位置</param>
        /// <param name="endpos">字符串结束位置</param>
        /// <returns>字符串是否匹配规定范围内容的类型</returns>
		private static bool CheckString(int startpos,int endpos,string _sentence)
		{
            //trim()，以防止在startpos到endpos头尾出现空格
			string temp=_sentence.Substring(startpos,endpos-startpos+1).Trim().ToLower();
            int len = temp.Length;	//这里的temp.length不一定等于endpos-startpos+1
			if(len==1)
			{
				switch(temp)
				{
					case "e":
                        //BUGFIX:处理ex
                        //向后看一个字符，如果为x，就认为是ex
                        if(_chArray.Length>endpos+1&&_chArray[endpos+1]=='x')
                        {
                            return false;
                        }
						return true;
				}
			}
			else if(len==2)
			{
				switch(temp)
				{
					case "ln":
					case "lg":
					case "tg":
					case "pi":
						return true;
				}
			}
			else if(len==3)
			{
				switch(temp)
				{
					case "cos":
					case "sin":
					case "ctg":
					case "atg":
					case "ans":
					case "clr":
					case "sto":
					case "log":
						return true;
				}
			}
			else if(len==4)
			{
				switch(temp)
				{
					case "acos":
					case "asin":
					case "actg":
					case "sbrt":
					case "cbrt":
						return true;
				}
			}
			return false;
		}
        /// <summary>
        /// run phrase analysis
        /// </summary>
        public static void Analyze(string _sentence,PhraseStorage _ps)
		{
            _chArray = _sentence.ToLower().ToCharArray();

            int i = 0;
            int startpos = 0, endpos = 0;
            //设置初态
            _prestate = DFAState.S0;
            while (i < _chArray.Length)
            {
                //if (_prestate == DFAState.SX)
                //{
                //    return false;
                //}

                if (Char.IsLetter(_chArray[i]))
                {
                    if (_prestate == DFAState.S0)
                    {
                        //initial state to string state
                        _prestate = DFAState.S3;
                    }
                    else if (_prestate == DFAState.S3)  //the previous state is string state
                    {
                        endpos = i - 1;

                        //check if the string is in the predefined list
                        if (CheckString(startpos, endpos,_sentence) == true)
                        {
                            SavePhrase(_ps,startpos, endpos,_prestate,DFAState.SX,_sentence);
                            startpos = i;
                        }
                    }
                    else if (_prestate == DFAState.S16 || _prestate == DFAState.S17 || _prestate == DFAState.S18)
                    { 
                        //do nothing
                    }
                    else
                    {
                        //handle the previous char
                        endpos = i - 1;

                        SavePhrase(_ps, startpos, endpos, _prestate, DFAState.S3, _sentence);

                        _prestate = DFAState.S3;

                        startpos = i;
                    }
                }
                else if (Char.IsDigit(_chArray[i]))
                {
                    if (_prestate == DFAState.S0)
                    {
                        //initial state
                        _prestate = DFAState.S1;
                        startpos = i;	
                    }
                    else if (_prestate == DFAState.S1)
                    { 
                    
                    }
                    else if (_prestate == DFAState.S3)
                    {
                        endpos = i - 1;

                        if (CheckString(startpos, endpos, _sentence) == true)
                        {
                            SavePhrase(_ps, startpos, endpos, _prestate, DFAState.S1, _sentence);
                            _prestate = DFAState.S1;
                            startpos = i;
                        }
                    }
                    else if (_prestate == DFAState.S2)
                    {
                        // float state
                        _prestate = DFAState.S2;
                    }
                    else if (_prestate == DFAState.S17 || _prestate == DFAState.S18 || _prestate == DFAState.S16)
                    { 
                        //varialbe/dataprovider state, do nothing
                    }
                    else
                    {
                        //handle the previous char
                        endpos = i - 1;

                        SavePhrase(_ps, startpos, endpos, _prestate, DFAState.S1, _sentence);

                        //non-numeric state to integer state
                        _prestate = DFAState.S1;

                        startpos = i;
                    }
                }
                else if (_chArray[i] == '.')
                {
                    //小数点
                    if (_prestate == DFAState.S1 || _prestate == DFAState.S0)
                        _prestate = DFAState.S2;	//由整数串或初态变为浮点数串
                    else
                    {	//未知态
                        // 需要讨论: 是否保存前一个词
                        _prestate = DFAState.SX;
                    }
                }
                else if (Char.IsWhiteSpace(_chArray[i]))
                {
                    //skip whitespace
                }
                else if (_chArray[i] == '_')
                {
                    if (_prestate != DFAState.S16 && _prestate != DFAState.S17 && _prestate != DFAState.S18)
                    {
                        throw new PhraseException("incorrect syntax near '_'");
                    }
                }
                else if (_chArray[i] == '[')
                {
                    if (_prestate != DFAState.S0)
                    {
                        //handle the previous phrase
                        endpos = i - 1;
                        SavePhrase(_ps, startpos, endpos, _prestate, DFAState.SX, _sentence);
                    }
                    startpos = i + 1;
                    _prestate = DFAState.S16;   //goto variable state
                }
                else if (_chArray[i] == ']')
                {
                    //handle the previous phrase
                    endpos = i - 1;

                    if (_prestate == DFAState.S17)  //in dataprovider state
                    {
                        SavePhrase(_ps, startpos, endpos, _prestate, DFAState.S18, _sentence);
                        _prestate = DFAState.S18;
                    }
                    else if (_prestate == DFAState.S16)
                    {
                        SavePhrase(_ps, startpos, endpos, _prestate, DFAState.S16, _sentence);
                    }
                    else
                    {
                        throw new PhraseException("incorrect syntax near ']'");
                    }
                    startpos = i + 1;
                }
                else if (_chArray[i] == ':')
                {
                    if (_prestate == DFAState.S16)  //in variable state
                    {
                        //handle the previous char
                        endpos = i - 1;
                        SavePhrase(_ps, startpos, endpos, _prestate, DFAState.S17, _sentence);
                        _prestate = DFAState.S17;   //goto dataprovider state
                        startpos = i + 1;
                    }
                    else
                    {
                        throw new PhraseException("incorrect syntax near ':'");
                    }
                }
                else if (_chArray[i] == '+' || _chArray[i] == '-' || _chArray[i] == '*' || _chArray[i] == '/' || _chArray[i] == '^'
                    || _chArray[i] == '%' || _chArray[i] == '(' || _chArray[i] == ')' || _chArray[i] == '!' || _chArray[i] == '#'
                    || _chArray[i] == '@' || _chArray[i] == '=')
                {
                    if (_prestate != DFAState.S0)
                    {
                        //handle the previous phrase
                        endpos = i - 1;
                        SavePhrase(_ps, startpos, endpos, _prestate, DFAState.SX, _sentence);
                    }
                    if (_chArray[i] == '-')
                    {
                        if (_ps.PhraseTypeResult.Count > 0)
                        {
                            PhraseType prept = _ps.PhraseTypeResult[_ps.PhraseTypeResult.Count - 1];
                            if (prept != PhraseType.variable && prept != PhraseType.clr && prept != PhraseType.sto && prept != PhraseType.rightbracket && prept != PhraseType.number)
                            {
                                _chArray[i] = '@';
                            }
                        }
                        else
                        {
                            _chArray[i] = '@';
                        }
                    }
                    if (_chArray[i] == '+')
                        _prestate = DFAState.S4;
                    else if (_chArray[i] == '-')
                        _prestate = DFAState.S5;
                    else if (_chArray[i] == '*')
                        _prestate = DFAState.S6;
                    else if (_chArray[i] == '/')
                        _prestate = DFAState.S7;
                    else if (_chArray[i] == '=')     //not support so far
                        _prestate = DFAState.S11;
                    else if (_chArray[i] == '%')
                        _prestate = DFAState.S8;
                    else if (_chArray[i] == '^')
                        _prestate = DFAState.S10;
                    else if (_chArray[i] == '(')
                        _prestate = DFAState.S12;
                    else if (_chArray[i] == ')')
                        _prestate = DFAState.S13;
                    else if (_chArray[i] == '!')
                        _prestate = DFAState.S9;
                    else if (_chArray[i] == '#')
                        _prestate = DFAState.S14;
                    else if (_chArray[i] == '@')
                        _prestate = DFAState.S15;

                    startpos = i;
                }
                else
                {
                    //_prestate = DFAState.SX;
                    throw new PhraseException(string.Format("unknown character: {0}", _chArray[i]));
                }
				i++;
			}
			//return true;
		}
	}
}
