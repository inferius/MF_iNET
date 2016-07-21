using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INetCore.Core.Language.JavaScript
{
    public class Object
    {
        private string _name;
        private List<Object> _object = new List<Object>();
        private ValueType _type = ValueType.Object;
        private string _value;

        #region Property
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
        
        public string Value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        public ValueType TypeOfValue
        {
            get { return this._type; }
            set { this._type = value; }
        }

        public List<Object> Objects
        {
            get { return this._object; }
            set { this._object = value; }
        }
        #endregion
    }

    public enum ValueType
    {
        Object,
        Float,
        Integer,
        String
    }
}
