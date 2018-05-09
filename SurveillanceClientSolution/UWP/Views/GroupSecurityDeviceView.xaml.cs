using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.DTO;
using UWP.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWP.Views.Dialogs;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupSecurityDeviceView : Page
    {
        private GroupSecurityDeviceViewVm _vm;
        public GroupSecurityDeviceView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                this.Frame.GoBack();//.Navigate(typeof(MainPage));
            }
            else if (e.Parameter is Tuple<string, GroupSecurityDeviceDto>)
            {
                var param = e.Parameter as Tuple<string, GroupSecurityDeviceDto>;
                _vm = new GroupSecurityDeviceViewVm(param.Item1, param.Item2);
            }
            else if (e.Parameter is Tuple<string, long, long>) // groupName, groupId, deviceId
            {
                var param = e.Parameter as Tuple<string, long, long>;
                _vm = new GroupSecurityDeviceViewVm(param.Item1, param.Item2, param.Item3);
            }

            DataContext = _vm;

            base.OnNavigatedTo(e);
        }


        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteDevice dialog = new DeleteDevice(_vm.Device.GroupId, _vm.Device.Id);
            await dialog.ShowAsync();
            if (dialog.DeleteConfirmed)
                Frame.GoBack();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // TODO: add edit functionality
        }
    }
}
