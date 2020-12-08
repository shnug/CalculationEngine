using System;
using CalcEngine;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Summary description for VariablesTest
    /// </summary>
    public class VariablesTest
    {
        public VariablesTest()
        {
            variables = new Variables();
        }

        Variables variables;

        [Fact]
        public void TestGetPI()
        {
            Assert.Equal(variables.PI, variables.Get("pi"));
            Assert.Equal(variables.PI, variables.Get("Pi"));
            Assert.Equal(variables.PI, Math.PI );
            Assert.Equal(Math.PI, variables.Get("Pi"));
        }

        [Fact]
        public void TestGetExp()
        {
            Assert.Equal(variables.EXP, variables.Get("e"));
            Assert.Equal(variables.EXP, variables.Get("E"));
            Assert.Equal(variables.EXP,Math.Exp(1));
            Assert.Equal(Math.Exp(1), variables.Get("e"));
        }
        [Fact]
        public void TestSetExpOrPI()
        {

            try
            {
                variables.Set("PI", 1);
                throw new Exception("Argument exception should be thrown if PI is set");
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                throw new Exception("Argument exception should be thrown if PI is set");
            }
            try
            {
                variables.Set("E", 1);
                throw new Exception("Argument exception should be thrown if E is set");
            }
            catch (ArgumentException)
            {

            }
            catch
            {
                throw new Exception("Argument exception should be thrown if PI is set");
            }
        }

        [Fact]
        public void TestSetValues()
        {
            variables.Set("ax", 1.0);
            Assert.Equal(1.0, variables.Get("ax"));
            variables.Set("bx", 1);
            Assert.Equal(1, variables.Get("ax"));
            variables.Set("ax", 5);
            Assert.Equal(5, variables.Get("ax"));
        }
    }
}
