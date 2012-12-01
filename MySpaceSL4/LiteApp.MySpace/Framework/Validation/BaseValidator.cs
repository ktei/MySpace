using System;
using System.Collections.Generic;
using System.Reflection;

namespace LiteApp.MySpace.Framework.Validation
{
    public class BaseValidator : IValidator
    {
        readonly Dictionary<string, string> Errors;
        object _target;
        Dictionary<string, PropertyInfo> _propertiesMap;

        public object Target
        {
            get { return _target; }
            set
            {
                if (_target != value)
                {
                    _target = value;
                    _propertiesMap = null; // Reset this to null to enforce reload next time
                }
            }
        }

        public BaseValidator()
        {
            Errors = new Dictionary<string, string>();
        }

        public void AddError(string propertyName, string message)
        {
            if (!Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = message;
            }
            else
            {
                throw new ArgumentException("propertyName already exists in errors dictionary.");
            }
        }

        public void RemoveError(string propertyName)
        {
            Errors.Remove(propertyName);
        }

        public string GetPropertyErrorMessage(string propertyName)
        {
            string message = string.Empty;
            Errors.TryGetValue(propertyName, out message);
            return message;

        }

        public bool HasErrors
        {
            get { return Errors.Count != 0; }
        }

        public string ValidateProperty(string propertyName)
        {
            if (_propertiesMap == null)
                LoadValidationRequiredProperties();
            PropertyInfo prop = null;
            if (_propertiesMap.TryGetValue(propertyName, out prop))
            {
                return ValidateProperty(prop);
            }
            return string.Empty;
        }

        protected virtual string ValidateProperty(PropertyInfo property)
        {
            foreach (var attr in property.GetCustomAttributes(true))
            {
                var validationAttr = attr as ValidationAttribute;
                if (validationAttr == null)
                    continue;

                var rule = ValidationRuleFactory.GetRule(validationAttr);
                if (rule == null)
                    throw new Exception(string.Format("{0} does not have related validation rule.", validationAttr));

                var gettter = property.GetGetMethod();
                if (gettter == null)
                    continue;

                var validationResult = rule.Validate(validationAttr, gettter.Invoke(Target, new object[] { }));
                if (string.IsNullOrEmpty(validationResult))
                    continue;
                
                return validationResult;
            }
            return string.Empty;
        }

        void LoadValidationRequiredProperties()
        {
            _propertiesMap = new Dictionary<string, PropertyInfo>();
            if (Target != null)
            {
                foreach (var propInfo in Target.GetType().GetProperties())
                {
                    if (IsValidationRequrired(propInfo))
                    {
                        _propertiesMap[propInfo.Name] = propInfo;
                    }
                }
            }
        }

        private static bool IsValidationRequrired(PropertyInfo propInfo)
        {
            foreach (var attr in propInfo.GetCustomAttributes(true))
            {
                if (attr is ValidationAttribute)
                    return true;
            }
            return false;
        }
    }
}
