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

        public Text(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("lines"))
            {
                lines = tmp["lines"];
                _lines = TextLine.LoadManyFromJSON(lines);
            } else
            {
                lines = "";
                _lines = new TextLine[0];
                Debug.LogWarning(string.Format("Empty conversation {0}", this));
            }
            if (tmp.ContainsKey("actor"))
            {
                actor = tmp["actor"];
            }
            else
            {
                Debug.LogError(string.Format("No one is saying: {1}", lines));
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
            return IO.JsonLoader.Indent(ret + "\n}");
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