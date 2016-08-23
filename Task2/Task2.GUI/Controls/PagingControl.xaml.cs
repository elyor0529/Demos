using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Task2.GUI.Controls
{
  /// <summary>
  ///   Interaction logic for PagingControl.xaml.
  /// </summary>
  public partial class PagingControl
  {
    #region Static Constructor. dEclaration of Dependency properties

    /// <summary>
    ///   Initializes static members of the <see cref="PagingControl" /> class.
    /// </summary>
    static PagingControl()
    {
      var md = new UIPropertyMetadata(0, PropertyTotalCountChanged);
      TotalCountProperty = DependencyProperty.Register("TotalCount", typeof(int), typeof(PagingControl), md);
      var md1 = new UIPropertyMetadata(0, PropertyPageIndexChanged);
      PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(int), typeof(PagingControl), md1);
      var md2 = new UIPropertyMetadata(0, PropertyPageSizeChanged);
      PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(PagingControl), md2);

      // Registro del Comando.
      ChangedIndexCommandProperty =
        DependencyProperty.Register("ChangedIndexCommand", typeof(ICommand), typeof(PagingControl),
          new UIPropertyMetadata(null));
    }

    #endregion

    #region Constructor

    /// <summary>
    ///   Initializes a new instance of the <see cref="PagingControl" /> class.
    /// </summary>
    public PagingControl()
    {
      InitializeComponent();

      TotalCount = 0;
      PageIndex = 1;
      CbPageSize.SelectedIndex = 0;
      IsControlVisible = true;
      HasNextPage = false;
      HasPreviousPage = false;
    }

    #endregion

    #region Dependency Command Declaration

    /// <summary>
    ///   Gets or sets NextIndexCommand.
    /// </summary>
    /// <value>
    ///   The next index command.
    /// </value>
    public ICommand ChangedIndexCommand
    {
      get { return (ICommand) GetValue(ChangedIndexCommandProperty); }
      set { SetValue(ChangedIndexCommandProperty, value); }
    }

    #endregion

    #region Metodos Publicos

    /// <summary>
    ///   Lleva el page index a 1.
    /// </summary>
    public void ResetPageIndex()
    {
      PageIndex = 1;
    }

    #endregion

    #region Dependency properties Declarations

    /// <summary>
    ///   Total Count.
    /// </summary>
    public static readonly DependencyProperty TotalCountProperty;

    /// <summary>
    ///   Page Index.
    /// </summary>
    public static readonly DependencyProperty PageIndexProperty;

    /// <summary>
    ///   Page Size.
    /// </summary>
    public static readonly DependencyProperty PageSizeProperty;

    /// <summary>
    ///   Dependency command property declaration.
    /// </summary>
    public static readonly DependencyProperty ChangedIndexCommandProperty;

    /// <summary>
    ///   Gets or sets TotalRow.
    /// </summary>
    /// <value>
    ///   The total row.
    /// </value>
    public int TotalCount
    {
      get { return (int) GetValue(TotalCountProperty); }

      set { SetValue(TotalCountProperty, value); }
    }

    /// <summary>
    ///   Gets or sets ActualPaging.
    /// </summary>
    /// <value>
    ///   The actual paging.
    /// </value>
    public int PageIndex
    {
      get { return (int) GetValue(PageIndexProperty); }

      set { SetValue(PageIndexProperty, value); }
    }

    /// <summary>
    ///   Gets or sets PageSize.
    /// </summary>
    /// <value>
    ///   The page size.
    /// </value>
    public int PageSize
    {
      get { return (int) GetValue(PageSizeProperty); }

      set { SetValue(PageSizeProperty, value); }
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets or sets a value indicating whether IsVisible.
    /// </summary>
    /// <value>
    ///   The is visible.
    /// </value>
    public bool IsControlVisible
    {
      get { return Visibility == Visibility.Visible; }
      set { Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
    }

    /// <summary>
    ///   Gets TotalPages.
    /// </summary>
    /// <value>
    ///   The total pages.
    /// </value>
    public int TotalPages
    {
      get
      {
        // Calcule el número de paginas necesarias
        if (PageSize > 0)
        {
          var tc = TotalCount/PageSize;
          tc = tc*PageSize < TotalCount ? tc + 1 : tc;
          return tc;
        }

        return 1;
      }
    }

    /// <summary>
    ///   Gets a value indicating whether HasPreviousPage.
    /// </summary>
    /// <value>
    ///   The has previous page.
    /// </value>
    public bool HasPreviousPage
    {
      get { return BtnFirst.IsEnabled; }
      internal set { BtnFirst.IsEnabled = BtnPrevious.IsEnabled = value; }
    }

    /// <summary>
    ///   Gets a value indicating whether HasNextPage.
    /// </summary>
    /// <value>
    ///   The has next page.
    /// </value>
    public bool HasNextPage
    {
      get { return BtnLast.IsEnabled; }
      internal set { BtnLast.IsEnabled = BtnNext.IsEnabled = value; }
    }

    #endregion

    #region Refactoring Configuracion de Elementos

    /// <summary>
    ///   Config Visibility, and pageSize.
    /// </summary>
    /// <param name="gp">
    ///   The gp.
    /// </param>
    private static void ConfigureValoresInternos(PagingControl gp)
    {
      // Set the Total de paginas....
      gp.LTotalPagina.Content = gp.TotalPages;

      // Set the pageSize control
      foreach (ComboBoxItem comboBoxItem in gp.CbPageSize.Items)
      {
        var cbi = Convert.ToInt32(comboBoxItem.Content);
        if (cbi == gp.PageSize)
        {
          gp.CbPageSize.SelectedItem = comboBoxItem;
          break;
        }
      }

      // if the setted value in Page size is not in list, return to original value.
      var sel = (ComboBoxItem) gp.CbPageSize.SelectedItem;
      gp.PageSize = Convert.ToInt32(sel.Content);

      // Set the visibility of Pagination Buttons.
      gp.ButtonGrid.Visibility = gp.TotalCount > gp.PageSize
        ? Visibility.Visible
        : Visibility.Hidden;

      // Calculate the HasNextPage and previous page
      gp.HasPreviousPage = gp.PageIndex > 1;
      gp.HasNextPage = gp.TotalPages > gp.PageIndex;
    }

    /// <summary>
    ///   Execute the command if it is assigned.
    /// </summary>
    private void ExecuteCommandChangeIndex()
    {
      // Test if the command index is asigned.
      if (ChangedIndexCommand != null)
      {
        ChangedIndexCommand.Execute(null);
      }
    }

    #endregion

    #region Eventos controles

    /// <summary>
    ///   Change the Page Size Property control.
    ///   This make that PageIndex go to 1.
    /// </summary>
    /// <param name="d">
    ///   The d.
    /// </param>
    /// <param name="e">
    ///   The e.
    /// </param>
    private static void PropertyPageSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var gp = (PagingControl) d;

      // int pagesize = (int)e.NewValue;
      ConfigureValoresInternos(gp);
      gp.PageIndex = 1;
    }

    /// <summary>
    ///   Evento para actualizar el page index.
    /// </summary>
    /// <param name="d">
    ///   The d.
    /// </param>
    /// <param name="e">
    ///   The e.
    /// </param>
    private static void PropertyPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      // Update Total Count label....
      var gp = (PagingControl) d;
      var actualPage = (int) e.NewValue;
      gp.LPagina.Content = actualPage;
      ConfigureValoresInternos(gp);
    }

    /// <summary>
    ///   Evento cuando cambia la cantidad total de Registros.
    /// </summary>
    /// <param name="d">
    ///   The d.
    /// </param>
    /// <param name="e">
    ///   The e.
    /// </param>
    private static void PropertyTotalCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      // Update Total Count label....
      var gp = (PagingControl) d;
      gp.LTotal.Content = string.Format("{0} запис", e.NewValue);
      ConfigureValoresInternos(gp);
    }

    /// <summary>
    ///   Cambia la selección del PageSize.
    /// </summary>
    /// <param name="sender">
    ///   The sender.
    /// </param>
    /// <param name="e">
    ///   The e.
    /// </param>
    private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var items = e.AddedItems;
      if (items != null && items.Count > 0)
      {
        var value = ((ComboBoxItem) items[0]).Content;
        PageSize = Convert.ToInt32(value);
        if (TotalCount > 0)
        {
          ExecuteCommandChangeIndex();
        }
      }
    }

    #endregion

    #region Button events for Index Control

    /// <summary>
    ///   Increment the Page Index, and invoke ChangeIndexCommand.
    /// </summary>
    /// <param name="sender">
    ///   The sender.
    /// </param>
    /// <param name="e">
    ///   The e.
    /// </param>
    private void BtnNextClick(object sender, RoutedEventArgs e)
    {
      if (PageIndex < TotalPages)
      {
        PageIndex++;
        ExecuteCommandChangeIndex();
      }
    }

    /// <summary>
    ///   Increment the Page Index to the last index, and invoke ChangeIndexCommand.
    /// </summary>
    /// <param name="sender">
    ///   The sender.
    /// </param>
    /// <param name="e">
    ///   The e.
    /// </param>
    private void BtnLastClick(object sender, RoutedEventArgs e)
    {
      var page = TotalPages;
      PageIndex = page;
      ExecuteCommandChangeIndex();
    }

    /// <summary>
    ///   Decrement the Page Index, and invoke ChangeIndexCommand.
    /// </summary>
    /// <param name="sender">
    ///   The sender.
    /// </param>
    /// <param name="e">
    ///   The e.
    /// </param>
    private void BtnPreviousClick(object sender, RoutedEventArgs e)
    {
      if (PageIndex > 1)
      {
        PageIndex--;
        ExecuteCommandChangeIndex();
      }
    }

    /// <summary>
    ///   Go to first index, and invoke ChangeIndexCommand.
    /// </summary>
    /// <param name="sender">
    ///   The sender.
    /// </param>
    /// <param name="e">
    ///   The e.
    /// </param>
    private void BtnFirstClick(object sender, RoutedEventArgs e)
    {
      const int page = 1;
      PageIndex = page;
      ExecuteCommandChangeIndex();
    }

    #endregion
  }
}