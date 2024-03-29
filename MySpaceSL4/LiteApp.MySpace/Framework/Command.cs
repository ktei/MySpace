﻿using System;
using System.Windows.Input;

namespace LiteApp.MySpace.Framework
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Predicate<Object> _canExecute = null;
        Action<Object> _executeAction = null;

        public Command(Action<object> executeAction)
        {
            _executeAction = executeAction;
        }

        public Command(Predicate<Object> canExecute, Action<object> executeAction)
        {
            _canExecute = canExecute;
            _executeAction = executeAction;
        }


        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute(parameter);

            return true;
        }

        public void UpdateCanExecuteState()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
        public void Execute(object parameter)
        {
            if (_executeAction != null)
                _executeAction(parameter);



            UpdateCanExecuteState();
        }
    }

}
