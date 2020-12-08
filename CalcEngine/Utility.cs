using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    class Utility
    {
        const double PiDividedBy180 = Math.PI / 180;
        /// <summary>
        /// 将角度转换为弧度
        /// </summary>
        /// <param name="degrees">角度值</param>
        /// <returns>角度对应的弧度</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * PiDividedBy180;
        }

        /// <summary>
        /// 将弧度转换为角度
        /// </summary>
        /// <param name="radians">弧度值</param>
        /// <returns>弧度对应的角度</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians / PiDividedBy180;
        }
    }
}
