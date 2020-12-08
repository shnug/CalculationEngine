using System;
using System.Collections;
using CalcEngine.SyntaxAnalyzer;
using CalcEngine.Phrase;

namespace CalcEngine
{
	/// <summary>
	/// Calculation Unit
	/// </summary>
    public class CalcUnit : IDisposable
	{
		private Stack _optr=null;	//Operator Stack
		private Stack _opnd=null;	//Number Stack
		private PhraseStorage _ps=null;
        private CalculationMode _cm = CalculationMode.Degree;

        public CalcUnit(PhraseStorage ps)
		{
			_optr=new Stack();
			_opnd=new Stack();
			_ps=ps;
            if (_ps.PhraseTypeResult[_ps.Length - 1] != PhraseType.sharp)
                _ps.AddPhraseResult("#", PhraseType.sharp);
		}
        public void Dispose()
        {
            if (_optr != null)
                _optr.Clear();
            if (_opnd != null)
                _opnd.Clear();
            if (_ps != null)
                _ps.Dispose();            
        }
        /// <summary>
        /// Calculation Mode: Degree/Radius
        /// </summary>
        public CalculationMode Mode
        {
            get { return _cm; }
            set { _cm = value; }
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
				case OperandType.O2:	//˫Ŀ����
                    if (_opnd.Count < 2)
                        throw new SyntaxException("Not enough operands to finish the calculation of "+pt.ToString());
					b=(double)_opnd.Pop();
					a=(double)_opnd.Pop();
					break;
				case OperandType.O1:	//��Ŀ����
                    if (_opnd.Count < 1)
                        throw new SyntaxException("Not enough operands to finish the calculation of " + pt.ToString());

					a=(double)_opnd.Pop();
					b=0.0;
					break;
				case OperandType.O0:	//��Ŀ����
					return 0.0;
			}
			//Let's calculate!!!
            double tmp = 0.0;
			switch(pt)
			{
				//��������
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
				//���Ǻ���
				case PhraseType.sin:
                    if (Mode == CalculationMode.Degree)
                    {
                        a = Utility.DegreesToRadians(a);
                    }
					return Math.Sin(a);
				case PhraseType.cos:
                    if (Mode == CalculationMode.Degree)
                    {
                        a = Utility.DegreesToRadians(a);
                    }
					return Math.Cos(a);
				case PhraseType.tg:
                    if (Mode == CalculationMode.Degree)
                    {
                        a = Utility.DegreesToRadians(a);
                    }
					return Math.Tan(a);
				case PhraseType.ctg:
                    if (Mode == CalculationMode.Degree)
                    {
                        a = Utility.DegreesToRadians(a);
                    }
					return 1.0/Math.Tan(a);
				case PhraseType.acos:
                    tmp=Math.Acos(a);
                    if(Mode==CalculationMode.Degree)
                    {
                        tmp=Utility.RadiansToDegrees(tmp);
                    }
                    return tmp;
				case PhraseType.asin:
                    tmp = Math.Asin(a);
                    if (Mode == CalculationMode.Degree)
                    {
                        tmp = Utility.RadiansToDegrees(tmp);
                    }
                    return tmp;
				case PhraseType.atg:
                    tmp = Math.Atan(a);
                    if (Mode == CalculationMode.Degree)
                    {
                        tmp = Utility.RadiansToDegrees(tmp);
                    }
                    return tmp;
				case PhraseType.actg:
                    tmp = Math.Atan(1.0/a);
                    if (Mode == CalculationMode.Degree)
                    {
                        tmp = Utility.RadiansToDegrees(tmp);
                    }
                    return tmp;
				//�˷�
				case PhraseType.pow:
					return Math.Pow(a,b);
				case PhraseType.sbrt:
					return Math.Sqrt(a);
				case PhraseType.cbrt:
					return Math.Pow(a,1.0/3.0);
				//log series
				case PhraseType.ln:
					return Math.Log(a,Variables.Get("e"));
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
			//�����еĴ�ѹ��ջ��
			int i=0;
			_optr.Clear();
			_optr.Push(PhraseType.sharp);	//��#��Ϊջ����������־
			
			_opnd.Clear();

			while(i<_ps.Length)
			{
				PhraseType temp_pt=_ps.PhraseTypeResult[i];
				if(temp_pt==PhraseType.number)		
					_opnd.Push(_ps.GetNumberValue(i));
				else if(temp_pt==PhraseType.e)		
                    _opnd.Push(Variables.EXP);
				else if(temp_pt==PhraseType.pi)		
                    _opnd.Push(Variables.PI);
				else if(temp_pt==PhraseType.ans)	
                    _opnd.Push(Variables.ANS);
				else if(temp_pt==PhraseType.variable)
					_opnd.Push(Variables.Get(_ps.PhraseResult[i] as string));
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
							if(Operator.OperandCount((PhraseType)_optr.Peek())!=OperandType.O0)	//������Ŀ�����
								_opnd.Push(Calculate((PhraseType)_optr.Pop()));

						}while((PriorityCmpType)Operator.OperatorCmp2((PhraseType)_optr.Peek(),temp_pt)==PriorityCmpType.Higher);
						//������PhraseType���ȼ����ʱ
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
			Variables.ANS=(double)_opnd.Peek();
		}
		/// <summary>
		/// Result
		/// </summary>
		public double ANS
		{
			get{
                if (Variables.ANS.CompareTo(double.NaN) == 0		//none-number
                    || Variables.ANS.CompareTo(double.PositiveInfinity) == 0	//positive infinity
                    || Variables.ANS.CompareTo(double.NegativeInfinity) == 0)	//negitive infinity
                    throw new CalculationException("Calculation failed");

                return Variables.ANS;
			}
		}
	}
}
