using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VizNov.Viz
{
    public enum EventType {Start, End};

    public delegate void CharacterEvent(Domain.Character character, EventType type);
    public delegate void SceneEvent(Domain.Scene scene, EventType type);
    public delegate void StoryEvent(Domain.Story story, EventType type);

    public class Narrator : MonoBehaviour
    {
        static Narrator _instance;
        public static Narrator Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.FindObjectOfType<Narrator>();
                }
                return _instance;
            }
        }

        public static void Continue()
        {
            Instance.Next();
        }

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(_instance.gameObject);
            }
            _instance = this;
        }

        public event CharacterEvent OnCharacter;
        public event SceneEvent OnScene;
        public event StoryEvent OnStory;

        Domain.Story story;
        int nextSceneIndex = -1;
        Domain.Scene scene;

        private void Start()
        {
            story = GetComponent<IO.JsonLoader>().GetStory();
            Debug.Log(string.Format("Loaded story:\n{0}", story.ToJSON()));
        }

        void EmitStoryCharacters(EventType type)
        {
            for (int i = 0, l=story.Characters.Length; i<l; i++)
            {
                if (OnCharacter != null)
                {
                    OnCharacter(story.Characters[i], type);
                }
            }
        }

        void EmitScene(EventType type)
        {
            if (OnScene != null)
            {
                OnScene(scene, type);
            }
        }

        void EmitStory(EventType type)
        {
            if (OnStory != null)
            {
                OnStory(story, type);
            }
        }
        
        void Next()
        {
            waiting = false;
        }

        bool waiting = false;

        private void Update()
        {
            if (waiting || story == null) {
                return;
            } else if (story.Scenes.Length <= nextSceneIndex) {
                if (scene != null && story.Scenes.Length == nextSceneIndex)
                {
                    EmitScene(EventType.End);
                    EmitStoryCharacters(EventType.End);
                    nextSceneIndex++;
                    EmitStory(EventType.End);
                }
                return;
            }

            if (nextSceneIndex == -1)
            {
                EmitStory(EventType.Start);
                EmitStoryCharacters(EventType.Start);
                nextSceneIndex = 0;
                waiting = false;
            } else {
                if (scene != null)
                {
                    EmitScene(EventType.End);
                }
                scene = story.Scenes[nextSceneIndex];
                nextSceneIndex++;
                EmitScene(EventType.Start);
                waiting = true;
            }
        }
    }
}
