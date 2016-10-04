using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

using Anvil.Attributes;

namespace Anvil
{
    public class AnvilRegistry
    {
        private Dictionary<string, ModInfo> registeredMods = new Dictionary<string, ModInfo>();

        public void GetMods()
        {
            ModInfo info;

            foreach (Type type in GenericHelpers.GetTypesWithAttribute(typeof(Mod)))
            {
                info = new ModInfo();

                info.ModClass = Activator.CreateInstance(type);

                info.PreInit = type.GetMethod("PreInit");
                info.Load = type.GetMethod("Load");
                info.PostInit = type.GetMethod("PostInit");

                info.ModClassType = type;

                registeredMods.Add(GenericHelpers.GetAttribute<Mod>(type).ModID, info);
            }
        }

        public void ModsPreInit()
        {
            foreach (KeyValuePair<string, ModInfo> info in registeredMods)
            {
                if (info.Value.PreInit != null)
                {
                    Debug.Log(info.Key + " PreInit is being invoked!");
                    info.Value.PreInit.Invoke(info.Value.ModClass, new object[] { });
                }
            }
        }

        public void ModsLoad()
        {
            foreach (KeyValuePair<string, ModInfo> info in registeredMods)
            {
                if (info.Value.PreInit != null)
                {
                    Debug.Log(info.Key + " LoadInit is being invoked!");
                    info.Value.Load.Invoke(info.Value.ModClass, new object[] { });
                }
            }
        }

        public void ModsPostInit()
        {
            foreach (KeyValuePair<string, ModInfo> info in registeredMods)
            {
                if (info.Value.PreInit != null)
                {
                    Debug.Log(info.Key + " PostInit is being invoked!");
                    info.Value.PostInit.Invoke(info.Value.ModClass, new object[] { });
                }
            }
        }
    }
}