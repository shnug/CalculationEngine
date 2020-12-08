using System;
using System.Collections.Generic;
using System.Text;
using CalcEngine.Phrase;

namespace CalcEngine
{
    public class CalculationContext:IDisposable
    {
        Variables _variables=null;
        PhraseStorage _ps = null;
        string _expr = null;

        public CalculationContext()
        {
            _ps = new PhraseStorage();
            _variables = new Variables();
        }

        public CalculationContext(string expression)
        {
            _expr = expression;
            _ps = new PhraseStorage();
            _variables = new Variables();
            Analyze(expression);
        }
        /// <summary>
        /// variable list
        /// </summary>
        public Variables VarsList
        {
            get { 
                return _variables;
            }
        }
        public string Expression
        {
            get { return _expr; }
        }
        public void SetVariable(string name, double value)
        {
            _variables.Set(name, value);
        }
        public double GetVariable(string name)
        {
            return _variables.Get(name);
        }
        CalculationMode _cm = CalculationMode.Degree;
        /// <summary>
        /// Calculation Mode: Degree/Radius
        /// </summary>
        public CalculationMode Mode
        {
            get { return _cm; }
            set { _cm = value; }
        }
        /// <summary>
        /// Calculation phrases parsed by Phrase Analyzer
        /// </summary>
        public PhraseStorage Phrases
        {
            get { return _ps; }
            set { _ps = value; }
        }
        /// <summary>
        /// Result
        /// </summary>
        public double ANS
        {
            get
            {
                if (VarsList.ANS.CompareTo(double.NaN) == 0		//none-number
                    || VarsList.ANS.CompareTo(double.PositiveInfinity) == 0	//positive infinity
                    || VarsList.ANS.CompareTo(double.NegativeInfinity) == 0)	//negitive infinity
                    throw new CalculationException("Calculation failed");

                return VarsList.ANS;
            }
        }
        public void Analyze(string expression)
        {
            _ps.ClearResult();
            PhraseAnalyzer.Analyze(expression,_ps);
        }
        public void ClearANS()
        {
            VarsList.ANS=0;
        }
        #region IDisposable Members

        public void Dispose()
        {
            if (_ps != null)
                _ps.Dispose();
        }

        #endregion
    }
}
