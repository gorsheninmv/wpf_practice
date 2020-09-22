using System.Windows;
using WpfPractice.ViewModels;

namespace WpfPractice.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      this.DataContext = new FileManagerViewModel();
      InitializeComponent();
    }
  }
}
