using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.Resources;

namespace LiteApp.MySpace.Framework.Validation
{
    public abstract class BaseValidationRule : IValidationRule
    {
        public string Validate(ValidationAttribute attr, object value)
        {
            bool valid = DoValidate(attr, value);

            if (valid)
                return string.Empty;

            if (attr.ErrorMessageResourceName != null && attr.ErrorMessageResourceType != null)
            {
                ResourceManager rm = new ResourceManager(attr.ErrorMessageResourceType);
                return rm.GetString(attr.ErrorMessageResourceName);
            }

            return attr.DefaultErrorMessage;
        }

        protected virtual bool DoValidate(ValidationAttribute attr, object value)
        {
            return true;
        }
    }
}
