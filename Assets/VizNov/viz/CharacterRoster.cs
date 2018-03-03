using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Viz
{
    public class CharacterRoster : MonoBehaviour
    {
        Dictionary<string, Domain.Character> roster = new Dictionary<string, Domain.Character>();

        Narrator narrator;
        SceneManager sceneManager;

        private void OnEnable()
        {
            narrator = Narrator.Instance;
            narrator.OnCharacter += HandleCharacterEvent;
            sceneManager = SceneManager.Instance;
            sceneManager.OnCharacter += HandleCharacterEvent;
        }

        private void OnDisable()
        {
            narrator.OnCharacter -= HandleCharacterEvent;
            sceneManager.OnCharacter -= HandleCharacterEvent;
        }

        private void OnDestroy()
        {
            OnDisable();
        }

        private void HandleCharacterEvent(Domain.Character character, EventType type)
        {
            switch (type)
            {
                case EventType.Start:
                    if (roster.ContainsKey(character.Id))
                    {
                        Debug.LogWarning(string.Format(
                            "Duplicated id '{0}' in roster for {1} and {2}",
                            character.Id, character.Name, roster[character.Id].Name
                        ));
                    }
                    roster[character.Id] = character;
                    break;
                case EventType.End:
                    if (!roster.ContainsKey(character.Id))
                    {
                        Debug.LogWarning(string.Format(
                            "Missing id '{0}' in roster.",
                            character.Id
                        ));
                    } else
                    {
                        roster.Remove(character.Id);
                    }
                    break;
            }   
        }

        bool _Has(string id)
        {
            return roster.ContainsKey(id);
        }
        public static bool Has(string id)
        {
            return Instance._Has(id);
        }

        Domain.Character _Get(string id)
        {
            return roster[id];
        }
        public static Domain.Character Get(string id)
        {
            return Instance._Get(id);
        }

        string[] _AllId
        {
            get
            {
                return roster.Keys.ToArray();
            }
        }

        public static string[] AllId
        {
            get
            {
                return Instance._AllId;
            }
        }

        static CharacterRoster _instance;
        static CharacterRoster Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<CharacterRoster>();
                }
                return _instance;
            }
        }

        private void Start()
        {
            if (_instance && _instance == this)
            {
                Destroy(this);
            } else if (!_instance)
            {
                _instance = this;
            }
        }
    }
}