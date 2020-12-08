using System;

namespace CalcEngine.Syntax
{
    /// <summary>
    /// 优先级比较类型
    /// </summary>
    public enum PriorityCmpType
    {
        Unknown = 0,	//无法比较
        Higher = 1,	//高于
        Lower = 2,	//低于
        Equal = 3		//等于
    }
}