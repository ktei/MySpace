using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteApp.Portable.Mvvm.Validation
{
    public interface IRuleLocator
    {
        IValidationRule FindRule(ValidationAttribute attr);
    }
}
