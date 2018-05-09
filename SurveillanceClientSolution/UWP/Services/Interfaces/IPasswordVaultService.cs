using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Services.Interfaces
{
    public interface IPasswordVaultService
    {
        void SaveTokens();
        void LoadExistingTokens();
        void RemoveExistingTokens();
    }
}
