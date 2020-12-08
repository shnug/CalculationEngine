using System;
 using CalcEngine;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Summary description for CalculationEngineTest
    /// </summary>
    public class CalculationEngineTest
    {
        public CalculationEngineTest()
        {
        }
        [Fact]
        public void CalculationTest1()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("1+2+3#=");
                CalculationEngine ce = new CalculationEngine(context);
                Assert.Equal(0.0, context.ANS);
            ce.Run();
                Assert.Equal(6.0, context.ANS);
            }
        }
        [Fact]
        public void CalculationTest2()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("1+(2*3-5)/2#=");
                CalculationEngine ce = new CalculationEngine(context);
            ce.Run();
                Assert.Equal(1.5, context.ANS);
            }
        }
        [Fact]
        public void CalculationTest3()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("1+(2*3-1)/2=");
                CalculationEngine ce = new CalculationEngine(context);
            ce.Run();
                Assert.Equal(3.5, context.ANS);
            }
        }
        [Fact]
        public void CalculationTest4()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("asin(sin90*5/5.00+cos90)=");
                CalculationEngine ce = new CalculationEngine(context);
            ce.Run();
                Assert.Equal(90, context.ANS);
            }
        }
        [Fact]
        public void CalculationTest5()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("2^(2+4)=");
                CalculationEngine ce = new CalculationEngine(context);
            ce.Run();
                Assert.Equal(Math.Pow(2,6), context.ANS);
            }
        }
        [Fact]
        public void CalculationTest6()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("6!=");
                CalculationEngine ce = new CalculationEngine(context);
            ce.Run();
                Assert.Equal(720, context.ANS);
            }
        }
        [Fact]
        public void CalculationTest7()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("cossin(90)=");
                CalculationEngine ce = new CalculationEngine(context);
            ce.Run();
                Assert.Equal(0.999848, context.ANS);
            }
        }
        [Fact]
        public void CalculationTest8_WrongPhrase()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("loglnlg3=");
                CalculationEngine ce = new CalculationEngine(context);
                try
                {
                    ce.Run();
                }
                catch (SyntaxException e)
                {
                    Assert.Contains("Not enough operands to finish the calculation", e.Message);
                }
            }
        }
        [Fact]
        public void CalculationTest9()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("5+-1--1=");    
                CalculationEngine ce = new CalculationEngine(context);
                ce.Run();
                Assert.Equal(5, context.ANS);
            }
        }
        [Fact]
        public void CalculationTest10()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("5---1=");  //same as 5-1
                CalculationEngine ce = new CalculationEngine(context);
                ce.Run();
                Assert.Equal(4, context.ANS);
            }
        }
        [Fact]
        public void CalculationWithANSTest()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("1+2=");
                CalculationEngine ce = new CalculationEngine(context);
                Assert.Equal(0.0, context.ANS);
            ce.Run();
                Assert.Equal(3, context.ANS);

                context.Analyze("2+ANS=");
                ce = new CalculationEngine(context);
                ce.Run();
                Assert.Equal(5, context.ANS);

                context.Analyze("ANS*2=");
                ce = new CalculationEngine(context);
            ce.Run();
                Assert.Equal(10, context.ANS);
            }
        }

        [Fact]
        public void CalculationWithVariableTest1()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.Analyze("1+(2*3-[ax])/2=");
                CalculationEngine ce = new CalculationEngine(context);
                Assert.Equal(0.0, context.ANS);
            try
            {
                ce.Run();
            }
            catch (CalculationException e)
            {
                Assert.Contains("variable 'ax' not found", e.Message);
            }
            context.SetVariable("ax", 5);
            ce.Run();
                Assert.Equal(1.5, context.ANS);
            }
        }

        [Fact]
        public void CalculationWithVariableTest2()
        {
            using (CalculationContext context = new CalculationContext())
            {
                context.SetVariable("a", 1.56);
                context.SetVariable("b", 1.44);
                context.SetVariable("c", -5);

                context.Analyze("[a]+[b]+[c]=");
                CalculationEngine ce = new CalculationEngine(context);
                Assert.Equal(0.0, context.ANS);
                ce.Run();
                Assert.Equal(-2, context.ANS);
            }
        }
    }
}
