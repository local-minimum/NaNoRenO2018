using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain
{
    public class TextLine
    {
        private float delay;
        public float Delay
        {
            get
            {
                return delay;
            }
        }

        private string text;
        public string Text
        {
            get
            {
                return text;
            }
        }

        public TextLine(Dictionary<string, string> tmp)
        {
            text = tmp["text"];
            if (tmp.ContainsKey("delay"))
            {
                delay = float.Parse(tmp["delay"]);
            }
            else
            {
                delay = 0f;
            }
        }

        public static TextLine LoadFromJSON(string json)
        {
            return new TextLine(JsonLoader.LoadObject(json));
        }

        public static TextLine[] LoadManyFromJSON(string json)
        {
            return JsonLoader.LoadArr(json).Select(e => TextLine.LoadFromJSON(e)).ToArray();
        }
    }
}