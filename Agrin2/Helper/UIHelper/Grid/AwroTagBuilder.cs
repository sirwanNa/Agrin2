using System.Collections.Generic;
using System.Linq;

namespace Agrin2.Helper.UIHelper.Grid
{
    public class Agrin2TagBuilder
    {       
        private string _tagName;
        public string InnerHtml { get; set; }
        private Dictionary<string, string> _htmlAttributes;
        public Agrin2TagBuilder(string tagName)
        {
            _tagName = tagName;
            _htmlAttributes = new Dictionary<string, string>();
        }
        public void MergeAttribute(string key, string value)
        {
            _htmlAttributes.Add(key, value);
        }
        public override string ToString()
        {
            var result = "<"+_tagName;
            if (_htmlAttributes != null&& _htmlAttributes.Count()>0)
            {
                foreach(var item in _htmlAttributes)
                {
                    result +=" "+ item.Key + "='" + item.Value + "'"+" ";
                }
            }
            result += ">";
            result +="\n"+ InnerHtml;
            result += "\n" + "</" + _tagName+">";
            return result;
        }
    }
}
