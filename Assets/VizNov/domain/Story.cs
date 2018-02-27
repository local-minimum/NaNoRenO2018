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
            }
        }

        public static Story LoadFromJSON(string json)
        {
            return new Story(JsonLoader.LoadObject(json));
        }
    }
}
