using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VizNov.Viz
{
    public class CharacterRoster : MonoBehaviour
    {
        Dictionary<string, Domain.Character> roster = new Dictionary<string, Domain.Character>();

        private void OnEnable()
        {
            Narrator.Instance.OnCharacter += HandleCharacterEvent;
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

        public Domain.Character Get(string id)
        {
            return roster[id];
        }
    }
}