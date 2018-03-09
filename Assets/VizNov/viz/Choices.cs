using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VizNov.Viz
{
    public class Choices : MonoBehaviour
    {

        List<Button> choices = new List<Button>();

        [SerializeField]
        GameObject choicesContainer;

        SceneManager sceneManager;

        private void OnEnable()
        {
            sceneManager = SceneManager.Instance;
            sceneManager.OnChoices += HandleChoices;
        }

        private void OnDisable()
        {
            sceneManager.OnChoices -= HandleChoices;
        }

        private void OnDestroy()
        {
            sceneManager.OnChoices -= HandleChoices;
        }

        System.Action<int> callback;
        private void HandleChoices(Domain.ChoiceOption[] choices, System.Action<int> callback)
        {
            this.callback = callback;
            int choiceIndex;
            for(choiceIndex = 0; choiceIndex < choices.Length; choiceIndex++)
            {
                if (choiceIndex == this.choices.Count)
                {
                    this.choices.Add(GetNewChoiceButton());
                }

                this.choices[choiceIndex].gameObject.SetActive(false);

            }
            while (choiceIndex < this.choices.Count)
            {
                this.choices[choiceIndex].gameObject.SetActive(false);
                choiceIndex++;
            }
            throw new System.NotImplementedException();
        }

        Button GetNewChoiceButton()
        {
            return choicesContainer.AddComponent<Button>();
        }
    }
}