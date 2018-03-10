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
            choicesContainer.SetActive(true);
            this.callback = callback;
            int choiceIndex;
            for(choiceIndex = 0; choiceIndex < choices.Length; choiceIndex++)
            {
                if (choiceIndex == this.choices.Count)
                {
                    this.choices.Add(GetNewChoiceButton(choiceIndex));
                }

                this.choices[choiceIndex].gameObject.SetActive(true);

            }
            while (choiceIndex < this.choices.Count)
            {
                this.choices[choiceIndex].gameObject.SetActive(false);
                choiceIndex++;
            }
        }

        public void Clicking(int index)
        {
            Debug.Log(index);
            //choicesContainer.SetActive(false);
            callback(index);
        }

        Button GetNewChoiceButton(int index)
        {
            GameObject GO = new GameObject();
            GO.transform.SetParent(choicesContainer.transform);
            LayoutElement le = GO.AddComponent<LayoutElement>();
            le.flexibleHeight = 1;
            Button btn = GO.AddComponent<Button>();
            Image img = GO.AddComponent<Image>();
            btn.image = img;
            img.color = Color.red;
            btn.onClick = new Button.ButtonClickedEvent();
            btn.onClick.AddListener(delegate() { Clicking(index); });
            return btn;
        }
    }
}