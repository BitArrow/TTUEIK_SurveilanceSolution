using Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IIoTService
    {
        Task<NewDeviceDto> RegisterNewDevice();
        Task DeleteDevice(string deviceId);
    }
}
