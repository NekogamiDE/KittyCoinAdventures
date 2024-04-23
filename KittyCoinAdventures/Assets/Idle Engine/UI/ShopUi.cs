using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.UserInterface
{
    public class ShopUi : MonoBehaviour
    {
        public string Itemname;
        public double price;
        public Sprite image;
        public string type;
        public string explanation;
        public Button buy;
        public bool owned;
        public Button BackButton;


        public void SetShop(MultipleCanvasController controller)
        {
            
            BackButton.onClick.AddListener(() => controller.Shop_BackBtn_Click());
        }
    }
}
