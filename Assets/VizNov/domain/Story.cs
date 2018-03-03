using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain
{
    [System.Serializable]
    public class Story
    {
        [SerializeField]
        string characters;
        Character[] _characters;
        public Character[] Characters
        {
            get
            {
                return _characters;
            }
        }

        [SerializeField]
        string scenes;
        Scene[] _scenes;
        public Scene[] Scenes
        {
            get
            {
                return _scenes;
            }
        }

        public Story(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("characters"))
            {
                characters = tmp["characters"];
                _characters = Character.LoadManyFromJSON(characters);
            } else
            {
                characters = "";
                _characters = new Character[0];
            }
            if (tmp.ContainsKey("scenes"))
            {
                scenes = tmp["scenes"];
                _scenes = Scene.LoadManyFromJSON(scenes);
            } else
            {
                scenes = "";
                _scenes = new Scene[0];
                Debug.Log("No scene");
                foreach (string k in tmp.Keys)
                {
                    Debug.Log(string.Format("{0} => {1}", k, tmp[k]));
                }
            }
            string[] acceptedKeys = new string[] { "scenes", "characters" };
            string[] extras = tmp.Keys.Where(k => !acceptedKeys.Contains(k)).ToArray();
            if (extras.Length > 0)
            {
                Debug.LogWarning(string.Format("{0} got extra keys: {1}", this, string.Join(", ", extras)));
            }
        }

        public static Story LoadFromJSON(string json)
        {
            Debug.Log(json);
            return new Story(IO.JsonLoader.LoadObject(json));
        }

        public string ToJSON()
        {
            string ret = "{";
            bool isListing = false;
            string characters = string.Join(",\n", Characters.Select(c => c.ToJSON()).ToArray());
            if (!string.IsNullOrEmpty(characters))
            {
                ret += string.Format("\n\"characters\": [\n{0}\n]", characters);
                isListing = true;
            }
            string scenes = string.Join(",\n", Scenes.Select(s => s.ToJSON()).ToArray());
            if (!string.IsNullOrEmpty(scenes))
            {
                if (isListing)
                {
                    ret += ",\n";
                } else
                {
                    ret += "\n";
                }
                ret += string.Format("\"scenes\": [\n{0}\n]", scenes);
                isListing = true;
            }
            if (isListing)
            {
                return IO.JsonLoader.Indent(ret + "\n}");
            } else
            {
                return ret + "}";
            }
            
        }
    }
}
