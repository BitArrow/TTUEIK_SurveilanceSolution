using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP.DTO;
using UWP.ViewModels;
using UWP.Views.Dialogs;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupView : Page
    {
        private GroupViewVm _vm;

        public GroupView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _vm = new GroupViewVm();
            if (e.Parameter == null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            else if (e.Parameter is GroupDto)
            {
                _vm.GetGroup((e.Parameter as GroupDto).Id);
            }

            DataContext = _vm;

            base.OnNavigatedTo(e);
        }

        private async void btnEditGroup_Click(object sender, RoutedEventArgs e)
        {
            EditGroup dialog = new EditGroup(_vm.Group);
            await dialog.ShowAsync();

            _vm.GetGroup(_vm.Group.Id);
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteGroup dialog = new DeleteGroup(_vm.Group.Id);
            await dialog.ShowAsync();
            if (dialog.DeleteConfirmed)
                Frame.GoBack();
        }

        private async void btnAddDevice_Click(object sender, RoutedEventArgs e)
        {
            AddDevice dialog = new AddDevice(_vm.Group.Id);
            await dialog.ShowAsync();
            if (dialog.Device != null)
            {
                Frame.Navigate(typeof(GroupSecurityDeviceView), new Tuple<string, GroupSecurityDeviceDto>(_vm.Group.Name, dialog.Device));
                _vm.Devices.Add(dialog.Device);
            }
        }

        private void lvDevices_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is GroupSecurityDeviceDto selected)
                //this.Frame.Navigate(typeof(GroupSecurityDeviceDto), new Tuple<string, long, long>(_vm.Group.Name, _vm.Group.Id , selected.Id));
                Frame.Navigate(typeof(GroupSecurityDeviceView), new Tuple<string, GroupSecurityDeviceDto>(_vm.Group.Name, selected));
        }
    }
}
