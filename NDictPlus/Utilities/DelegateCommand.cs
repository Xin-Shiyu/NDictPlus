using System;
using System.Windows.Input;

namespace NDictPlus.Utilities
{
    class StatedDelegateCommand<T> : ICommand 
        where T : class
    {
        readonly Action<T> action;
        bool canExecute;

        public StatedDelegateCommand(Action<T> action, bool canExecuteNow = true)
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

    class StatedDelegateCommand : ICommand
    {
        readonly Action action;
        bool canExecute;

        public StatedDelegateCommand(Action action, bool canExecuteNow = true)
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

    class CuriousDelegateCommand<T> : ICommand
        where T : class
    {
        readonly Action<T> action;
        readonly Func<T, bool> canExecuteFunc;

        public CuriousDelegateCommand(Action<T> action, Func<T, bool> canExecuteFunc)
        {
            this.action = action;
            this.canExecuteFunc = canExecuteFunc;
        }

        public event EventHandler CanExecuteChanged;

        public void UpdateExecutability()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter) => canExecuteFunc.Invoke(parameter as T);

        public void Execute(object parameter)
        {
            action.Invoke(parameter as T);
        }
    }

    class CuriousDelegateCommand : ICommand
    {
        readonly Action action;
        readonly Func<bool> canExecuteFunc;

        public CuriousDelegateCommand(Action action, Func<bool> canExecuteFunc)
        {
            this.action = action;
            this.canExecuteFunc = canExecuteFunc;
        }

        public event EventHandler CanExecuteChanged;

        public void UpdateExecutability()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter) => canExecuteFunc.Invoke();

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }
}
