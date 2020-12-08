using System;
using CalcEngine.Phrase;
using CalcEngine;
using Xunit;

namespace UnitTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>

    public class PhraseAnalyzerTest
    {
        public PhraseAnalyzerTest()
        {
        }
        [Fact]
        public void TestCalculationExpression1()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1+2+3=", phrases);

            
            Assert.Equal(5,phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.plus, phrases[1].Value);
            Assert.Equal(PhraseType.number, phrases[2].Value);
            Assert.Equal(PhraseType.plus, phrases[3].Value);
            Assert.Equal(PhraseType.number, phrases[4].Value);

            Assert.Equal("1", phrases[0].Key);
            Assert.Equal("+", phrases[1].Key);
            Assert.Equal("2", phrases[2].Key);
            Assert.Equal("+", phrases[3].Key);
            Assert.Equal("3", phrases[4].Key);
        }

        [Fact]
        public void TestCalculationExpression2()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1.1+2.20+3=", phrases);
            
            Assert.Equal(5, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.plus, phrases[1].Value);
            Assert.Equal(PhraseType.number, phrases[2].Value);
            Assert.Equal(PhraseType.plus, phrases[3].Value);
            Assert.Equal(PhraseType.number, phrases[4].Value);

            Assert.Equal("1.1", phrases[0].Key);
            Assert.Equal("+", phrases[1].Key);
            Assert.Equal("2.20", phrases[2].Key);
            Assert.Equal("+", phrases[3].Key);
            Assert.Equal("3", phrases[4].Key);
        }
        [Fact]
        public void TestCalculationExpression3()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1.1+2.20++3=", phrases);
            

            Assert.Equal(6, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.plus, phrases[1].Value);
            Assert.Equal(PhraseType.number, phrases[2].Value);
            Assert.Equal(PhraseType.plus, phrases[3].Value);
            Assert.Equal(PhraseType.plus, phrases[4].Value);
            Assert.Equal(PhraseType.number, phrases[5].Value);

            Assert.Equal("1.1", phrases[0].Key);
            Assert.Equal("+", phrases[1].Key);
            Assert.Equal("2.20", phrases[2].Key);
            Assert.Equal("+", phrases[3].Key);
            Assert.Equal("+", phrases[4].Key);
            Assert.Equal("3", phrases[5].Key);

        }
        [Fact]
        public void TestCalculationExpression4()
        {
            PhraseStorage phrases = new PhraseStorage();
            try
            {
                PhraseAnalyzer.Analyze("1.1.1+1=", phrases);
            }
            catch(PhraseException e)
            {
                Assert.True(e.Message.Contains("Unrecognizable symbol"));
            }
        }
        [Fact]
        public void TestCalculationExpression5()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("sincos1=", phrases);
            

            Assert.Equal(3, phrases.Length);
            Assert.Equal(PhraseType.sin, phrases[0].Value);
            Assert.Equal(PhraseType.cos, phrases[1].Value);
            Assert.Equal(PhraseType.number, phrases[2].Value);

            Assert.Equal("sin", phrases[0].Key);
            Assert.Equal("cos", phrases[1].Key);
            Assert.Equal("1", phrases[2].Key);
        }
        [Fact]
        public void TestCalculationExpression6()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("cos(1+2*3-4)/5=", phrases);
            

            Assert.Equal(12, phrases.Length);
            Assert.Equal(PhraseType.cos, phrases[0].Value);
            Assert.Equal(PhraseType.leftbracket, phrases[1].Value);
            Assert.Equal(PhraseType.number, phrases[2].Value);
            Assert.Equal(PhraseType.plus, phrases[3].Value);
            Assert.Equal(PhraseType.number, phrases[4].Value);
            Assert.Equal(PhraseType.mutiple, phrases[5].Value);
            Assert.Equal(PhraseType.number, phrases[6].Value);
            Assert.Equal(PhraseType.minus, phrases[7].Value);
            Assert.Equal(PhraseType.number, phrases[8].Value);
            Assert.Equal(PhraseType.rightbracket, phrases[9].Value);
            Assert.Equal(PhraseType.divide, phrases[10].Value);
            Assert.Equal(PhraseType.number, phrases[11].Value);

            Assert.Equal("cos", phrases[0].Key);
            Assert.Equal("(", phrases[1].Key);
            Assert.Equal("1", phrases[2].Key);
            Assert.Equal("+", phrases[3].Key);
            Assert.Equal("2", phrases[4].Key);
            Assert.Equal("*", phrases[5].Key);
            Assert.Equal("3", phrases[6].Key);
            Assert.Equal("-", phrases[7].Key);
            Assert.Equal("4", phrases[8].Key);
            Assert.Equal(")", phrases[9].Key);
            Assert.Equal("/", phrases[10].Key);
            Assert.Equal("5", phrases[11].Key);
        }
        [Fact]
        public void TestCalculationExpression7()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("30!/50*sin30=", phrases);
            //

            Assert.Equal(7, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.fact, phrases[1].Value);
            Assert.Equal(PhraseType.divide, phrases[2].Value);
            Assert.Equal(PhraseType.number, phrases[3].Value);
            Assert.Equal(PhraseType.mutiple, phrases[4].Value);
            Assert.Equal(PhraseType.sin, phrases[5].Value);
            Assert.Equal(PhraseType.number, phrases[6].Value);
            
            Assert.Equal("30", phrases[0].Key);
            Assert.Equal("!", phrases[1].Key);
            Assert.Equal("/", phrases[2].Key);
            Assert.Equal("50", phrases[3].Key);
            Assert.Equal("*", phrases[4].Key);
            Assert.Equal("sin", phrases[5].Key);
            Assert.Equal("30", phrases[6].Key);
        }
        [Fact]
        public void TestCalculationExpression8()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("50-10*lg70=", phrases);

            Assert.Equal(6, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.minus, phrases[1].Value);
            Assert.Equal(PhraseType.number, phrases[2].Value);
            Assert.Equal(PhraseType.mutiple, phrases[3].Value);
            Assert.Equal(PhraseType.lg, phrases[4].Value);
            Assert.Equal(PhraseType.number, phrases[5].Value);

            Assert.Equal("50", phrases[0].Key);
            Assert.Equal("-", phrases[1].Key);
            Assert.Equal("10", phrases[2].Key);
            Assert.Equal("*", phrases[3].Key);
            Assert.Equal("lg", phrases[4].Key);
            Assert.Equal("70", phrases[5].Key);
        }
        [Fact]
        public void TestCalculationExpression9()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1/tg180*sin60=", phrases);

            Assert.Equal(7, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.divide, phrases[1].Value);
            Assert.Equal(PhraseType.tg, phrases[2].Value);
            Assert.Equal(PhraseType.number, phrases[3].Value);
            Assert.Equal(PhraseType.mutiple, phrases[4].Value);
            Assert.Equal(PhraseType.sin, phrases[5].Value);
            Assert.Equal(PhraseType.number, phrases[6].Value);

            Assert.Equal("1", phrases[0].Key);
            Assert.Equal("/", phrases[1].Key);
            Assert.Equal("tg", phrases[2].Key);
            Assert.Equal("180", phrases[3].Key);
            Assert.Equal("*", phrases[4].Key);
            Assert.Equal("sin", phrases[5].Key);
            Assert.Equal("60", phrases[6].Key);
        }
        [Fact]
        public void TestCalculationExpression10()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1/sbrt5%2+-3=", phrases);

            Assert.Equal(9, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.divide, phrases[1].Value);
            Assert.Equal(PhraseType.sbrt, phrases[2].Value);
            Assert.Equal(PhraseType.number, phrases[3].Value);
            Assert.Equal(PhraseType.mod, phrases[4].Value);
            Assert.Equal(PhraseType.number, phrases[5].Value);
            Assert.Equal(PhraseType.plus, phrases[6].Value);
            Assert.Equal(PhraseType.negative, phrases[7].Value);
            Assert.Equal(PhraseType.number, phrases[8].Value);

            Assert.Equal("1", phrases[0].Key);
            Assert.Equal("/", phrases[1].Key);
            Assert.Equal("sbrt", phrases[2].Key);
            Assert.Equal("5", phrases[3].Key);
            Assert.Equal("%", phrases[4].Key);
            Assert.Equal("2", phrases[5].Key);
            Assert.Equal("+", phrases[6].Key);
            Assert.Equal("3", phrases[8].Key);
        }
        [Fact]
        public void TestCalculationExpressionWithVariable1()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1+[abc]*2=", phrases);

            Assert.Equal(5, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.plus, phrases[1].Value);
            Assert.Equal(PhraseType.variable, phrases[2].Value);
            Assert.Equal(PhraseType.mutiple, phrases[3].Value);
            Assert.Equal(PhraseType.number, phrases[4].Value);

            
            Assert.Equal("1", phrases[0].Key);
            Assert.Equal("+", phrases[1].Key);
            Assert.Equal("abc", phrases[2].Key);
            Assert.Equal("*", phrases[3].Key);
            Assert.Equal("2", phrases[4].Key);
        }
        [Fact]
        public void TestCalculationExpressionWithVariable2()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1+[a_bc]/2=", phrases);

            Assert.Equal(5, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.plus, phrases[1].Value);
            Assert.Equal(PhraseType.variable, phrases[2].Value);
            Assert.Equal(PhraseType.divide, phrases[3].Value);
            Assert.Equal(PhraseType.number, phrases[4].Value);

            
            Assert.Equal("1", phrases[0].Key);
            Assert.Equal("+", phrases[1].Key);
            Assert.Equal("a_bc", phrases[2].Key);
            Assert.Equal("/", phrases[3].Key);
            Assert.Equal("2", phrases[4].Key);
        }
        [Fact]
        public void TestCalculationExpressionWithDataProvider1()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1+[data1:abc]*2=", phrases);

            Assert.Equal(6, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.plus, phrases[1].Value);
            Assert.Equal(PhraseType.dataprovider, phrases[2].Value);
            Assert.Equal(PhraseType.fieldname, phrases[3].Value);
            Assert.Equal(PhraseType.mutiple, phrases[4].Value);
            Assert.Equal(PhraseType.number, phrases[5].Value);

            
            Assert.Equal("1", phrases[0].Key);
            Assert.Equal("+", phrases[1].Key);
            Assert.Equal("data1", phrases[2].Key);
            Assert.Equal("abc", phrases[3].Key);
            Assert.Equal("*", phrases[4].Key);
            Assert.Equal("2", phrases[5].Key);
        }
        [Fact]
        public void TestCalculationExpressionWithDataProvider2()
        {
            PhraseStorage phrases = new PhraseStorage();
            PhraseAnalyzer.Analyze("1+[dat_a1:a_b2c_5]*2=", phrases);

            Assert.Equal(6, phrases.Length);
            Assert.Equal(PhraseType.number, phrases[0].Value);
            Assert.Equal(PhraseType.plus, phrases[1].Value);
            Assert.Equal(PhraseType.dataprovider, phrases[2].Value);
            Assert.Equal(PhraseType.fieldname, phrases[3].Value);
            Assert.Equal(PhraseType.mutiple, phrases[4].Value);
            Assert.Equal(PhraseType.number, phrases[5].Value);

            
            Assert.Equal("1", phrases[0].Key);
            Assert.Equal("+", phrases[1].Key);
            Assert.Equal("dat_a1", phrases[2].Key);
            Assert.Equal("a_b2c_5", phrases[3].Key);
            Assert.Equal("*", phrases[4].Key);
            Assert.Equal("2", phrases[5].Key);
        }
        [Fact]
        public void TestStringToType()
        {
            Assert.Equal(PhraseType.cos,PhraseAnalyzer.StrToType("cos"));
            Assert.Equal(PhraseType.sin, PhraseAnalyzer.StrToType("sin"));
            Assert.Equal(PhraseType.tg, PhraseAnalyzer.StrToType("tg"));
            Assert.Equal(PhraseType.ctg, PhraseAnalyzer.StrToType("ctg"));
            Assert.Equal(PhraseType.atg, PhraseAnalyzer.StrToType("atg"));
            Assert.Equal(PhraseType.actg, PhraseAnalyzer.StrToType("actg"));
            Assert.Equal(PhraseType.acos, PhraseAnalyzer.StrToType("acos"));
            Assert.Equal(PhraseType.asin, PhraseAnalyzer.StrToType("asin"));
            Assert.Equal(PhraseType.ln, PhraseAnalyzer.StrToType("ln"));
            Assert.Equal(PhraseType.lg, PhraseAnalyzer.StrToType("lg"));
            Assert.Equal(PhraseType.log, PhraseAnalyzer.StrToType("log"));
            Assert.Equal(PhraseType.leftbracket, PhraseAnalyzer.StrToType("("));
            Assert.Equal(PhraseType.rightbracket, PhraseAnalyzer.StrToType(")"));
            Assert.Equal(PhraseType.fact, PhraseAnalyzer.StrToType("!"));
            Assert.Equal(PhraseType.pow, PhraseAnalyzer.StrToType("^"));
            Assert.Equal(PhraseType.plus, PhraseAnalyzer.StrToType("+"));
            Assert.Equal(PhraseType.minus, PhraseAnalyzer.StrToType("-"));
            Assert.Equal(PhraseType.mutiple, PhraseAnalyzer.StrToType("*"));
            Assert.Equal(PhraseType.divide, PhraseAnalyzer.StrToType("/"));
            Assert.Equal(PhraseType.cbrt, PhraseAnalyzer.StrToType("cbrt"));
            Assert.Equal(PhraseType.sbrt, PhraseAnalyzer.StrToType("sbrt"));
        }
        [Fact]
        public void TestTypeToString()
        {
            Assert.Equal("cos", PhraseAnalyzer.TypeToStr(PhraseType.cos));
            Assert.Equal("sin", PhraseAnalyzer.TypeToStr(PhraseType.sin));
            Assert.Equal("tg", PhraseAnalyzer.TypeToStr(PhraseType.tg));
            Assert.Equal("ctg", PhraseAnalyzer.TypeToStr(PhraseType.ctg));
            Assert.Equal("atg", PhraseAnalyzer.TypeToStr(PhraseType.atg));
            Assert.Equal("actg", PhraseAnalyzer.TypeToStr(PhraseType.actg));
            Assert.Equal("acos", PhraseAnalyzer.TypeToStr(PhraseType.acos));
            Assert.Equal("asin", PhraseAnalyzer.TypeToStr(PhraseType.asin));
            Assert.Equal("ln", PhraseAnalyzer.TypeToStr(PhraseType.ln));
            Assert.Equal("lg", PhraseAnalyzer.TypeToStr(PhraseType.lg));
            Assert.Equal("log", PhraseAnalyzer.TypeToStr(PhraseType.log));
            Assert.Equal("(", PhraseAnalyzer.TypeToStr(PhraseType.leftbracket));
            Assert.Equal(")", PhraseAnalyzer.TypeToStr(PhraseType.rightbracket));
            Assert.Equal("!", PhraseAnalyzer.TypeToStr(PhraseType.fact));
            Assert.Equal("^", PhraseAnalyzer.TypeToStr(PhraseType.pow));
            Assert.Equal("+", PhraseAnalyzer.TypeToStr(PhraseType.plus));
            Assert.Equal("-", PhraseAnalyzer.TypeToStr(PhraseType.minus));
            Assert.Equal("*", PhraseAnalyzer.TypeToStr(PhraseType.mutiple));
            Assert.Equal("/", PhraseAnalyzer.TypeToStr(PhraseType.divide));
            Assert.Equal("cbrt", PhraseAnalyzer.TypeToStr(PhraseType.cbrt));
            Assert.Equal("sbrt", PhraseAnalyzer.TypeToStr(PhraseType.sbrt));
        }
    }
}

