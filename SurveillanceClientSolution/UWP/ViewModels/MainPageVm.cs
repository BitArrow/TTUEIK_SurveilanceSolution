using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.DTO;
using UWP.Services.Interfaces;
using Windows.Networking.Connectivity;

namespace UWP.ViewModels
{
    public class MainPageVm : BaseVm
    {
        private readonly IGroupService _groupService;
        private readonly IHomeService _homeService;
        private ObservableCollection<GroupDto> groupList;
        private ObservableCollection<GroupDto> tempList;

        public ObservableCollection<GroupDto> GroupList
        {
            get { return groupList; }
            set { groupList = value;
                RaisePropertyChanged("GroupList");
            }
        }

        public string IPAddress { get; set; }


        public MainPageVm()
        {
            _groupService = App.Container.GetRequiredService<IGroupService>();
            _homeService = App.Container.GetRequiredService<IHomeService>();

            groupList = new ObservableCollection<GroupDto>();

            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter != null)
            {
                var hostname =
                    NetworkInformation.GetHostNames()
                        .SingleOrDefault(
                            hn =>
                                hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                == icp.NetworkAdapter.NetworkAdapterId);
                IPAddress = hostname?.CanonicalName;
            }
        }

        public async void LoadGroups()
        {
            tempList = new ObservableCollection<GroupDto>();
            var result = await _groupService.GetListAsync();
            if (result == null)
            {
                if (!groupList.Any()) // Do not empty, if couldn't get data
                    GroupList = new ObservableCollection<GroupDto>();
                return;
            }
            result.ForEach(x => tempList.Add(x));
            GroupList = tempList;
        }

        public async void SendTestMessage()
        {
            await _homeService.SendTest();
        }
        public async void StartServer()
        {
            await _homeService.StartServer();
        }
        public async void StopServer()
        {
            await _homeService.StopServer();
        }
    }
}
