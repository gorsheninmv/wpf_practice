﻿using System;
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
    private readonly DirectoryTree? parent;

    private string name;

    /// <summary>
    /// Название директории.
    /// </summary>
    public string Name
    {
      get
      {
        return this.name;
      }

      set
      {
        this.name = value;
        this.OnPropertyChanged(nameof(this.Name));
      }
    }

    private bool isExpanded;

    /// <summary>
    /// Раскрыт ли узел для показа содержимого.
    /// </summary>
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
            directory.LoadSubdirectories();
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
        fileFullPaths = Array.Empty<string>();
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
        directoryFullPaths = Array.Empty<string>();
      }

      var ret = directoryFullPaths.Select(Path.GetFileName).ToArray();
      return ret;
    }

    /// <summary>
    /// Возвращает полный путь директории.
    /// </summary>
    /// <returns>Полный путь директории.</returns>
    public string GetFullPath()
    {
      var parentPath = this.parent?.GetFullPath() ?? string.Empty;
      return Path.Combine(parentPath, this.name);
    }

    /// <summary>
    /// Подгрузить поддиректории.
    /// </summary>
    private void LoadSubdirectories()
    {
      if (this.Subdirectories.Count > 0)
        return;

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
      this.LoadSubdirectories();

      foreach (var directory in this.Subdirectories)
        directory.LoadSubdirectories();
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
