using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain
{
    [System.Serializable]
    public class Scene
    {
        [SerializeField]
        string id;
        public string Id
        {
            get
            {
                return id;
            }
        }

        [SerializeField]
        string characters;
        private Character[] _characters;
        public Character[] Characters
        {
            get
            {
                return _characters;
            }
        }

        [SerializeField]
        string image;
        Sprite _image;
        public Sprite Image
        {
            get
            {
                return _image;
            }
        }

        string texts;
        Text[] _texts;
        public Text[] Texts
        {
            get
            {
                return _texts;
            }
        }

        public Scene(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("id"))
            {
                id = tmp["id"];
            }
            if (tmp.ContainsKey("characters"))
            {
                characters = tmp["characters"];
                _characters = Character.LoadManyFromJSON(characters);
            } else
            {
                characters = "";
                _characters = new Character[0];
            }
            if (tmp.ContainsKey("image"))
            {
                image = tmp["image"];
                _image = Resources.Load<Sprite>(image);
            } else
            {
                image = "";
                _image = null;
            }
            if (tmp.ContainsKey("texts"))
            {
                texts = tmp["texts"];
                _texts = Text.LoadManyFromJSON(texts);
            } else
            {
                texts = "";
                _texts = new Text[0];
            }
        }

        void LogIssues(string json)
        {
            bool issues = false;
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("Scene got no id.");
                issues = true;
            }
            if (issues)
            {
                Debug.Log(string.Format("^Above issues from: {0}", json));
            }
        }

        public string ToJSON()
        {
            string sid = string.IsNullOrEmpty(id) ? "" : id;
            string ret = "{\n" + string.Format("\"id\": \"{0}\",\n\"name\": \"{1}\"", sid, image);
            string characters = string.Join(",\n", Characters.Select(c => c.ToJSON()).ToArray());
            if (!string.IsNullOrEmpty(characters))
            {
                ret += string.Format(",\n\"characters\": [\n{0}\n]", characters);
            }
            string stexts = string.Join(",\n", Texts.Select(t => t.ToJSON()).ToArray());
            if (!string.IsNullOrEmpty(characters))
            {
                ret += string.Format(",\n\"texts\": [\n{0}\n]", stexts);
            }

            return IO.JsonLoader.Indent(ret + "\n}");
        }

        public static Scene LoadFromJSON(string json)
        {
            Scene s = new Scene(IO.JsonLoader.LoadObject(json));
            s.LogIssues(json);
            return s;
        }

        public static Scene[] LoadManyFromJSON(string json)
        {
            return IO.JsonLoader.LoadArr(json).Select(e => LoadFromJSON(e)).ToArray();
        }
    }
}