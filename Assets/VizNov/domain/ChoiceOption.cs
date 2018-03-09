using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain
{
    public class ChoiceOption
    {
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
        }

        private Effect[] effects;
        public Effect[] Effects
        {
            get
            {
                return effects;
            }
        }

        private void LogIssues(string json)
        {
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning(string.Format("Choice Option has no text: {0}", json));
            }
            if (effects.Length == 0)
            {
                Debug.Log(string.Format("Choice Option has no effect: {0}", json));
            }
        }

        public ChoiceOption(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("text"))
            {
                text = tmp["text"];
            }
            if (tmp.ContainsKey("effects"))
            {
                effects = Effect.LoadManyFromJSON(tmp["effects"]);
            } else
            {
                effects = new Effect[0];
            }
        }

        public string ToJSON()
        {
            string ret = "{\n";
            bool needComma = false;
            if (!string.IsNullOrEmpty(text))
            {
                ret += string.Format("  \"text\": \"{0}\"", text);
                needComma = true;
            }
            if (effects.Length > 0)
            {
                if (needComma)
                {
                    ret += ",";
                }
                string effects = string.Join(",\n", Effects.Select(e => e.ToJSON()).ToArray());
                ret += string.Format("\n\"effects\": [\n{0}\n]", effects);
            }
            return IO.JsonLoader.Indent(ret + "\n}");
        }

        public static ChoiceOption LoadFromJSON(string json)
        {
            ChoiceOption co = new ChoiceOption(IO.JsonLoader.LoadObject(json));
            co.LogIssues(json);
            return co;
        }

        public static ChoiceOption[] LoadManyFromJSON(string json)
        {
            return IO.JsonLoader.LoadArr(json).Select(e => ChoiceOption.LoadFromJSON(e)).ToArray();
        }
    }
}