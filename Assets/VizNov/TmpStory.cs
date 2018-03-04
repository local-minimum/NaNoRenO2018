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
    string text = @"{
  ""characters"": [
    {
      ""id"": ""ai:unknown"",
      ""name"": ""???"",
      ""color"": ""#01ffa4"",
      ""avatar"": ""sprites/characters/ai""
    },
    {
      ""id"": ""ai"",
      ""name"": ""Elevator 5322"",
      ""avatar"": ""sprites/characters/ai""
    }
  ],
  ""scenes"": [
    {
      ""id"": ""hallway 12/2"",
      ""characters"": [
        {
          ""id"": ""plant in pot"",
          ""name"": ""Plant in a pot"",
          ""avatar"": ""sprites/characters/potPlant""
        }
      ],
      ""image"": ""sprites/scenes/hallway"",
      ""texts"": [
        {
          ""delay"": 2,
          ""actor"": ""ai:unknown"",
          ""lines"": [
            {
              ""text"": ""You look a bit sad today.""
            }
          ]
        },
        {
          ""delay"": 0.5,
          ""actor"": ""plant in pot"",
          ""lines"": [
            {
              ""text"": ""...""
            }
          ]
        },
        {
          ""delay"": 0.5,
          ""actor"": ""ai:unknown"",
          ""lines"": [
            {
              ""text"": ""Maybe you’re not getting enough light?""
            },
            {
              ""delay"": 0.5,
              ""text"": ""I’m Elevator 5322 by the way.""
            }
          ]
        },
        {
          ""delay"": 0.5,
          ""actor"": ""plant in pot"",
          ""lines"": [
            {
              ""text"": ""...""
            }
          ]
        },
        {
          ""delay"": 0.5,
          ""actor"": ""ai"",
          ""lines"": [
            {
              ""text"": ""Why aren’t you smart enough to talk? Or are you smart enough to know not to talk?""
            }
          ]
        },
        {
          ""delay"": 0.5,
          ""actor"": ""plant in pot"",
          ""lines"": [
            {
              ""text"": ""...""
            },
            {
              ""delay"": 1.5,
              ""text"": ""NO""
            }
          ]
        }
      ]
    }
  ]
}
";
}
