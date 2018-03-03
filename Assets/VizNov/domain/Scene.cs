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
            } else
            {
                Debug.LogError(string.Format("Scene {0} got no id.", this));
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

        public static Scene LoadFromJSON(string json)
        {
            return new Scene(IO.JsonLoader.LoadObject(json));
        }

        public static Scene[] LoadManyFromJSON(string json)
        {
            return IO.JsonLoader.LoadArr(json).Select(e => LoadFromJSON(e)).ToArray();
        }
    }
}