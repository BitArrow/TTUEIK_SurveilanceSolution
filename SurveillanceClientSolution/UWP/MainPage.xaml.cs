using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UWP.DTO;
using UWP.Services;
using UWP.Services.Interfaces;
using UWP.ViewModels;
using UWP.Views;
using UWP.Views.Dialogs;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageVm _vm;
        private IB2CService _b2c;
        private IGroupService _groupService;

        public MainPage()
        {
            InitializeComponent();
            
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _b2c = App.Container.GetRequiredService<IB2CService>();
            _groupService = App.Container.GetRequiredService<IGroupService>();

            _vm = new MainPageVm();
            DataContext = _vm;
            _vm.LoadGroups();

            UpdateSignInState();
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            await _b2c.Login();
            UpdateSignInState();
        }

        private async void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            await _b2c.LogOut();
            UpdateSignInState();
        }

        private async void CallApiBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var call = await _groupService.GetListAsync();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }

        private void UpdateSignInState()
        {
            bool loggedIn = !string.IsNullOrEmpty(App.BearerToken);
            if (loggedIn)
            {
                LogoutBtn.Visibility = Visibility.Visible;
                LoginBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                LogoutBtn.Visibility = Visibility.Collapsed;
                LoginBtn.Visibility = Visibility.Visible;
            }
        }

        private void lvGroups_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is GroupDto selected)
                this.Frame.Navigate(typeof(GroupView), selected);
        }

        private async void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            AddGroup dialog = new AddGroup();
            await dialog.ShowAsync();

            _vm.LoadGroups();
        }

        private void btnRefreshQuestion_Click(object sender, RoutedEventArgs e)
        {
            _vm.LoadGroups();
        }

        bool on = false;

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //_vm.SendTestMessage();
            if (!on)
            {
                _vm.StartServer();
                tbServerStatus.Text = "On";
                on = !on;
            }
            else
            {
                _vm.StopServer();
                tbServerStatus.Text = "Off";
                on = !on;
            }
        }
    }
}
