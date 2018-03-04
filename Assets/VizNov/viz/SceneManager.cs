using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VizNov.Viz
{
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
                //Convo.SetActive(false);
                //NextButton.gameObject.SetActive(false);
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
                //NextButton.gameObject.SetActive(false);
                Scene.sprite = scene.Image;
                Scene.color = Color.white;
                textIndex = 0;
                ShowDialog();
            }
        }

        void ExitScene()
        {
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

        public void NextLines()
        {
            //NextButton.gameObject.SetActive(false);
            textIndex += 1;
            if (textIndex >= scene.Texts.Length)
            {
                //TODO Scene end
            } else
            {
                StartCoroutine(_PlayText(scene.Texts[textIndex]));
            }
        }

        int textIndex = 0;

        IEnumerator<WaitForSeconds> _PlayText(Domain.Text txt)
        {
            Convo.SetActive(false);
            NextButton.gameObject.SetActive(false);
            Domain.Character chr = CharacterRoster.Get(txt.Actor);
            Avatar.sprite = chr.Avatar;
            Avatar.color = Color.white;
            AvatarBackground.color = chr.Color;
            CharacterName.text = chr.Name;
            CharacterNameBg.color = chr.Color;
            // yield return new WaitForSeconds(txt.Delay)
            Convo.SetActive(true);

            Lines.text = "";
            for (int i=0; i<txt.Lines.Length; i++)
            {
                if (i > 0)
                {
                    Lines.text += "\n";
                }
                Domain.TextLine line = txt.Lines[i];
                if (line.Delay > 0f)
                {
                    yield return new WaitForSeconds(line.Delay);
                }
                Lines.text += line.Text;
            }
            NextButton.gameObject.SetActive(true);
        }
    }
}
