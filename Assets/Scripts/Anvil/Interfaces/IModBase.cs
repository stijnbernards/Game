using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anvil.Interfaces
{
    public interface IModBase
    {
        public void PreInit();
        public void Load();
        public void PostInit();
    }
}
