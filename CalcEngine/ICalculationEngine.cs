using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    public delegate void BeforeCalculationHandler(object sender,CalculationEventArgs e);
    public delegate void CalculationCompleteHandler(object sender, CalculationEventArgs e); 

    public interface ICalculationEngine
    {
        void Run();
        double Calculate();
    }
}
