namespace CalcEngine
{

    public interface IBatchCalculation
    {
        void Run();
        void OnBeforeCalculation(CalculationEventArgs e);
        void OnCalculationComplete(CalculationEventArgs e);
    }
}