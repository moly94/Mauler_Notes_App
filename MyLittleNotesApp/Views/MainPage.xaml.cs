using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyLittleNotesApp.ViewModels;
using MyLittleNotesApp.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyLittleNotesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            InitializeComponent();
        }
        public MainViewModel ViewModel => DataContext as MainViewModel;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, se) =>
            {
                this.Frame.Navigate(typeof(MainPage));
            };
            base.OnNavigatedTo(e);

        }

        private void CreateNewNote_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewNoteView));
        }

        private void ReadNote_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReadNotesView));
        }

        private void SearchNote_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SearchNotesView));
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettignsView));
        }
    }
}
