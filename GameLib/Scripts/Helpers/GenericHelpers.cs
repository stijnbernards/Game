using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Linq;

public class GenericHelpers
{
    public static IEnumerable<Type> GetTypesWithAttribute(Type attribute) 
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        foreach (Type type in assembly.GetTypes())
        {
            if (type.GetCustomAttributes(attribute, true).Length > 0)
            {
                yield return type;
            }
        }
    }

    public static T GetAttribute<T>(Type t) where T : Attribute
    {
        T attribute = (T)Attribute.GetCustomAttribute(t, typeof(T));

        if (attribute != null)
        {
            return attribute as T;
        }
        else
        {
            return null;
        }
    }
}