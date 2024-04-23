using System;
using System.Linq;
using IdleEngine.SaveSystem;
using IdleEngine.Sessions;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.Cosmetic
{

    [CreateAssetMenu(fileName = "Cosmetic", menuName = "Game/Cosmetic")]
    public class Cosmetic : ScriptableObject {

        public string Name;
        public double price;
        public Sprite image;
        public string type;
        public string explanation;
        public Button buy;
        public bool owned;
    }
}