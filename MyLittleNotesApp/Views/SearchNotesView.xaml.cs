using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyLittleNotesApp.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyLittleNotesApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchNotesView : Page
    {
        public SearchNotesView()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            this.InitializeComponent();
            if (rootFrame.CanGoBack)
            {
                //Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility=
                //    AppViewBackButtonVisibility.Visible;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, se) =>
            {
                this.Frame.Navigate(typeof(MainPage));
            };
            base.OnNavigatedTo(e);
        }

        public MainViewModel ViewModel => DataContext as MainViewModel;
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested -= PageName_BackRequested;
            base.OnNavigatingFrom(e);
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NoteDetails));
        }
    }
}
