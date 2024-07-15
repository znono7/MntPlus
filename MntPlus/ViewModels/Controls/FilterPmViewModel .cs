using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class FilterPmViewModel : BaseViewModel
    {
        private string? _field = "Empty";
        private string? _operator = "Est";
        private string? _valueType;
        private object? _value;

        public string? Field 
        {
            get => _field;
            set 
            { 
                if (_field != value)
                {
                    _field = value;
                    OnPropertyChanged(nameof(Field));
                    OnFieldChanged();
                }
            }
        }

        public string? Operator
        {
            get => _operator;
            set { _operator = value; OnPropertyChanged(nameof(Operator)); }
        }

        public string? ValueType
        {
            get => _valueType;
            set { _valueType = value; OnPropertyChanged(nameof(ValueType)); }
        }

        public object? Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(nameof(Value)); }
        }

        private void OnFieldChanged()
        {
            if (Field == "Prochaine échéance")
            {
                ValueType = "Date range";
                Value = new DateRangeModel { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7) };
            }else if (Field == "Empty")
            {
                ValueType = "Empty";
                Value = string.Empty;
            }
            
        
        }

       
    }
}
