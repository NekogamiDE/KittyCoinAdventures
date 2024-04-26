using System.Collections;
using System.Collections.Generic;
using IdleEngine;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine
{
    public class MultipleCanvasController : MonoBehaviour
    {
        private List<string> before_location = new List<string>();
        private string current_location;
        public List<Canvas> canvaslist = new List<Canvas>();
        public IdleEngine engine;

        //Help
        public Image HelpBackground;
        public TextMeshProUGUI HelpText;
        public Sprite OpenHelpButton;
        public Sprite CloseHelpButton;
        public Button HelpButton;
        private bool help = false;

        private void Start()
        {
            before_location.Clear();
            current_location = "Idle Canvas";
            UpdateLocation();
        }

        public void ToggleHelpBtn_Click()
        {
            help = !help;
            UpdateHelp(help);
        }
        public void UpdateHelp(bool help)
        {
            if (help)
            {
                HelpBackground.enabled = true;
                HelpText.enabled = true;
                HelpButton.image.sprite = CloseHelpButton;
            }
            else
            {
                HelpBackground.enabled = false;
                HelpText.enabled = false;
                HelpButton.image.sprite = OpenHelpButton;
            }
        }
        
        public void Idle_GoToBtn_Click()
        {
            engine.CreateGeneratorUis();

            before_location.Clear();
            current_location = "Idle Canvas";

            help = false;
            UpdateHelp(help);

            UpdateLocation();

            return;
        }

        public void Shop_GoToBtn_Click()
        {
            engine.CreateShopCatUi();

            if(current_location == "Shop Canvas")
            {
                return;
            }

            before_location.Add(current_location);
            current_location = "Shop Canvas";

            for (int i = 0; i < canvaslist.Count; i++)
            {
                if (canvaslist[i].name == current_location)
                {
                    //canvaslist[i] = null;
                }
            }

            UpdateLocation();

            return;
        }

        //zurück
        public void Shop_BackBtn_Click()
        {
            //-1 evtl. weglassen
            current_location = before_location[before_location.Count - 1];
            before_location.RemoveAt(before_location.Count - 1);

            if (current_location == "Idle Canvas")
                Idle_GoToBtn_Click();

            UpdateLocation(); //doppelt hält besser, oder so

            return;
        }

        public void CatHome_GoToBtn_Click(Generator.Generator generator/*, Image image*/)
        {
            engine.CreateCatHomeUi(generator);

            if (current_location == "CatHome Canvas")
            {
                return;
            }

            before_location.Add(current_location);
            current_location = "CatHome Canvas";

            for (int i = 0; i < canvaslist.Count; i++)
            {
                if (canvaslist[i].name == current_location)
                {
                    //canvaslist[i] = null;
                }
            }

            UpdateLocation();

            return;
        }

        //zurück
        public void CatHome_BackBtn_Click()
        {
            //-1 evtl. weglassen
            current_location = before_location[before_location.Count - 1];
            before_location.RemoveAt(before_location.Count - 1);

            if (current_location == "Idle Canvas")
                Idle_GoToBtn_Click();

            UpdateLocation(); //doppelt hält besser, oder so

            return;
        }

        private void UpdateLocation()
        {
            for (int i = 0; i < canvaslist.Count; i++)
            {
                if (canvaslist[i].name == current_location)
                    canvaslist[i].enabled = true;
                else
                    canvaslist[i].enabled = false;
            }

            return;
        }
    }
}
