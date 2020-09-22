using System;
using System.Windows.Input;

namespace WpfPractice.Mvvm
{
  /// <summary>
  /// Инфраструктура для вызова команд.
  /// </summary>
  /// <typeparam name="T">Тип аргумента, получаемый из команды.</typeparam>
  internal sealed class Command<T> : ICommand
  {
    #region Поля и свойства

    /// <summary>
    /// Логика выполнения.
    /// </summary>
    private readonly Action<T> execute;

    /// <summary>
    /// Логика состояния выполнения.
    /// </summary>
    private readonly Predicate<T>? canExecute;

    #endregion

    #region ICommand

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return this.canExecute == null || this.canExecute((T) parameter);
    }

    public void Execute(object parameter)
    {
      this.execute((T) parameter);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Создает новую команду.
    /// </summary>
    /// <param name="execute">Логика выполнения.</param>
    /// <param name="canExecute">Логика состояния выполнения.</param>
    /// <remarks> Если <paramref name="canExecute"/> null, то <paramref name="execute"/> исполняется всегда.</remarks>
    public Command(Action<T> execute, Predicate<T>? canExecute = null)
    {
      this.execute = execute;
      this.canExecute = canExecute;
    }

    #endregion
  }
}
