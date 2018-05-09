using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP.DTO;
using UWP.Services.Interfaces;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.Views.Dialogs
{
    public sealed partial class AddDevice : ContentDialog
    {
        private readonly IGroupService _groupService;
        private long _groupId;
        public GroupSecurityDeviceDto Device;
        public AddDevice(long groupId)
        {
            this.InitializeComponent();

            _groupService = App.Container.GetRequiredService<IGroupService>();

            _groupId = groupId;
        }

        private async void ContentDialog_CreateButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Device = await _groupService.AddNewSecurityDevice(txbDeviceName.Text, _groupId);
            await Task.Delay(750);
            Hide();
        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            txbDeviceName.Text = "";
            Hide();
        }
    }
}
