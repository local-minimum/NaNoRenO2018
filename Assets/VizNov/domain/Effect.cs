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

        public string ToJSON()
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

    public class EffectAnimation : Effect {
        public EffectAnimation(Dictionary<string, string> tmp)
        {

        }
    }

    public class EffectSound : Effect
    {
        private string trigger;

        public EffectSound(Dictionary<string, string> tmp)
        {            
        }
    }

    public class EffectStory : Effect
    {
        public EffectStory(Dictionary<string, string> tmp)
        {

        }
    }
}