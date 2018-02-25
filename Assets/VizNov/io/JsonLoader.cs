using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VizNov
{
    public class JsonLoader : MonoBehaviour
    {

        [SerializeField]
        TextAsset file;

        void Start()
        {
            //Domain.Character someone = JsonUtility.FromJson<Domain.Character>(file.text);
            Debug.Log(JsonUtility.ToJson(new Domain.Character("test", "Hello", null, "#a0f")));
            var c = Domain.Character.LoadFromJSON("{\"_id\":\"test\",\"_name\":\"Hello\",\"_avatarName\":\"\",\"_colorName\":\"#a0f\"}");
            Debug.Log(c.color.ToString());
        }
    }
}
