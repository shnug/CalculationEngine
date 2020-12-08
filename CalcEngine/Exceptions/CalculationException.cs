using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    public class CalculationException:Exception
    {
        public CalculationException():base()
        { 
        }

        public CalculationException(string msg)
            : base(msg)
        { 
        
        }

        public CalculationException(string msg, Exception innerException)
            : base(msg, innerException)
        { 
        
        }
    }
}
