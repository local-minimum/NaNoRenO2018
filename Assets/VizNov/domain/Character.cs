using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VizNov.Domain {
    static class CharacterColors
    {
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
            } else if (colorLength != 6 || colorLength != 8)
            {
                throw new System.ArgumentException(
                    string.Format(
                        "Only supports format [#fff, #fffff, #ffff, #ffffffff], not {}", colorName)
                );
            }
            int[] arr = new int[4];
            int idx = 0;
            for (int pos=startAt; pos < colorName.Length; pos += step, idx+=1)
            {
                arr[idx] = int.Parse(
                    colorName.Substring(pos, step),
                    System.Globalization.NumberStyles.HexNumber
                ) * (step == 1 ? 16 : 1); 
            }
            if (idx == 2)
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
            return _colorMaps[FullID2Key(id)];
        }
    }

    [System.Serializable]
    public struct Character {

        bool _loaded;

        [SerializeField]
        private string _id;
        public string id
        {
            get
            {
                return _id;
            }
        }

        [SerializeField]
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
        }

        [SerializeField]
        private string _avatarName;

        private Sprite _avatar;
        public Sprite avatar
        {
            get
            {
                return _avatar;
            }
        }

        [SerializeField]
        private string _colorName;
        private Color32 _color;
        public Color32 color
        {
            get
            {
                return _color;
            }
        }

        public Character(string id, string name, string avatar)
        {
            _id = id;
            _name = name;
            _avatarName = avatar;
            _avatar = Resources.Load<Sprite>(avatar);
            _colorName = null;
            _color = CharacterColors.Get(id);
            _loaded = true;
        }

        public Character(string id, string name, string avatar, string color)
        {
            _id = id;
            _name = name;
            _avatarName = avatar;
            _avatar = Resources.Load<Sprite>(avatar);
            _colorName = color;
            _color = CharacterColors.Load(id, color);
            _loaded = true;
        }

        public Character(Character template)
        {
            _id = template._id;
            _name = template._name;
            _avatarName = template._avatarName;
            _avatar = Resources.Load<Sprite>(_avatarName);
            _colorName = template._colorName;
            _color = CharacterColors.Load(_id, template._colorName);
            _loaded = true;
        }

        public static Character LoadFromJSON(string json)
        {
            return new Character(JsonUtility.FromJson<Character>(json));
        }

        
    }
}