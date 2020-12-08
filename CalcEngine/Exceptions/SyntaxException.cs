using System;
using System.Collections.Generic;
using System.Text;
using CalcEngine.Phrase;

namespace CalcEngine
{
    public class SyntaxException:Exception
    {
        public SyntaxException():base()
        { 
        }


        public SyntaxException(string msg):base(msg)
        {
        
        }

        public SyntaxException(PhraseType phraseType)
            : this(string.Format("Incorrect syntax near '{0}'",PhraseAnalyzer.TypeToStr(phraseType)))
        { 
            
        }

        public SyntaxException(double phraseValue)
            :this(string.Format("Incorrect syntax near '{0}'",phraseValue))
        { 
            
        }

        public SyntaxException(string msg, Exception innerException)
            : base(msg, innerException)
        { 
        
        }
    }
}
