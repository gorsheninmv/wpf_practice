using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using WpfPractice.Models;
using WpfPractice.Mvvm;

namespace WpfPractice.ViewModels
{
  /// <summary>
  /// ViewModel отображения файлов и папок.
  /// </summary>
  internal sealed class FileManagerViewModel : NotifyPropertyChangedBase
  {
    #region Поля и свойства

    /// <summary>
    /// Дерево папок.
    /// </summary>
    public ObservableCollection<DirectoryTree> DirectoryTree { get; }

    /// <summary>
    /// Список файлов.
    /// </summary>
    public ObservableCollection<string> Files { get; }

    private DirectoryTree? selectedItem;

    /// <summary>
    /// Выбранный элемент дерева папок.
    /// </summary>
    public object? SelectedItem
    {
      get
      {
        return this.selectedItem;
      }

      set
      {
        if (value is DirectoryTree selectedItem)
        {
          this.selectedItem = selectedItem;
          this.UpdateFilesCollection();
          this.UpdateDirectoryContentCollection();
          this.IsOpenEnabled = true;
        }
        else
        {
          this.Files.Clear();
          this.directoryContent.Clear();
          this.IsOpenEnabled = false;
        }
      }
    }

    private bool isOpenEnabled;

    /// <summary>
    /// Разрешено ли открывать выбранную папку.
    /// </summary>
    public bool IsOpenEnabled
    {
      get
      {
        return this.isOpenEnabled;
      }

      set
      {
        this.isOpenEnabled = value;
        this.OnPropertyChanged(nameof(this.IsOpenEnabled));
      }
    }

    private ICommand? openTreeNode;

    /// <summary>
    /// Команда открыть.
    /// </summary>
    public ICommand OpenTreeNode => this.openTreeNode ??= new Command<object>(this.OpenContentView);

    /// <summary>
    /// Содержимое выбранной папки в дереве.
    /// </summary>
    private readonly ObservableCollection<string> directoryContent;

    #endregion

    #region Методы

    /// <summary>
    /// Построить дерево папок.
    /// </summary>
    /// <returns></returns>
    private ObservableCollection<DirectoryTree> BuildDirectoryTree()
    {
      string driveName = Directory.GetDirectoryRoot(AppDomain.CurrentDomain.BaseDirectory);

      var root = new DirectoryTree(driveName);
      var ret = new ObservableCollection<DirectoryTree> { root };
      return ret;
    }

    /// <summary>
    /// Открыть окно с содержимым выбранной папки.
    /// </summary>
    /// <param name="sender">Отправитель команды.</param>
    private void OpenContentView(object sender)
    {
      string directoryPath = this.selectedItem?.GetFullPath() ?? string.Empty;
      var viewModel = new DirectoryContentViewModel(this.directoryContent, directoryPath);
      PopupWindowService.OpenViewModel(viewModel, directoryPath);
    }

    /// <summary>
    /// Обновить содержимое свойства <see cref="Files"/>.
    /// </summary>
    private void UpdateFilesCollection()
    {
      string[] files = this.selectedItem?.GetFiles() ?? new string[0];

      this.Files.Clear();

      foreach (var file in files)
        this.Files.Add(file);
    }

    /// <summary>
    /// Обновить содержимое поля <see cref="directoryContent"/>.
    /// </summary>
    private void UpdateDirectoryContentCollection()
    {
      IEnumerable<string> files = this.selectedItem?.GetFiles() ?? new string[0];
      IEnumerable<string> directories = (this.selectedItem?.GetDirectories() ?? new string[0])
        .Select(directory => "+ " + directory);
      IEnumerable<string> items = directories.Concat(files);

      this.directoryContent.Clear();

      foreach (var item in items)
        this.directoryContent.Add(item);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FileManagerViewModel()
    {
      this.directoryContent = new ObservableCollection<string>();
      this.Files = new ObservableCollection<string>();
      this.DirectoryTree = this.BuildDirectoryTree();
    }

    #endregion
  }
}
