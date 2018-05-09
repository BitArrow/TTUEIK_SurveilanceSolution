using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Services.Interfaces
{
    public interface IHomeService
    {
        Task SendTest();
        Task StartServer();
        Task StopServer();
    }
}
