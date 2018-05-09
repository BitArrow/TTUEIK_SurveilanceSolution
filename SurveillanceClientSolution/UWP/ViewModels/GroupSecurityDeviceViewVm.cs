using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.DTO;
using UWP.Services.Interfaces;

namespace UWP.ViewModels
{
    public class GroupSecurityDeviceViewVm : BaseVm
    {
        private readonly IGroupService _groupService;

        private string _groupTitle;

        public string GroupTitle
        {
            get { return _groupTitle; }
            set { _groupTitle = value; }
        }

        private GroupSecurityDeviceDto _device;

        public GroupSecurityDeviceDto Device
        {
            get { return _device; }
            set { _device = value;
                RaisePropertyChanged("Device");
            }
        }

        public GroupSecurityDeviceViewVm(string groupName, long groupId, long deviceId)
        {
            _groupService = App.Container.GetRequiredService<IGroupService>();
            GroupTitle = groupName;

            GetDeviceAsync(groupId, deviceId);
        }

        public GroupSecurityDeviceViewVm(string groupName, GroupSecurityDeviceDto device)
        {
            _groupService = App.Container.GetRequiredService<IGroupService>();
            GroupTitle = groupName;
            Device = device;
        }

        public async void GetDeviceAsync(long groupId, long deviceId)
        {
            Device = await _groupService.GetSecurityDevice(groupId, deviceId);
        }
    }
}
