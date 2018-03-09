using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VizNov.Viz
{
    public delegate void EffectEvent(Domain.Effect effect);
    public delegate void ChoicesEvent(Domain.ChoiceOption[] choices, System.Action<int> callback);

    public class SceneManager : MonoBehaviour
    {
        static SceneManager _instance;
        public static SceneManager Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<SceneManager>();
                }
                return _instance;
            }
        }

        public event CharacterEvent OnCharacter;
        public event EffectEvent OnEffect;
        public event ChoicesEvent OnChoices;

        [SerializeField]
        Image Scene;

        [SerializeField]
        Image Background;

        [SerializeField]
        Image Avatar;

        [SerializeField]
        Image AvatarBackground;

        [SerializeField]
        Text CharacterName;

        [SerializeField]
        Image CharacterNameBg;

        [SerializeField]
        Text Lines;

        [SerializeField]
        GameObject Convo;

        [SerializeField]
        Button NextButton;

        Domain.Scene scene;

        private void Start()
        {
            if (_instance && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
        }

        Narrator narrator;

        private void OnEnable()
        {
            narrator = Narrator.Instance;
            narrator.OnScene += HandleSceneChange;
        }

        private void OnDisable()
        {
            narrator.OnScene -= HandleSceneChange;
        }

        private void OnDestroy()
        {
            OnDisable();
        }

        private void HandleSceneChange(Domain.Scene scene, EventType type)
        {
            if (this.scene != null)
            {
                ExitScene();
            }
            this.scene = scene;
            EmitCharacters(type);
            if (type == EventType.Start)
            {
                Scene.sprite = scene.Image;
                Scene.color = Color.white;
                textIndex = 0;
                ShowDialog();
            }
        }

        void ExitScene()
        {
            txt = null;
            EmitCharacters(EventType.End);            
        }

        void EmitCharacters(EventType type)
        {
            if (OnCharacter == null)
            {
                return;
            }
            for (int i=0, l=scene.Characters.Length; i<l; i++)
            {
                OnCharacter(scene.Characters[i], type);
            }
        }


        void ShowDialog()
        {
            if (scene.Texts.Length <= textIndex)
            {
                return;
            }
            Domain.Text txt = scene.Texts[textIndex];
            StartCoroutine(_PlayText(txt));
        }

        public void NextText()
        {
            textIndex += 1;
            if (textIndex >= scene.Texts.Length)
            {
                ExitScene();
            } else
            {
                StartCoroutine(_PlayText(scene.Texts[textIndex]));
            }
        }

        int textIndex = 0;

        Domain.Text txt;

        IEnumerator<WaitForSeconds> _PlayText(Domain.Text txt)
        {
            this.txt = txt;
            Convo.SetActive(false);
            NextButton.gameObject.SetActive(false);
            Domain.Character chr = CharacterRoster.Get(txt.Actor);
            if (chr.Avatar)
            {
                Avatar.sprite = chr.Avatar;
                Avatar.color = Color.white;
                AvatarBackground.color = chr.Color;
                AvatarBackground.gameObject.SetActive(true);
                Avatar.gameObject.SetActive(true);
            }
            else {
                Avatar.gameObject.SetActive(false);
                AvatarBackground.gameObject.SetActive(false);
            }
            CharacterName.text = chr.Name;
            CharacterNameBg.color = chr.Color;
            yield return new WaitForSeconds(txt.Delay);
            Convo.SetActive(true);

            Lines.text = "";
            for (int lineIndex=0; lineIndex<txt.Lines.Length; lineIndex++)
            {
                if (lineIndex > 0)
                {
                    Lines.text += "\n";
                }
                Domain.TextLine line = txt.Lines[lineIndex];
                if (line.Delay > 0f)
                {
                    yield return new WaitForSeconds(line.Delay);
                }
                Lines.text += line.Text;
            }

            if (txt.Choices.Length > 0)
            {
                if (OnChoices != null)
                {
                    OnChoices(txt.Choices, EmitChoice);
                } else
                {
                    Debug.LogError(string.Format("SceneManager has no-one to take care of choices, story halts at: {0}", txt.ToJSON()));
                }
            }
            else
            {
                NextButton.gameObject.SetActive(true);
            }
        }

        void EmitChoice(int index)
        {
            if (OnEffect != null) {
                Domain.ChoiceOption choice = txt.Choices[index];
                for (int i = 0; i < choice.Effects.Length; i++)
                {
                    OnEffect(choice.Effects[i]);
                }
            }
        }
    }
}
