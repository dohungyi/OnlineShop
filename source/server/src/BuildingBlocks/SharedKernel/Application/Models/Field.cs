using SharedKernel.Libraries.Utility;

namespace SharedKernel.Application;

public class Field
    {
        private string _fieldName { get; set; }
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        private string _value { get; set; }
        public string Value
        {
            get
            {
                if (_value is null)
                {
                    return string.Empty;
                }
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public WhereType Condition { get; set; } = WhereType.E;
        
    }