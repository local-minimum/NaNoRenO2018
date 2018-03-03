using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain {
    static class CharacterColors
    {
        static Color32 defaultColor = new Color32(0, 0, 0, 255);
        static Dictionary<string, Color32> _colorMaps = new Dictionary<string, Color32>();

        static Color32 String2Color(string colorName)
        {
            int startAt = 0;
            if (colorName[startAt] == '#')
            {
                startAt = 1;
            }
            int colorLength = colorName.Length - startAt;
            int step = 2;
            if (colorLength == 3 || colorLength == 4)
            {
                step = 1;
            } else if (!(colorLength == 3 * step || colorLength == 4 * step))
            {
                Debug.Log(colorLength);
                throw new System.ArgumentException(
                    string.Format(
                        "Only supports format [#fff, #fffff, #ffff, #ffffffff], not {0}", colorName)
                );
            }
            int[] arr = new int[4];
            for (int pos=startAt, idx=0; pos < colorName.Length; pos += step, idx+=1)
            {
                arr[idx] = int.Parse(
                    colorName.Substring(pos, step),
                    System.Globalization.NumberStyles.HexNumber
                ) * (step == 1 ? 16 : 1); 
            }
            if (colorLength != 4 * step)
            {
                arr[3] = 255;
            }
            return new Color32((byte)arr[0], (byte)arr[1], (byte)arr[2], (byte)arr[3]);
        }

        static string FullID2Key(string fullID)
        {
            return fullID.Split(':')[0];
        }

        public static Color32 Load(string id, string colorName)
        {
            if (string.IsNullOrEmpty(colorName))
            {
                return Get(id);
            }
            Color32 color = String2Color(colorName);
            _colorMaps[FullID2Key(id)] = color;
            return color;
        }

        public static Color32 Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = ":default";
            } else
            {
                id = FullID2Key(id);
                if (!_colorMaps.ContainsKey(id))
                {
                    Debug.LogWarning(string.Format("No color known for '{0}', using default", id));
                    id = ":default";
                }
            }
            if (id == ":default")
            {
                return defaultColor;
            }
            return _colorMaps[id];
        }
    }

    [System.Serializable]
    public class Character {

        [SerializeField]
        private string id;
        public string Id
        {
            get
            {
                return id;
            }
        }

        [SerializeField]
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        [SerializeField]
        private string avatar;

        private Sprite _avatarSprite;
        public Sprite Avatar
        {
            get
            {
                return _avatarSprite;
            }
        }

        [SerializeField]
        private string color;
        private Color32 _color;
        public Color32 Color
        {
            get
            {
                return _color;
            }
        }
    
        public Character(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("name"))
            {
                name = tmp["name"];
            }
            if (tmp.ContainsKey("avatar"))
            {
                avatar = tmp["avatar"];
            }
            if (tmp.ContainsKey("id"))
            {
                id = tmp["id"];
            }
            _avatarSprite = Resources.Load<Sprite>(avatar);
            color = tmp.ContainsKey("color") ? tmp["color"] : "";
            _color = CharacterColors.Load(id, color);            
        }

        void LogIssues(string json)
        {
            bool issues = false;
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("Character got no name");
                issues = true;
            }
            if (string.IsNullOrEmpty(avatar))
            {
                Debug.Log("Character got no avatar");
            }

            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError("Character got no id");
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
            string sname = string.IsNullOrEmpty(name) ? "" : name;
            string ret = "{\n" + string.Format("\"id\": \"{0}\",\n\"name\": \"{1}\"", sid, sname);
            if (!string.IsNullOrEmpty(avatar))
            {
                ret += string.Format(",\n\"avatar\": \"{0}\"", avatar);
            }
            if (!string.IsNullOrEmpty(color))
            {
                ret += string.Format(",\n\"color\": \"{0}\"", color);
            }
            return IO.JsonLoader.Indent(ret + "\n}");
        }

        public static Character LoadFromJSON(string json)
        {
            Character c = new Character(IO.JsonLoader.LoadObject(json));
            c.LogIssues(json);
            return c;
        }

        public static Character[] LoadManyFromJSON(string characters)
        {
            return IO.JsonLoader.LoadArr(characters).Select(e => LoadFromJSON(e)).ToArray();
        }

    }
}