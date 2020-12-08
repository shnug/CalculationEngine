using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    public class CalculationEventArgs : EventArgs
    {
        CalculationContext _context;
        int _index;
        string _expr;

        public CalculationEventArgs(CalculationContext context,string expr):this(context,-1,expr)
        {
        }

        public CalculationEventArgs(CalculationContext context, int index, string expr)
        {
            this._index = index;
            this._expr = expr;
            this._context = context;
        }

        public string Expression
        {
            get { return this._expr; }
        }
        public int Index
        {
            get { return this._index; }
        }
        public double Result
        {
            get { return _context.ANS; }
        }
    }
}
