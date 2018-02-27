using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VizNov
{
    public class JsonLoader : MonoBehaviour
    {
        static readonly List<char> JSON_SYNTAX = new List<char>()
        {
            '[', ']',
            '{', '}',
            '"', '"',
        };

        static bool isSequenceControl(char c)
        {
            return JSON_SYNTAX.Select(e => e == c).Any();
        }

        static bool isGoingDeeper(char c)
        {
            int idx = JSON_SYNTAX.IndexOf(c);
            if (idx >= 0)
            {
                return idx % 2 == 0;
            }
            return false;
        }

        static bool inString(List<char> stack)
        {
            return stack.Count > 0 && stack[stack.Count - 1] == '"';
        }

        static bool isGoingShallower(char c, List<char> stack)
        {
            if (stack.Count < 1)
            {
                return false;
            }
            int idxDeeper = JSON_SYNTAX.IndexOf(stack[stack.Count - 1]);
            int idx = JSON_SYNTAX.IndexOf(c);
            return idx == idxDeeper + 1 && idxDeeper >= 0;
        }

        static string Indent(string json)
        {
            string[] lines = json.Split('\n');
            int indent = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
                char last = ' ';
                if (lines[i].Length > 0)
                {
                    last = lines[i][lines[i].Length - 1];
                    if (last == ',' && lines[i].Length > 1)
                    {
                        last = lines[i][lines[i].Length - 2];
                    }
                }
                else
                {
                    continue;
                }
                if (last == '[' || last == '{' || last == ':')
                {
                    lines[i] = new string(' ', indent) + lines[i];
                    indent += 2;
                }
                else if (last == ']' || last == '}')
                {
                    indent = Mathf.Max(0, indent - 2);
                    lines[i] = new string(' ', indent) + lines[i];
                }
                else
                {
                    lines[i] = new string(' ', indent) + lines[i];
                }
            }
            return string.Join("\n", lines);
        }

        public static IEnumerable<string> LoadArr(string json)
        {
            List<char> stack = new List<char>();
            bool escapeNext = false;
            int itemStart = 0;
            for (int idx = 0; idx < json.Length; idx++)
            {
                char cur = json[idx];
                if (escapeNext)
                {
                    continue;
                }
                else if (cur == '\\')
                {
                    escapeNext = true;
                    continue;
                }

                if (isSequenceControl(cur))
                {
                    if (inString(stack) && cur == '"' || isGoingShallower(cur, stack))
                    {
                        stack.RemoveAt(stack.Count - 1);
                        if (stack.Count == 1)
                        {
                            yield return Indent(json.Substring(itemStart, idx - itemStart + 1));
                        }
                    }
                    else if (isGoingDeeper(cur))
                    {
                        stack.Add(cur);
                        if (stack.Count == 2)
                        {
                            itemStart = idx;
                        }
                    }
                }
            }
        }

        public static Dictionary<string, string> LoadObject(string json)
        {
            Dictionary<string, string> obj = new Dictionary<string, string>();
            List<char> stack = new List<char>();
            bool escapeNext = false;
            int itemStart = 0;
            int keyStart = 0;
            string key = "";
            bool nextIsKey = false;

            for (int idx = 0; idx < json.Length; idx++)
            {
                char cur = json[idx];
                if (escapeNext)
                {
                    continue;
                }
                else if (cur == '\\')
                {
                    escapeNext = true;
                    continue;
                }

                if (isSequenceControl(cur))
                {
                    if (nextIsKey)
                    {
                        if (cur == '"')
                        {
                            if (stack.Count == 1)
                            {
                                keyStart = idx;
                                stack.Add(cur);
                                if (stack.Count == 1)
                                {
                                    nextIsKey = true;
                                }
                            }
                            else
                            {
                                key = json.Substring(keyStart + 1, idx - keyStart - 1);
                                stack.RemoveAt(stack.Count - 1);
                            }
                        }
                        else if (stack.Count == 1 && cur == ':')
                        {
                            nextIsKey = false;
                        }
                    }
                    else
                    {

                        if (inString(stack) && cur == '"' || isGoingShallower(cur, stack))
                        {
                            if (stack.Count == 2)
                            {
                                if (inString(stack))
                                {
                                    obj[key] = Indent(json.Substring(itemStart + 1, idx - itemStart - 1));
                                }
                                else
                                {
                                    obj[key] = Indent(json.Substring(itemStart, idx - itemStart + 1));
                                }
                                nextIsKey = true;
                            }
                            stack.RemoveAt(stack.Count - 1);
                        }
                        else if (isGoingDeeper(cur))
                        {
                            stack.Add(cur);
                            if (stack.Count == 2)
                            {
                                itemStart = idx;
                            }
                            else if (stack.Count == 1)
                            {
                                nextIsKey = true;
                            }
                        }
                    }
                }
            }
            return obj;
        }


        [SerializeField]
        TextAsset file;

        public Domain.Story GetStory()
        {
            return Domain.Story.LoadFromJSON(file.text);
        }
    }
}
