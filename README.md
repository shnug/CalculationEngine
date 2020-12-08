It used to be called Calculator.NET. As the name indicates, this is an calculator project. The difference from Windows Caculator is that it supports expression calculation, which can greatly improve user experience.

Now, I rename it as CalcEngine to provide great features like expression calculation, variable list and data provider. This library will be useful in financial system, which needs calculation very frequently.
 

## Getting Started 

To calculate the expression, asin(sin90*5/5.00+cos90)

 
```
using CalcEngine;

using (CalculationContext context = new CalculationContext())
{
    context.Analyze("asin(sin90*5/5.00+cos90)=");
    CalculationEngine ce = new CalculationEngine(context);
    ce.Run();
    Console.WriteLine(context.ANS);
}
```
NOTE: Don’t forget the ‘=' behind the expression.

 

## What kind of operators are supported

+,-,*,/, sin, cos, tg, ctg, asin, acos, atg, actg, lg, ln, log, ^ (means power), ! (means Factorial), (, ), mod, cbrt (means the cube root of x), sbrt (means the quare root of x)

For example,

6!=720

sbrt9=3

cbrt27=3

2^3=8

2log4=2

 

## Define Variable

To define a variable called ax, please use context.SetVariable

To reference a variable value in the expression, please use [variable name]
```
using (CalculationContext context = new CalculationContext())
{
    context.Analyze("1+(2*3-[ax])/2=");
    CalculationEngine ce = new CalculationEngine(context);
    context.SetVariable("ax", 5);
    ce.Run();
}
```
