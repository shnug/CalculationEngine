using System;
using System.Collections;
using CalcEngine.Syntax;
using CalcEngine.Phrase;

namespace CalcEngine
{
	/// <summary>
	/// Calculation Unit
	/// </summary>
    public class CalculationEngine : IDisposable, ICalculationEngine
        {
		Stack _optr=null;	//Operator Stack
		Stack _opnd=null;	//Number Stack
        CalculationContext _context = null;

        public CalculationEngine(CalculationContext context)
		{
			_optr=new Stack();
			_opnd=new Stack();
            _context = context;
            if (context.Phrases.PhraseTypeResult[context.Phrases.Length - 1] != PhraseType.sharp)
                context.Phrases.AddPhraseResult("#", PhraseType.sharp);
		}
        public void Dispose()
        {
            if (_optr != null)
                _optr.Clear();
            if (_opnd != null)
                _opnd.Clear();

        }
		/// <summary>
		/// Factorial
		/// </summary>
		/// <param name="i">Floor</param>
		/// <returns></returns>
		private double Factorial(double i) 
        { 
			return((i <= 1) ? 1 : (i * Factorial(i-1))); 
        }
        /// <summary>
		/// Calculate by phrase type
        /// </summary>
		/// <param name="a">Operator 1</param>
		/// <param name="b">Operator 2</param>
		/// <param name="pt">Phrase Type</param>
		/// <returns>Result</returns>
		private double Calculate(PhraseType pt)
		{
			//get the operators
			double a,b;
			a=b=0.0;
			OperandType ot=Operator.OperandCount(pt);
			switch(ot)
			{
				case OperandType.O2:	//双目运算
                    if (_opnd.Count < 2)
                        throw new SyntaxException("Not enough operands to finish the calculation of "+pt.ToString());
					b=(double)_opnd.Pop();
					a=(double)_opnd.Pop();
					break;
				case OperandType.O1:	//单目运算
                    if (_opnd.Count < 1)
                        throw new SyntaxException("Not enough operands to finish the calculation of " + pt.ToString());

					a=(double)_opnd.Pop();
					b=0.0;
					break;
				case OperandType.O0:	//零目运算
					return 0.0;
			}
			//Let's calculate!!!
            double tmp = 0.0;
			switch(pt)
			{
				//四则运算
				case PhraseType.plus:
					return a+b;
				case PhraseType.minus:
					return a-b;
				case PhraseType.mutiple:
					return a*b;
				case PhraseType.divide:
					return a/b;
				case PhraseType.mod:
					return a%b;
                case PhraseType.negative:
                    return 0.0-a;
				case PhraseType.fact:
					return Factorial(a);
				//三角函数
				case PhraseType.sin:
                    if (_context.Mode == CalculationMode.Degree)
                    {
                        a = Utility.DegreesToRadians(a);
                    }
					return Math.Sin(a);
				case PhraseType.cos:
                    if (_context.Mode == CalculationMode.Degree)
                    {
                        a = Utility.DegreesToRadians(a);
                    }
					return Math.Cos(a);
				case PhraseType.tg:
                    if (_context.Mode == CalculationMode.Degree)
                    {
                        a = Utility.DegreesToRadians(a);
                    }
					return Math.Tan(a);
				case PhraseType.ctg:
                    if (_context.Mode == CalculationMode.Degree)
                    {
                        a = Utility.DegreesToRadians(a);
                    }
					return 1.0/Math.Tan(a);
				case PhraseType.acos:
                    tmp=Math.Acos(a);
                    if (_context.Mode == CalculationMode.Degree)
                    {
                        tmp=Utility.RadiansToDegrees(tmp);
                    }
                    return tmp;
				case PhraseType.asin:
                    tmp = Math.Asin(a);
                    if (_context.Mode == CalculationMode.Degree)
                    {
                        tmp = Utility.RadiansToDegrees(tmp);
                    }
                    return tmp;
				case PhraseType.atg:
                    tmp = Math.Atan(a);
                    if (_context.Mode == CalculationMode.Degree)
                    {
                        tmp = Utility.RadiansToDegrees(tmp);
                    }
                    return tmp;
				case PhraseType.actg:
                    tmp = Math.Atan(1.0/a);
                    if (_context.Mode == CalculationMode.Degree)
                    {
                        tmp = Utility.RadiansToDegrees(tmp);
                    }
                    return tmp;
				//乘方
				case PhraseType.pow:
					return Math.Pow(a,b);
				case PhraseType.sbrt:
					return Math.Sqrt(a);
				case PhraseType.cbrt:
					return Math.Pow(a,1.0/3.0);
				//log series
				case PhraseType.ln:
                    return Math.Log(a, _context.VarsList.Get("e"));
				case PhraseType.log:
					return Math.Log(b,a);		//a log b
				case PhraseType.lg:
					return Math.Log10(a);
				default:
					return 0.0;
			}
        }
        /// <summary>
		/// Run the calculation
        /// </summary>
		public void Run()
            {
			//把所有的词压入栈中
			int i=0;
			_optr.Clear();
			_optr.Push(PhraseType.sharp);	//将#作为栈操作结束标志

			_opnd.Clear();

			while(i<_context.Phrases.Length)
        {
				PhraseType temp_pt=_context.Phrases.PhraseTypeResult[i];
				if(temp_pt==PhraseType.number)		
					_opnd.Push(_context.Phrases.GetNumberValue(i));
				else if(temp_pt==PhraseType.e)
                    _opnd.Push(_context.VarsList.EXP);
				else if(temp_pt==PhraseType.pi)
                    _opnd.Push(_context.VarsList.PI);
				else if(temp_pt==PhraseType.ans)
                    _opnd.Push(_context.VarsList.ANS);
				else if(temp_pt==PhraseType.variable)
                    _opnd.Push(_context.VarsList.Get(_context.Phrases.PhraseResult[i] as string));
				else	//is operator
            {
					//calculation stop
					if((PhraseType)_optr.Peek()==PhraseType.sharp&&temp_pt==PhraseType.sharp)
						break;

					PriorityCmpType temp_pct=(PriorityCmpType)Operator.OperatorCmp2((PhraseType)_optr.Peek(),temp_pt);
					if(temp_pct==PriorityCmpType.Higher)
        {
						do
            {
							//calculate the previous operator
							if(Operator.OperandCount((PhraseType)_optr.Peek())!=OperandType.O0)	//不是零目运算符
								_opnd.Push(Calculate((PhraseType)_optr.Pop()));

						}while((PriorityCmpType)Operator.OperatorCmp2((PhraseType)_optr.Peek(),temp_pt)==PriorityCmpType.Higher);
						//当相邻PhraseType优先级相等时
						if((PriorityCmpType)Operator.OperatorCmp2((PhraseType)_optr.Peek(),temp_pt)==PriorityCmpType.Equal)
            {
							_optr.Pop();	//pop same prePhraseType
            }
						else
        {
							_optr.Push(temp_pt);
        }
        }
					else if(temp_pct==PriorityCmpType.Lower)
        {
						_optr.Push(temp_pt);
        }
					else if(temp_pct==PriorityCmpType.Equal)
            {
						_optr.Pop();
            }
        }
				i++;
            }
			//save result to ans
            _context.VarsList.ANS = (double)_opnd.Peek();
        }
    }
}
