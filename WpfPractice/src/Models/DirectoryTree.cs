using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using WpfPractice.Mvvm;

namespace WpfPractice.Models
{
  /// <summary>
  /// Модель директории.
  /// </summary>
  internal sealed class DirectoryTree : NotifyPropertyChangedBase
  {
    #region Поля и свойства

    /// <summary>
    /// Родительская директория.
    /// </summary>
    private DirectoryTree? parent;

    /// <summary>
    /// Название директории.
    /// </summary>
    private string name;

    public string Name
    {
      get
      {
        return this.name;
      }

      set
      {
        this.name = value;
        OnPropertyChanged(nameof(this.Name));
      }
    }

    /// <summary>
    /// Раскрыт ли узел для показа содержимого.
    /// </summary>
    private bool isExpanded;

    public bool IsExpanded
    {
      get
      {
        return this.isExpanded;
      }

      set
      {
        this.isExpanded = value;

        if (this.isExpanded)
        {
          foreach (var directory in this.Subdirectories)
            directory.LoadSubdirs();
        }
      }
    }

    /// <summary>
    /// Коллекция дочерних директорий.
    /// </summary>
    public ObservableCollection<DirectoryTree> Subdirectories { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Возвращает короткие имена файлов в текущей директории.
    /// </summary>
    /// <returns>Короткие имена файлов.</returns>
    public string[] GetFiles()
    {
      string fullPath = this.GetFullPath();
      string[]? fileFullPaths = null;

      try
      {
        fileFullPaths = Directory.GetFiles(fullPath, "*", SearchOption.TopDirectoryOnly);
      }
      catch (UnauthorizedAccessException)
      {
        fileFullPaths = new string[0];
      }

      var ret = fileFullPaths.Select(Path.GetFileName).ToArray();
      return ret;
    }

    /// <summary>
    /// Возвращает короткие имена поддиректорий в текущей директории.
    /// </summary>
    /// <returns>Короткие имена поддиректорий.</returns>
    public string[] GetDirectories()
    {
      string fullPath = this.GetFullPath();
      string[]? directoryFullPaths = null;

      try
      {
        directoryFullPaths = Directory.GetDirectories(fullPath, "*", SearchOption.TopDirectoryOnly);
      }
      catch (UnauthorizedAccessException)
      {
        directoryFullPaths = new string[0];
      }

      var ret = directoryFullPaths.Select(Path.GetFileName).ToArray();
      return ret;
    }

    /// <summary>
    /// Возвращает полный путь директории.
    /// </summary>
    /// <returns>Полный путь директории.</returns>
    private string GetFullPath()
    {
        var parentPath = this.parent?.GetFullPath() ?? string.Empty;
        return Path.Combine(parentPath, this.name);
    }

    /// <summary>
    /// Подгрузить поддиректории.
    /// </summary>
    private void LoadSubdirs()
    {
      string nodeFullPath = this.GetFullPath();

      string[]? subdirsFullPath = null;

      try
      {
        subdirsFullPath = Directory.GetDirectories(nodeFullPath, "*", SearchOption.TopDirectoryOnly);
      }
      catch (UnauthorizedAccessException)
      {
        return;
      }

      foreach (var fullPath in subdirsFullPath)
      {
        var subdirName = Path.GetFileName(fullPath);
        var directory = new DirectoryTree(subdirName, this);
        this.Subdirectories.Add(directory);
      }
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="name">Название директории.</param>
    /// <remarks>Если директория корневая, то родительская директория null.</remarks>
    public DirectoryTree(string name) : this(name, null)
    {
      this.LoadSubdirs();

      foreach (var directory in this.Subdirectories)
        directory.LoadSubdirs();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="name">Название директории.</param>
    /// <param name="parent">Родительский узел.</param>
    private DirectoryTree(string name, DirectoryTree? parent)
    {
      this.name = name;
      this.Subdirectories = new ObservableCollection<DirectoryTree>();
      this.parent = parent;
    }

    #endregion
  }
}
