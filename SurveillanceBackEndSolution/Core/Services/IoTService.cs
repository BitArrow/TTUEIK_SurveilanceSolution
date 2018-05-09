using Core.DTO;
using Core.Services.Interfaces;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class IoTService : IIoTService
    {
        private readonly IConfiguration _configuration;
        private readonly RegistryManager _deviceManager;

        public IoTService(IConfiguration configuration)
        {
            _configuration = configuration;

            _deviceManager = RegistryManager.CreateFromConnectionString(_configuration["IoTHub:ServiceConnection"]);
        }

        public async Task<NewDeviceDto> RegisterNewDevice()
        {
            var guid = Guid.NewGuid();
            var registeredDevice = await _deviceManager.AddDeviceAsync(new Device(guid.ToString()));
            return new NewDeviceDto
            {
                DeviceId = registeredDevice.Id,
                DeviceKey = registeredDevice.Authentication.SymmetricKey.PrimaryKey
            };
        }

        public async Task DeleteDevice(string deviceId)
        {
            await _deviceManager.RemoveDeviceAsync(deviceId);
        }
    }
}
