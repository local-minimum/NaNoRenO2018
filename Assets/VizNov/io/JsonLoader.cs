using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov
{
    public class JsonLoader : MonoBehaviour
    {

        [SerializeField]
        TextAsset file;

        void Start()
        {
            var s = Domain.Story.LoadFromJSON(file.text);
            Debug.Log(string.Join(", ", s.Characters.Select(c => c.Name).ToArray()));
        }
    }
}
