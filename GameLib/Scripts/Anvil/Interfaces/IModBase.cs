using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anvil.Interfaces
{
    public interface IModBase
    {
        void PreInit();
        void Load();
        void PostInit();
    }
}
