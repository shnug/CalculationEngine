using System;
using System.Collections.Generic;
using System.Text;

namespace CalcEngine
{
    public class PhraseException:Exception
    {
        public PhraseException():base()
        { 
        }

        public PhraseException(string msg)
            : base(msg)
        { 
        
        }

        public PhraseException(string msg, Exception innerException)
            : base(msg, innerException)
        { 
        
        }
    }
}
