using System;
using System.Collections;
using CalcEngine.Phrase;

namespace CalcEngine.Syntax
{
	/// <summary>
	/// Syntax Analyze
	/// </summary>
	public class SemanticAnalyzer
	{
		private Stack _optr;		//运算符栈
		private Stack _opnd;		//数符栈
		private Stack _op;			//符号栈（包括运算符和数符的栈）
		private PhraseStorage _ps=null;
		private PhraseType _lastOpForError;	//可能引起错误的那个运算符

		public SemanticAnalyzer(PhraseStorage ps)
		{
			_optr=new Stack();
			_opnd=new Stack();
			_op=new Stack();
			_ps=ps;
		}
        public PhraseType ErrorOperator
        {
            get { return _lastOpForError; }
        }
		public string ErrorTip
		{
			get{
				//错误信息处理（UI错误提示优化）
				if(_lastOpForError==PhraseType.unknown)
					return "计算表达式出现未知错误";
				else if(_lastOpForError==PhraseType.sharp)
					return "计算表达式出错";
				else if(_lastOpForError==PhraseType.plus)
					return "在\'+\'附近存在错误";
				else if(_lastOpForError==PhraseType.minus)
					return "在\'-\'附近存在错误";
				else if(_lastOpForError==PhraseType.mutiple)
					return "在\'*\'附近存在错误";
				else if(_lastOpForError==PhraseType.divide)
					return "在\'/\'附近存在错误";
				else if(_lastOpForError==PhraseType.rightbracket)
					return "在\')\'附近存在错误";
				else if(_lastOpForError==PhraseType.leftbracket)
					return "在\'(\'附近存在错误";
				else if(_lastOpForError==PhraseType.pow)
					return "在\'^\'附近存在错误";
				else if(_lastOpForError==PhraseType.fact)
					return "在\'!\'附近存在错误";
				else if(_lastOpForError==PhraseType.mod)
					return "在\'%\'附近存在错误";
                else if(_lastOpForError==PhraseType.negative)
                    return "在\'@\'附近存在错误";
				else if(_lastOpForError==PhraseType.number)
					return "在某个数字附近存在错误";
				else
					return "在\'"+_lastOpForError.ToString()+"\'附近可能存在错误";
			}
		}
		/// <summary>
		/// 虚拟运算（不进行真实的计算）
		/// </summary>
		/// <returns>是否有错误发生</returns>
		private bool FakeCalculate()
		{
			PhraseType pt=(PhraseType)_optr.Pop();
			OperandType oc=Operator.OperandCount(pt);	//栈顶运算符目数

			PhraseType temp_pt;	//存储_op中pop出的一个符号

			switch(oc)
			{
				case OperandType.O0:	//0目运算符，不存在
					_lastOpForError=pt;
					return false;
					//_op.Pop();
					//break;
				case OperandType.O1:	//1目运算符
					if(_opnd.Count>=1)
					{
						_opnd.Pop();
						_op.Pop();	//抛出数符
					}
					else
					{	//没有足够的数符用于匹配运算符，出错
						_lastOpForError=pt;
						return false;
					}
					temp_pt=(PhraseType)_op.Pop();
					//抛出运算符，邻近符号检查
					if(Operator.OperatorCmp((PhraseType)_op.Peek(),temp_pt)==PriorityCmpType.Unknown)
					{
						_lastOpForError=pt;
						return false;
					}
					_opnd.Push(PhraseType.number);
					_op.Push(PhraseType.number);
					break;
				case OperandType.O2:	//2目运算符
					if(_opnd.Count>=2)
					{
						_opnd.Pop();
						_opnd.Pop();
						_op.Pop();	//抛出数符
					}
					else
					{
						_lastOpForError=pt;
						return false;
					}
					temp_pt=(PhraseType)_op.Pop();
					//抛出数符，邻近符号检查
					if(Operator.OperatorCmp((PhraseType)_op.Peek(),temp_pt)==PriorityCmpType.Unknown)
					{
						_lastOpForError=pt;
						return false;
					}
					temp_pt=(PhraseType)_op.Pop();
					//抛出运算符，邻近符号检查
					if(Operator.OperatorCmp((PhraseType)_op.Peek(),temp_pt)==PriorityCmpType.Unknown)
					{
						_lastOpForError=pt;
						return false;
					}

					_opnd.Push(PhraseType.number);
					_op.Push(PhraseType.number);
					break;
			}
			return true;
		}
		/// <summary>
		/// 检查文法
		/// </summary>
		/// <returns>是否正确</returns>
		public bool Check()
		{
			_optr.Clear();
			_optr.Push(PhraseType.sharp);	//将#作为栈操作结束标志
			_opnd.Clear();
			_op.Clear();
			_op.Push(PhraseType.sharp);		//将#作为栈操作结束标志
			
			int i=0;
			while(i<_ps.Length)
			{
				PhraseType temp_pt=_ps.PhraseTypeResult[i];
				//运算前算符相邻检查
				PriorityCmpType temp_pct=(PriorityCmpType)Operator.OperatorCmp((PhraseType)_op.Peek(),temp_pt);
				if(temp_pct==PriorityCmpType.Unknown)
				{
					_lastOpForError=temp_pt;
					return false;
				}
				//假运算处理
				if(temp_pt==PhraseType.number||temp_pt==PhraseType.e||temp_pt==PhraseType.pi||temp_pt==PhraseType.ans
					||temp_pt==PhraseType.variable)
				{	//是数
					_opnd.Push(PhraseType.number);
					_op.Push(PhraseType.number);
				}
				else	//是运算符
				{
					//运算结束
					if((PhraseType)_optr.Peek()==PhraseType.sharp&&temp_pt==PhraseType.sharp)
						break;

					temp_pct=(PriorityCmpType)Operator.OperatorCmp2((PhraseType)_optr.Peek(),temp_pt);
					if(temp_pct==PriorityCmpType.Higher)
					{
						do
						{
							if(this.FakeCalculate()==false)	//虚拟运算
								return false;
						}while((PriorityCmpType)Operator.OperatorCmp2((PhraseType)_optr.Peek(),temp_pt)==PriorityCmpType.Higher);
						//当相邻PhraseType优先级相等时
						if((PriorityCmpType)Operator.OperatorCmp2((PhraseType)_optr.Peek(),temp_pt)==PriorityCmpType.Equal)
						{
							_optr.Pop();	//抛出相等的prePhraseType
							//对类似于(number)的情况做处理
							PhraseType pt1=(PhraseType)_op.Pop();
							_op.Pop();
							_op.Push(pt1);
						}
						else
						{
							_optr.Push(temp_pt);
							_op.Push(temp_pt);
						}
					}
					else if(temp_pct==PriorityCmpType.Lower)
					{
						_optr.Push(temp_pt);
						_op.Push(temp_pt);
					}
					else if(temp_pct==PriorityCmpType.Equal)
					{
						_optr.Pop();
						PhraseType pt1=(PhraseType)_op.Pop();
						_op.Pop();
						_op.Push(pt1);
					}
					else
					{		//出现了不允许相邻的符号
						_lastOpForError=(PhraseType)_optr.Peek();
						return false;
					}
				}
				i++;
			}
			//数栈检查，如果并非只剩一个元素报错
			if(_opnd.Count!=1)
			{
				_lastOpForError=PhraseType.unknown;
				return false;
			}
			return true;
		}
	}
}
