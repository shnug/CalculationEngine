using System;

namespace CalcEngine.Phrase
{
	/// <summary>
	/// �������������Զ���״̬
	/// </summary>
	public enum DFAState:int
	{
		S0=0,	///��̬
		S1=1,	///������������С����
		S2=2,	///��������
		S3=3,	///��ĸ��
		S4=4,	/// +
		S5=5,	/// -
		S6=6,	/// *
		S7=7,	/// /
		S8=8,	/// %
		S9=9,	/// !
		S10=10,	/// ^
		S11=11,	/// =
		S12=12,	/// (
		S13=13,	/// )
		S14=14, /// #
        S15=15, /// @
        S16 = 16,  /// variable [...]
        S17 = 17,  /// dataprovider [<dataprovider>:field]
        S18 = 18,  /// fieldname [dataprovider:<field>]
		SX=50,	/// δ֪̬
	}
}
