using System;
using System.Collections.Generic;

namespace CalcEngine
{
	/// <summary>
	/// Constant and Variable list
	/// </summary>
	public class Variables
	{
        Dictionary<string, double> _variable = new Dictionary<string, double>();

        public Variables()
        {
        }
        public double PI
        {
            get { return Math.PI; }
        }
        public double EXP
        {
            get { return Math.Exp(1); }
        }
        public double ANS
        {
            get {
                if (_variable.ContainsKey("ans"))
                    return _variable["ans"];
                else
                    return 0.0;
            }
            set { 
                if(_variable.ContainsKey("ans"))
                    _variable["ans"] = value; 
                else
                    _variable.Add("ans",value); 
            }
        }
		/// <summary>
		/// Save variable to the variable list
		/// </summary>
		/// <param name="x">variable name</param>
		/// <param name="Value">variable value</param>
		public void Set(string x,double value)
		{
            x=x.ToLower();
            if ( x== "pi" || x == "e")
                throw new ArgumentException("Variable PI or E cannot be set");

            if (!_variable.ContainsKey(x))
                _variable.Add(x, value);
            else
                _variable[x] = value;
		}
		/// <summary>
		/// Get variable value from the variable list
		/// </summary>
		/// <param name="x">variable name</param>
        public double Get(string x)
		{
            x = x.ToLower();
            if (x == "pi")
            {
                return Math.PI;
            }
            else if (x == "e")
            {
                return Math.Exp(1);
            }
            else if (_variable.ContainsKey(x))
                return _variable[x];

            throw new CalculationException(string.Format("variable '{0}' not found in the variable list", x));
        }
		/// <summary>
		/// clear all the variables except pi and e
		/// </summary>
		public void ClearAll()
		{
            _variable.Clear();
		}
	}
}
