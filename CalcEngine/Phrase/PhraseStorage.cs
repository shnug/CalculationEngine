using System;
using System.Collections;
using System.Collections.Generic;

namespace CalcEngine.Phrase
{
	/// <summary>
	/// �ʴ洢��Ԫ
	/// </summary>
	public class PhraseStorage:IDisposable
	{
        List<KeyValuePair<string,PhraseType>> _store=null;
		
		public PhraseStorage()
		{
            _store = new List<KeyValuePair<string, PhraseType>>();
		}
		/// <summary>
		/// �ʵ�����
		/// </summary>
		public int Length
		{
			get{return _store.Count;}
		}
		/// <summary>
		/// ����洢�Ľ��
		/// </summary>
		public void ClearResult()
		{
            _store.Clear();
		}
		/// <summary>
		/// ���һ���ʺ�����Ӧ�Ĵ���
		/// </summary>
		/// <param name="phrase">��</param>
		/// <param name="pt">����</param>
		public void AddPhraseResult(string phrase,PhraseType pt)
		{
			_store.Add(new KeyValuePair<string,PhraseType>(phrase,pt));			
		}
		/// <summary>
		/// ������ֵĸ���ֵ
		/// </summary>
		/// <param name="index">����</param>
		/// <returns></returns>
		public double GetNumberValue(int index)
		{
			string temp_str=_store[index].Key;
			if(temp_str[0]=='@')	
				temp_str=temp_str.Replace('@','-');	//��'@'ת��Ϊ����

				return Convert.ToDouble(temp_str);
		}
		/// <summary>
		/// ����ִʽ��
		/// </summary>
		public List<string> PhraseResult
		{
			get{
                List<string> tmp = new List<string>();
				 List<KeyValuePair<string,PhraseType>>.Enumerator e=_store.GetEnumerator();
                 while (e.MoveNext())
                 {
                     tmp.Add(e.Current.Key);
                 }
                 return tmp;
			}
		}
		/// <summary>
		/// ����ִ����ͽ��
		/// </summary>
        public List<PhraseType> PhraseTypeResult
		{
			get
			{
                List<PhraseType> tmp = new List<PhraseType>();
                List<KeyValuePair<string, PhraseType>>.Enumerator e = _store.GetEnumerator();
                while (e.MoveNext())
                {
                    tmp.Add(e.Current.Value);
                }
                return tmp;
			}
		}

        public KeyValuePair<string, PhraseType> this[int index]
        {
            get { return _store[index]; }
            set { _store[index] = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            _store.Clear();
        }

        #endregion
    }
}
