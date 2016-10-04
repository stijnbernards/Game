using System;
using System.Reflection;

namespace Anvil
{
    public class ModInfo
    {
        public object ModClass = null;

        public Type ModClassType = null;

        public MethodInfo PreInit = null;
        public MethodInfo Load = null;
        public MethodInfo PostInit = null;
    }
}