using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov.Domain
{
    public enum EffectType
    {
        Null,
        Animation,
        Sound,
        Story,
    };

    public enum EffectTime {
        Null,
        OnEnter,
        OnExit,
    };

    public class Effect
    {
        static EffectType ParseEffectType(string effectType)
        {
            switch (effectType.ToLower())
            {
                case "animation":
                    return EffectType.Animation;
                case "sound":
                    return EffectType.Sound;
                case "story":
                    return EffectType.Story;
                default:
                    throw new System.ArgumentException(string.Format("{0} is not a known effect type", effectType));
            }
        }
        private EffectType _effectType;
        private string __effectType;
        public EffectType effectType {
            get {
                return _effectType;
            }
        }

        static EffectTime ParseEffecTime(string effectTime)
        {
            switch (effectTime.ToLower())
            {
                case "onenter":
                    return EffectTime.OnEnter;
                case "onexit":
                    return EffectTime.OnExit;
                default:
                    throw new System.ArgumentException(string.Format("{0} is not a know effect time", effectTime));
            }
        }
        private EffectTime _effectTime;
        private string __effectTime;
        public EffectTime effectTime
        {
            get
            {
                return _effectTime;
            }
        }

        private string reciever;
        public string Reciever
        {
            get
            {
                return reciever;
            }
        }

        private float delay;
        public float Delay
        {
            get
            {
                return delay;
            }
        }

        protected string ToInnerJSON()
        {
            string ret = string.Join(
                ",\n  ",
                new string[]
                {
                    string.Format("\"effectType\": \"{0}\"", __effectType),
                    string.Format("\"effectTime\": \"{0}\"", __effectTime),
                }
            );
            if (!string.IsNullOrEmpty(reciever))
            {
                ret += string.Format(",\n  \"reciever\": \"{0}\"", reciever);
            }
            if (delay > 0)
            {
                ret += string.Format(",\n  \"delay\": \"{0}\"", delay);
            }
            return ret;
        }

        public virtual string ToJSON()
        {
            switch (effectType) {
                case EffectType.Animation:
                    return ((EffectAnimation)this).ToJSON();
                case EffectType.Sound:
                    return ((EffectSound)this).ToJSON();
                case EffectType.Story:
                    return ((EffectStory)this).ToJSON();
                default:
                    return "{\n" + ToInnerJSON() + "\n}";
            }
        }

        protected void SetBaseState(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("effectType"))
            {
                _effectType = ParseEffectType(tmp["effectType"]);
                __effectType = tmp["effectType"];
            }
            if (tmp.ContainsKey("effectTime"))
            {
                _effectTime = ParseEffecTime("effectTime");
                __effectTime = tmp["effectTime"];
            }
            if (tmp.ContainsKey("reciever"))
            {
                reciever = tmp["reciever"];
            }
            if (tmp.ContainsKey("delay"))
            {
                try
                {
                    delay = float.Parse(tmp["delay"]);
                }
                catch (System.FormatException)
                {
                    delay = -1;
                }
            }
        }

        protected void LogIssues(string json)
        {
            if (effectType == EffectType.Null)
            {
                Debug.LogError(string.Format("Failed to set effectType: {0}", json));
            }
            if (effectTime == EffectTime.Null)
            {
                Debug.LogError(string.Format("Failed to set effectTime: {0}", json));
            }
            if (delay < 0f)
            {
                Debug.LogError(string.Format("Failed to set delay from: {0}", json));
            }
        }

        public static Effect FromJSON(string json)
        {
            Dictionary<string, string> d = IO.JsonLoader.LoadObject(json);
            return FromJSON(d);
        }

        public static Effect FromJSON(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("effectType") | tmp.ContainsKey("EffectType"))
            {
                string effectType = tmp.ContainsKey("effectType") ? tmp["effectType"] : tmp["EffectType"];
                switch (ParseEffectType(effectType))
                {
                    case EffectType.Animation:
                        return new EffectAnimation(tmp);
                    case EffectType.Sound:
                        return new EffectSound(tmp);
                    case EffectType.Story:
                        return new EffectStory(tmp);
                    default:
                        Debug.LogError(string.Format("EffectType {0} not implemented", effectType));
                        return null;
                }
            } else
            {
                Debug.LogError("Effect doesn't have an effectType");
                return null;
            }
        }

        public static Effect[] LoadManyFromJSON(string json)
        {
            return IO.JsonLoader.LoadArr(json).Select(e => FromJSON(e)).ToArray();
        }
    }

    public enum AnimationEffectType { None, Trigger, Bool, Float };

    public class EffectAnimation : Effect {

        private AnimationEffectType type;
        public AnimationEffectType animationEffectType
        {
            get
            {
                return type;
            }
        }

        private string trigger;
        public string Trigger
        {
            get
            {
                return trigger;
            }
        }

        private bool boolean;
        public bool BooleanValue
        {
            get
            {
                return boolean;
            }
        }

        private float _float;
        public float FloatValue
        {
            get
            {
                return _float;
            }
        }

        public EffectAnimation(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("trigger"))
            {
                trigger = tmp["trigger"];
                type = AnimationEffectType.Trigger;
            } else if (tmp.ContainsKey("bool"))
            {
                try
                {
                    boolean = bool.Parse(tmp["bool"]);
                } catch (System.FormatException)
                {
                    Debug.LogError(string.Format("Animation Effect got malformed bool: {0}", tmp["bool"]));
                } finally {
                    type = AnimationEffectType.Bool;
                }
            } else if (tmp.ContainsKey("float"))
            {
                try
                {
                    _float = float.Parse(tmp["float"]);
                }
                catch (System.FormatException)
                {
                    Debug.LogError(string.Format("Animation Effect could not parse float: {0}", tmp["float"]));
                }
                finally
                {
                    type = AnimationEffectType.Float;
                }
            } else
            {
                Debug.LogError("Animation Effect doesn't have any effect (trigger/bool/float)");
            }
        }

        public override string ToJSON()
        {
            string ret = "{\n" + ToInnerJSON();
            switch (animationEffectType)
            {
                case AnimationEffectType.Bool:
                    ret += string.Format(",\n  \"bool\": \"{0}\"", BooleanValue);
                    break;
                case AnimationEffectType.Trigger:
                    ret += string.Format(",\n  \"trigger\": \"{0}\"", Trigger);
                    break;
                case AnimationEffectType.Float:
                    ret += string.Format(",\n  \"float\": \"{0}\"", FloatValue);
                    break;
            }
            return IO.JsonLoader.Indent(ret + ",\n}");
        }
    }

    public enum SoundEffectType { None, Play, PlayOneShot };

    public class EffectSound : Effect
    {
        static SoundEffectType GetSoundEffectType(string value)
        {
            switch(value.ToLower())
            {
                case "play":
                    return SoundEffectType.Play;
                case "playoneshot":
                    return SoundEffectType.PlayOneShot;
                default:
                    Debug.LogError(string.Format("Sound Effect could not parse soundEffectType: {0}", value));
                    return SoundEffectType.None;
            }
        }

        private string _soundEffectType;
        private SoundEffectType soundEffectType;
        public SoundEffectType SoundEffectType
        {
            get
            {
                return soundEffectType;
            }
        }

        private string __audioSource;
        private AudioSource _audioSource;
        public AudioSource audioSource
        {
            get
            {
                return _audioSource;
            }
        }

        public EffectSound(Dictionary<string, string> tmp)
        {
            if (tmp.ContainsKey("soundEffectType"))
            {
                _soundEffectType = tmp["soundEffectType"];
                soundEffectType = GetSoundEffectType(_soundEffectType);
            } else
            {
                Debug.LogError("Sound Effect didn't get a soundEffectType");
            }

            //TODO: continue with audiosource from resources
        }

        public override string ToJSON()
        {
            return ToInnerJSON();
        }
    }

    public class EffectStory : Effect
    {
        public EffectStory(Dictionary<string, string> tmp)
        {

        }
    }
}