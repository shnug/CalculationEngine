using System;
using System.Collections;
using System.Collections.Generic;

namespace CalcEngine.Phrase
{
	/// <summary>
	/// 词存储单元
	/// </summary>
	public class PhraseStorage:IDisposable
	{
        List<KeyValuePair<string,PhraseType>> _store=null;
		
		public PhraseStorage()
		{
            _store = new List<KeyValuePair<string, PhraseType>>();
		}
		/// <summary>
		/// 词的数量
		/// </summary>
		public int Length
		{
			get{return _store.Count;}
		}
		/// <summary>
		/// 清除存储的结果
		/// </summary>
		public void ClearResult()
		{
            _store.Clear();
		}
		/// <summary>
		/// 添加一个词和它对应的词类
		/// </summary>
		/// <param name="phrase">词</param>
		/// <param name="pt">词类</param>
		public void AddPhraseResult(string phrase,PhraseType pt)
		{
			_store.Add(new KeyValuePair<string,PhraseType>(phrase,pt));			
		}
		/// <summary>
		/// 获得数字的浮点值
		/// </summary>
		/// <param name="index">索引</param>
		/// <returns></returns>
		public double GetNumberValue(int index)
		{
			string temp_str=_store[index].Key;
			if(temp_str[0]=='@')	
				temp_str=temp_str.Replace('@','-');	//把'@'转换为负号

				return Convert.ToDouble(temp_str);
		}
		/// <summary>
		/// 输出分词结果
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
		/// 输出分词类型结果
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
