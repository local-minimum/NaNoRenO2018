using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain
{
    public abstract class AbsractComponent
    {
        abstract public T FromDict<T>(Dictionary<string, string> tmp) where T : AbsractComponent;

        public static T LoadFromJSON<T>(string json) where T : AbsractComponent
        {
            return System.Activator.CreateInstance<T>().FromDict<T>(JsonLoader.LoadObject(json));
        }

        public static T[] LoadManyFromJSON<T>(string json) where T : AbsractComponent
        {
            return JsonLoader.LoadArr(json).Select(e => LoadFromJSON<T>(e)).ToArray();
        }
    }
}
