using System;

namespace Anvil.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Mod : Attribute
    {
        public string ModID;

        public Mod(string modID)
        {
            ModID = modID;
        }
    }
}