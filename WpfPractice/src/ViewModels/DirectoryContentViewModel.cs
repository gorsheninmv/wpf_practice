using System.Collections.ObjectModel;
using WpfPractice.Mvvm;

namespace WpfPractice.ViewModels
{
  /// <summary>
  /// ViewModel содержимого директори.
  /// </summary>
  internal sealed class DirectoryContentViewModel : NotifyPropertyChangedBase
  {
    #region Поля и свойства

    /// <summary>
    /// Содержимое директории.
    /// </summary>
    public ObservableCollection<string> Content { get; }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="model">Модель содержимого директории.</param>
    public DirectoryContentViewModel(ObservableCollection<string> model)
    {
      this.Content = model;
    }

    #endregion
  }
}
