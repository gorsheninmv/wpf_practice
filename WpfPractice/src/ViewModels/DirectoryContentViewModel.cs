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
    public ObservableCollection<string> DirectoryContent { get; }

    /// <summary>
    /// Полное имя директории.
    /// </summary>
    public string DirectoryFullPath { get; }

    #endregion

    #region Базовый класс

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj))
        return true;

      if (obj is DirectoryContentViewModel vm)
        return this.DirectoryFullPath == vm.DirectoryFullPath;

      return false;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="model">Модель содержимого директории.</param>
    /// <param name="directoryFullPath">Полное имя директории.</param>
    public DirectoryContentViewModel(ObservableCollection<string> model, string directoryFullPath)
    {
      this.DirectoryContent = model;
      this.DirectoryFullPath = directoryFullPath;
    }

    #endregion
  }
}
