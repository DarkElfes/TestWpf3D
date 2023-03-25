using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test.Models
{
    static class ValidateValue
    {
        public static void Double(ref double var, double value, double minValue, double maxValue, [CallerMemberName] string? PropertyName = null)
        {
            if (value < minValue || value > maxValue)
                throw new ArgumentNullException(nameof(PropertyName), $"{PropertyName} cannot be less then {minValue} and greater then {maxValue}");

            if (var != value) var = value;
        }
    }
}
