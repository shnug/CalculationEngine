using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    class Utility
    {
        const double PiDividedBy180 = Math.PI / 180;
        /// <summary>
        /// ���Ƕ�ת��Ϊ����
        /// </summary>
        /// <param name="degrees">�Ƕ�ֵ</param>
        /// <returns>�Ƕȶ�Ӧ�Ļ���</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * PiDividedBy180;
        }

        /// <summary>
        /// ������ת��Ϊ�Ƕ�
        /// </summary>
        /// <param name="radians">����ֵ</param>
        /// <returns>���ȶ�Ӧ�ĽǶ�</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians / PiDividedBy180;
        }
    }
}
