using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NDictPlus.Utilities
{
    class DelegateCommand<T> : ICommand 
        where T : class
    {
        readonly Action<T> action;
        bool canExecute = true;

        public DelegateCommand(Action<T> action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public void ChangeCanExecute(bool value)
        {
            canExecute = value;
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter) => canExecute;

        public void Execute(object parameter)
        {
            action.Invoke(parameter as T);
        }
    }

    class DelegateCommand : ICommand
    {
        readonly Action action;
        bool canExecute = true;

        public DelegateCommand(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public void ChangeCanExecute(bool value)
        {
            canExecute = value;
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter) => canExecute;

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }
}
