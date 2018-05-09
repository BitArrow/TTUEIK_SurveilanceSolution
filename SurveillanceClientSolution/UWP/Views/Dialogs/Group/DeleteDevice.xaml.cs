using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class DeleteDevice : ContentDialog
    {
        private readonly IGroupService _groupService;
        private long _groupId;
        private long _deviceId;
        public bool DeleteConfirmed { get; set; }
        public DeleteDevice(long groupId, long deviceId)
        {
            this.InitializeComponent();

            _groupService = App.Container.GetRequiredService<IGroupService>();

            _groupId = groupId;
            _deviceId = deviceId;
        }

        private async void ContentDialog_DeleteButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DeleteConfirmed = true;
            await _groupService.DeleteSecurityDevice(_groupId, _deviceId);
            await Task.Delay(750);
            Hide();
        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _groupId = 0;
            _deviceId = 0;
            Hide();
        }
    }
}
