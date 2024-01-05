using System;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Build.UI
{
    [System.Serializable]
    public class KeyButtonPromptPair
    {
        public string Key;
        public AnimatedUiElement AnimatedUiElement;
    }
    public class ButtonPromptEventRouter : MonoBehaviour
    {
        [System.Serializable]
        public enum InputMode
        {
            Keyboard,
            Controller
        }

        public InputMode Mode;
        public List<KeyButtonPromptPair> PromptDictionary;
        public void ShowUpPrompt(bool shouldDisplay)
        {
            AnimatedUiElement prompt = GetElementWithKey("DpadUp");

            if (prompt != null)
            {
                prompt.gameObject.SetActive(shouldDisplay);
            }

        }

        public void ShowUpgradeMenuPrompt(bool shouldDisplay)
        {
            AnimatedUiElement prompt = null;
            switch (Mode)
            {
                case InputMode.Keyboard:
                    prompt = GetElementWithKey("Y");
                    break;
                case InputMode.Controller:
                    prompt = GetElementWithKey("Tab");
                    break;
            }

            if (prompt != null)
            {
                prompt.gameObject.SetActive(shouldDisplay);
            }
        }

        private AnimatedUiElement GetElementWithKey(string key)
        {
            foreach (KeyButtonPromptPair pair in PromptDictionary)
            {
                if (pair.Key == key)
                {
                    return pair.AnimatedUiElement;
                }
            }

            return null;
        }
    }
}