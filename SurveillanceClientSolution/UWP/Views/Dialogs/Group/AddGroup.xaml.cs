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
    public sealed partial class AddGroup : ContentDialog
    {
        private readonly IGroupService _groupService;
        public AddGroup()
        {
            this.InitializeComponent();

            _groupService = App.Container.GetRequiredService<IGroupService>();
        }

        private async void ContentDialog_AddButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await _groupService.AddGroupAsync(txbGroupName.Text);
            await Task.Delay(750);
            Hide();
        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            txbGroupName.Text = "";
            Hide();
        }
    }
}
