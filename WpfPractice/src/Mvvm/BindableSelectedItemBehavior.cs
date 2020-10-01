using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WpfPractice.Mvvm
{
  /// <summary>
  /// Поведение, расширяющее TreeView в сторону биндинга свойства зависимости SelectedItem.
  /// </summary>
  internal sealed class BindableSelectedItemBehavior : Behavior<TreeView>
  {
    #region Поля и свойства

    /// <summary>
    /// Свойство зависимости.
    /// </summary>
    public static readonly DependencyProperty SelectedItemProperty =
      DependencyProperty.Register("SelectedItem", typeof(object), typeof(BindableSelectedItemBehavior),
        new UIPropertyMetadata(null, SelectedItemPropertyChangedHandler));

    /// <summary>
    /// Свойство - обертка над SelectedItemProperty.
    /// </summary>
    public object SelectedItem
    {
      get { return this.GetValue(SelectedItemProperty); }
      set { this.SetValue(SelectedItemProperty, value); }
    }

    #endregion

    #region Методы

    private static void SelectedItemPropertyChangedHandler(DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue is TreeViewItem item)
      {
        item.SetValue(TreeViewItem.IsSelectedProperty, true);
      }
    }

    private void SelectedItemChangedHandler(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      this.SelectedItem = e.NewValue;
    }

    #endregion

    #region Базовый класс

    protected override void OnAttached()
    {
      base.OnAttached();

      this.AssociatedObject.SelectedItemChanged += this.SelectedItemChangedHandler;
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();

      if (this.AssociatedObject != null)
      {
        this.AssociatedObject.SelectedItemChanged -= this.SelectedItemChangedHandler;
      }
    }

    #endregion
  }
}
