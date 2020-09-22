using System.ComponentModel;

namespace WpfPractice.Mvvm
{
  /// <summary>
  /// Базовый класс для типов, реализующий интерфейс INotifyPropertyChanged.
  /// </summary>
  internal abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
  {
    #region Методы

    /// <summary>
    /// Вызов события.
    /// </summary>
    /// <param name="propertyName">Имя свойства, об изменении которого необходимо уведомить.</param>
    protected void OnPropertyChanged(string propertyName)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion
  }
}
