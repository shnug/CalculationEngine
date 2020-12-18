using System;
using System.Collections.Generic;
using System.Text;
using CalcEngine.Phrase;

namespace CalcEngine
{
    public class BatchCalculationEngine:IBatchCalculation
    {
        List<double> _batchResults=null;
        CalculationContext _global;

        public BatchCalculationEngine(CalculationContext globalcontext)
        { 
            this._global = globalcontext;
            _batchResults = new List<double>();
        }
        public event BeforeCalculationHandler BeforeCalculation;
        public event CalculationCompleteHandler CalculationComplete;
        
        /// <summary>
        /// calculated results in batch mode
        /// </summary>
        public List<double> Results
        {
            get 
            {
                return _batchResults;
            }
        }
        List<string> _expressions=new List<string>(2);

        public List<string> Expressions
        {
            get {
                return _expressions;
            }
        }
        public CalculationContext GlobalContext
        {
            get { return this._global; }
        }
        public virtual void OnBeforeCalculation(CalculationEventArgs e)
        {
            if (BeforeCalculation != null)
                BeforeCalculation(this, e);
        }

        public virtual void OnCalculationComplete(CalculationEventArgs e)
        {
            if (CalculationComplete != null)
                CalculationComplete(this, e);
        }

        public void Run()
        {
            _batchResults.Clear();
            for(int i=0;i<_expressions.Count;i++)
            {
                string expr = this._expressions[i];
                CalculationContext context = new CalculationContext(expr);
                CalculationEngine ce = new CalculationEngine(context);

                CalculationEventArgs args = new CalculationEventArgs(context,i,expr);
                OnBeforeCalculation(args);
                ce.Run();
                OnCalculationComplete(args);
                _batchResults.Add(context.ANS);
            }
        }
    }
}
