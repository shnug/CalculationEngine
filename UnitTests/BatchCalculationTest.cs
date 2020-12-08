using System;
using CalcEngine;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Summary description for CalculationEngineTest
    /// </summary>
    public class BatchCalculationTest
    {
        public BatchCalculationTest()
        {
        }
        
        [Fact]
        public void BatchCalculationTest1()
        {
            using (CalculationContext context = new CalculationContext())
            {
                BatchCalculationEngine calc = new BatchCalculationEngine(context);
                Assert.Empty(calc.Results);

                calc.Expressions.Add("1+1=");
                calc.Expressions.Add("1+3=");
                calc.Expressions.Add("sbrt(1.2*5*6)/5=");
                calc.Run();
                Assert.Equal(3, calc.Results.Count);
                Assert.Equal(2, calc.Results[0]);
                Assert.Equal(4, calc.Results[1]);
                Assert.Equal(1.2, calc.Results[2]);
            }
        }
        [Fact]
        public void BatchCalculationTestWithEvent1()
        {
            using (CalculationContext context = new CalculationContext())
            {
                BatchCalculationEngine calc = new BatchCalculationEngine(context);
                Assert.Empty(calc.Results);

                int counter1 = 0;
                calc.BeforeCalculation += (sender,e) =>
                    {
                        Assert.True(sender is BatchCalculationEngine);

                        Assert.True(e.Index==counter1);
                        counter1++;
                    };
                int counter2 = 0;
                calc.CalculationComplete += (sender, e) =>
                    {
                        Assert.True(counter1 > counter2);
                        Assert.True(e.Index == counter2);
                        counter2++;
                    };
                calc.Expressions.Add("1+1=");
                calc.Expressions.Add("1+3=");
                calc.Expressions.Add("sbrt(1.2*5*6)/5=");
                calc.Run();
                Assert.Equal(3, counter1);
                Assert.Equal(3, counter2);
            }
        }
        [Fact]
        public void BatchCalculationTestWithEvent2()
        {
            using (CalculationContext context = new CalculationContext())
            {
                BatchCalculationEngine calc = new BatchCalculationEngine(context);
                Assert.Empty(calc.Results);

                calc.BeforeCalculation += (sender, e) =>
                {
                    if (e.Index == 0)
                    {
                        Assert.Equal("1+1=", e.Expression);
                    }
                    else if (e.Index == 1)
                    {
                        Assert.Equal("1+3=", e.Expression);
                    }
                    else if (e.Index == 2)
                    {
                         Assert.Equal("sbrt(1.2*5*6)/5=", e.Expression);
                    }
                };
                calc.CalculationComplete += (sender, e) =>
                {
                    if (e.Index == 0)
                    {
                        Assert.Equal(2, e.Result);
                    }
                    else if (e.Index == 1)
                    {
                        Assert.Equal(4, e.Result);
                    }
                    else if (e.Index == 2)
                    {
                        Assert.Equal(1.2, e.Result);
                    }
                };
                calc.Expressions.Add("1+1=");
                calc.Expressions.Add("1+3=");
                calc.Expressions.Add("sbrt(1.2*5*6)/5=");
                calc.Run();

            }
        }
    }
}
