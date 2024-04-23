using System.Collections;
using System.Collections.Generic;
using IdleEngine;
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



        private void Start()
        {
            before_location.Clear();
            current_location = "Idle Canvas";
            UpdateLocation();
        }

        //hin
        public void Idle_GoToBtn_Click()
        {
            before_location.Clear();
            current_location = "Idle Canvas";

            UpdateLocation();

            return;
        }

        public void CatHome_GoToBtn_Click(Generator.Generator generator/*, Image image*/)
        {
            engine.CreateCatHomeUi(generator);

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

            UpdateLocation();

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
