using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain
{
    public class Text
    {
        private string actor;
        public string Actor
        {
            get
            {
                return actor;
            }
        }

        private string lines;
        
        public Text(Dictionary<string, string> tmp)
        {
            actor = tmp["actor"];
            if (tmp.ContainsKey("lines"))
            {
                lines = tmp["lines"];
            } else
            {
                lines = "";
            }
        }

        public static Text LoadFromJSON(string json)
        {
            return new Text(IO.JsonLoader.LoadObject(json));
        }

        public static Text[] LoadManyFromJSON(string json)
        {
            return IO.JsonLoader.LoadArr(json).Select(e => Text.LoadFromJSON(e)).ToArray();
        }
    }
}