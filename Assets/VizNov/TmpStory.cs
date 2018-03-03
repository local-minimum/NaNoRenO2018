using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpStory : MonoBehaviour {

    public string Text
    {
        get
        {
            return text;
        }
    }
    public string text = @"{
  ""characters"": [
    {
      ""id"": ""ai:unknown"",
      ""name"": ""???"",
      ""color"": ""#01ffa4"",
      ""image"": ""sprites/characters/ai""
    },
    {
      ""id"": ""ai"",
      ""name"": ""Vacuum C212"",
      ""image"": ""sprites/characters/ai""
    }
  ],
  ""scenes"": [
    {
      ""id"": ""hallway 12/2"",
      ""characters"": [
        {
          ""id"": ""plant in pot"",
          ""name"": ""Plant in a pot"",
          ""image"": ""sprites/characters/potPlant""
        }
      ],
      ""image"": ""sprites/scenes/hallway"",
      ""texts"": [
        {
          ""actor"": ""ai:unknown"",
          ""lines"": [
            {
              ""text"": ""You look a bit sad today.""
            }
          ]
        },
        {
          ""actor"": ""plant in pot"",
          ""lines"": [
            {
              ""text"": ""...""
            }
          ]
        },
        {
          ""actor"": ""ai:unknown"",
          ""lines"": [
            {
              ""text"": ""Maybe you’re not getting enough light?""
            },
            {
              ""delay"": 0.5,
              ""text"": ""I’m Vacuum C212 by the way.""
            }
          ]
        },
        {
          ""actor"": ""plant in pot"",
          ""lines"": [
            {
              ""text"": ""...""
            }
          ]
        },
        {
          ""actor"": ""ai"",
          ""lines"": [
            {
              ""text"": ""Why aren’t you smart enough to talk? Or are you smart enough to know not to talk?""
            }
          ]
        },
        {
          ""actor"": ""plant in pot"",
          ""lines"": [
            {
              ""text"": ""... no""
            }
          ]
        }
      ]
    }
  ]
}
";
}
