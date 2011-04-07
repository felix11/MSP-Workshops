using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecognitionLib
{
    public class StringInputData
    {
        private String key;

        public String Key
        {
            get { return key; }
            set { key = value; }
        }
        private String value;

        public String Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public StringInputData(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
