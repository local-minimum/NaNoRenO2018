using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain
{
    [System.Serializable]
    public struct Scene
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
        Sprite Image
        {
            get
            {
                return _image;
            }
        }

        string texts;
        Text[] _texts;
        Text[] Texts
        {
            get
            {
                return _texts;
            }
        }

        public Scene(Dictionary<string, string> tmp)
        {
            id = tmp["id"];
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
            return new Scene(JsonLoader.LoadObject(json));
        }

        public static Scene[] LoadManyFromJSON(string json)
        {
            return JsonLoader.LoadArr(json).Select(e => LoadFromJSON(e)).ToArray();
        }
    }
}