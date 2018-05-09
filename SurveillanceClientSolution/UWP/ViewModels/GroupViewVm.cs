using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.DTO;
using UWP.Services.Interfaces;

namespace UWP.ViewModels
{
    public class GroupViewVm : BaseVm
    {
        private GroupDto _group;
        private ObservableCollection<GroupUserDto> _groupUsers;
        private ObservableCollection<GroupSecurityDeviceDto> _devices;

        #region properties
        public GroupDto Group
        {
            get { return _group; }
            set { _group = value;
                RaisePropertyChanged("Group");
            }
        }

        public ObservableCollection<GroupUserDto> Users
        {
            get { return _groupUsers; }
            set { _groupUsers = value;
                RaisePropertyChanged("Users");
            }
        }

        public ObservableCollection<GroupSecurityDeviceDto> Devices
        {
            get { return _devices; }
            set { _devices = value;
                RaisePropertyChanged("Devices");
            }
        }
        #endregion

        private readonly IGroupService _groupService;
        public GroupViewVm()
        {
            _groupService = App.Container.GetRequiredService<IGroupService>();
        }

        public async void GetGroup(long id)
        {
            var result = await _groupService.GetGroupAsync(id);
            if (result == null)
            {
                Group = new GroupDto();
                return;
            }
            Group = result;
            var tempUsers = new ObservableCollection<GroupUserDto>();
            var tempDevices = new ObservableCollection<GroupSecurityDeviceDto>();
            result.GroupUsers.ForEach(x => tempUsers.Add(x));
            result.GroupSecurityDevices.ForEach(x => tempDevices.Add(x));
            Users = tempUsers;
            Devices = tempDevices;
        }
    }
}
