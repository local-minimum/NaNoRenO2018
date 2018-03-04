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
        private TextLine[] _lines;
        public TextLine[] Lines
        {
            get
            {
                return _lines;
            }
        }

        private float delay;
        public float Delay
        {
            get
            {
                return delay;
            }
        }

        public Text(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("lines"))
            {
                lines = tmp["lines"];
                _lines = TextLine.LoadManyFromJSON(lines);
            }
            else
            {
                lines = "";
                _lines = new TextLine[0];
            }
            if (tmp.ContainsKey("actor"))
            {
                actor = tmp["actor"];
            }
            if (tmp.ContainsKey("delay"))
            {
                try
                {
                    delay = float.Parse(tmp["delay"]);
                }
                catch (System.FormatException)
                {
                    delay = -1;
                }
            }
            else
            {
                delay = 0f;
            }
        }

        public string ToJSON()
        {
            string sactor = string.IsNullOrEmpty(actor) ? "" : actor;
            string ret = "{\n" + string.Format("\"actor\": \"{0}\"", sactor);
            string slines = string.Join(",\n", Lines.Select(l => l.ToJSON()).ToArray());
            if (!string.IsNullOrEmpty(slines))
            {
                ret += string.Format(",\n\"lines\": [\n{0}\n]", slines);
            }
            if (delay > 0f)
            {
                ret += string.Format(",\n\"delay\": {0}", delay);
            }
            return IO.JsonLoader.Indent(ret + "\n}");
        }

        void LogIssues(string json)
        {
            if (_lines.Length == 0)
            {
                Debug.LogWarning(string.Format("Empty conversation/no lines: {0}", json));
            }
            if (string.IsNullOrEmpty(actor)) {
                Debug.LogError(string.Format("No one is saying: {0}", json));
            }
            if (delay < 0f)
            {
                Debug.LogError(string.Format("Failed to set delay from: {0}", json));
            }
        }

        public static Text LoadFromJSON(string json)
        {
            Text t = new Text(IO.JsonLoader.LoadObject(json));
            t.LogIssues(json);
            return t;
        }

        public static Text[] LoadManyFromJSON(string json)
        {
            return IO.JsonLoader.LoadArr(json).Select(e => Text.LoadFromJSON(e)).ToArray();
        }
    }
}