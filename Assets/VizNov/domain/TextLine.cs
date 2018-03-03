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
            if(tmp.ContainsKey("text"))
            {
                text = tmp["text"];
            }
            
            if (tmp.ContainsKey("delay"))
            {
                //Debug.Log(string.Format("delay: '{0}'", tmp["delay"]));
                try
                {
                    delay = float.Parse(tmp["delay"]);
                } catch (System.FormatException)
                {
                    Debug.LogError(string.Format("Trying to set delay to: '{0}'", tmp["delay"]));
                }
            }
            else
            {
                delay = 0f;
            }
        }
        void LogIssues(string json)
        {
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError(string.Format("No text in text-line: {0}", json));
            }
        }

        public static TextLine LoadFromJSON(string json)
        {
            TextLine tl = new TextLine(IO.JsonLoader.LoadObject(json));
            tl.LogIssues(json);
            return tl;
        }

        public static TextLine[] LoadManyFromJSON(string json)
        {
            return IO.JsonLoader.LoadArr(json).Select(e => TextLine.LoadFromJSON(e)).ToArray();
        }
    }
}