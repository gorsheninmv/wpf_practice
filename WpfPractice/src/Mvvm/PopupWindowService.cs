using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WpfPractice.Mvvm
{
  /// <summary>
  /// Сервис создания Popup окна.
  /// </summary>
  /// <remarks>Поддерживает повторное открытие уже существующего окна.</remarks>
  internal static class PopupWindowService
  {
    #region Поля и свойства

    /// <summary>
    /// Список открытых popup окон.
    /// </summary>
    private static readonly List<Window> openedPopUps;

    #endregion

    #region Методы

    /// <summary>
    /// Отобразить окно по его ViewModel.
    /// </summary>
    public static void OpenViewModel(object viewModel, string title)
    {
      Window openedPopup = openedPopUps.FirstOrDefault(win => win.Content.Equals(viewModel));

      if (openedPopup == null)
      {
        var window = new PopupWindow
        {
          Content = viewModel,
          Title = title,
          Owner = App.Current.MainWindow,
        };

        window.Closed += ClosedHandler;

        openedPopUps.Add(window);

        window.Show();
      }
      else
      {
        if (openedPopup.WindowState == WindowState.Minimized)
          openedPopup.WindowState = WindowState.Normal;
      }
    }

    private static void ClosedHandler(object sender, EventArgs e)
    {
      if (sender is Window popup)
        openedPopUps.Remove(popup);
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static PopupWindowService()
    {
      openedPopUps = new List<Window>();
    }

    #endregion
  }
}
