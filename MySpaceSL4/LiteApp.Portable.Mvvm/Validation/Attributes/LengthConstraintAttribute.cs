﻿using System;

namespace LiteApp.Portable.Mvvm.Validation
{
    [RuleMapping(typeof(LengthConstraintRule))]
    public class LengthConstraintAttribute : ValidationAttribute
    {
        int _minimumLength = 1;
        int _maximumLength = 1;

        public LengthConstraintAttribute(int maximumLength)
        {
            MaximumLength = maximumLength;
        }

        public LengthConstraintAttribute(int minimumLength, int maximumLength)
        {
            MaximumLength = maximumLength;
            MinimumLength = minimumLength;
        }

        public override string DefaultErrorMessage
        {
            get
            {
                if (MinimumLength > 1)
                {
                    return string.Format("Must contain {0} to {1} characters.", MinimumLength, MaximumLength);
                }
                return string.Format("Must contain at most {0} characters.", MaximumLength);
            }
        }

        public int MinimumLength
        {
            get { return _minimumLength; }
            set
            {
                if (_minimumLength < 1)
                {
                    throw new Exception("MinimumLength must be at least 1.");
                }
                if (_minimumLength > _maximumLength)
                {
                    throw new Exception("MaximumLength must be greater than or equal to MinimumLength.");
                }
                _minimumLength = value;
            }
        }

        public int MaximumLength
        {
            get { return _maximumLength; }
            set
            {
                if (_maximumLength < _minimumLength)
                {
                    throw new Exception("MaximumLength must be greater than or equal to MinimumLength.");
                }
                _maximumLength = value;
            }
        }
    }
}
