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
        Text CharacterName;

        [SerializeField]
        Image CharacterNameBg;

        [SerializeField]
        Text Lines;

        [SerializeField]
        GameObject Convo;

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
                Convo.SetActive(false);
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
                Debug.Log(string.Join(", ", CharacterRoster.AllId));
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
            Domain.Text txt = scene.Texts[textIndex];
            Domain.Character chr = CharacterRoster.Get(txt.Actor);
            Avatar.sprite = chr.Avatar;
            Avatar.color = Color.white;
            CharacterName.text = chr.Name;
            CharacterNameBg.color = chr.Color;
            Background.color = Color.white;
            StartCoroutine(_PlayLines(txt.Lines));
            Convo.SetActive(true);
        }

        int textIndex = 0;

        IEnumerator<WaitForSeconds> _PlayLines(Domain.TextLine[] lines)
        {
            Lines.text = "";
            for (int i=0; i<lines.Length; i++)
            {
                if (i > 0)
                {
                    Lines.text += "\n";
                }
                Domain.TextLine line = lines[i];
                if (line.Delay > 0f)
                {
                    yield return new WaitForSeconds(line.Delay);
                }
                Lines.text += line.Text;
            }
        }
    }
}
