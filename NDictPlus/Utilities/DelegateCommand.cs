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
        bool canExecute;

        public DelegateCommand(Action<T> action, bool canExecuteNow = true)
        {
            this.action = action;
            this.canExecute = canExecuteNow;
        }

        public event EventHandler CanExecuteChanged;

        public void LetExecutableIf(bool value)
        {
            if (value == canExecute) return;
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
        bool canExecute;

        public DelegateCommand(Action action, bool canExecuteNow = true)
        {
            this.action = action;
            this.canExecute = canExecuteNow;
        }

        public event EventHandler CanExecuteChanged;

        public void UpdateExecutablity(bool value)
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
