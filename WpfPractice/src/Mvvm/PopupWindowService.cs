using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfPractice.Mvvm
{
  /// <summary>
  /// Сервис создания Popup окна.
  /// </summary>
  /// <typeparam name="T">UserControl, находящийся внутри popup окна.</typeparam>
  internal sealed class PopupWindowService<T> where T : UserControl, new()
  {
    #region Поля и свойства

    /// <summary>
    /// DataContext окна.
    /// </summary>
    private readonly object dataContext;

    /// <summary>
    /// Заголовок окна.
    /// </summary>
    private readonly string title;

    /// <summary>
    /// Окно.
    /// </summary>
    private Window? window;

    #endregion

    #region Методы

    /// <summary>
    /// Отобразить окно.
    /// </summary>
    public void Show()
    {
      if (this.window == null)
      {
        this.window = new PopupWindow
        {
          Content = new T(),
          DataContext = this.dataContext,
          Title = this.title,
          Owner = App.Current.MainWindow,
        };

        this.window.Closed += this.ClosedHandler;

        this.window.Show();
      }
      else
      {
        if (this.window.WindowState == WindowState.Minimized)
          this.window.WindowState = WindowState.Normal;
      }
    }

    private void ClosedHandler(object sender, EventArgs e)
    {
      this.window = null;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="dataContext">Дата контекст для UserControl типа <typeparamref name="T"/></param>
    /// <param name="title">Заголовок окна.</param>
    public PopupWindowService(object dataContext, string title)
    {
      this.dataContext = dataContext;
      this.title = title;
    }

    #endregion
  }
}
