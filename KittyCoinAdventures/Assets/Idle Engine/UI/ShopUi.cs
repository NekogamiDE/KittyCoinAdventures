using IdleEngine.Generator;
using IdleEngine.Sessions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.UserInterface
{
    public class ShopUi : MonoBehaviour
    {
        public TextMeshProUGUI itemnameText;
        public TextMeshProUGUI priceText;
        public Image itemImage; 
        public TextMeshProUGUI explanationText;
        public Button buyButton;
        public bool owned;

        private string type;
        private Generator.Generator _generator;
        private Cosmetic.Cosmetic _cosmetic;
        private Session _session;
        private MultipleCanvasController _controller;

        public void SetShopCat(Generator.Generator generator, Session session, MultipleCanvasController controller)
        {
            _session = session;
            _generator = generator;
            _controller = controller;

            itemImage.sprite = _generator.CatImage;
            itemnameText.text = _generator.Name;
            priceText.text = _generator.BaseCost.ToString();
            //explanationText.text = "";

            type = "Cat";

            //BackButton.onClick.AddListener(() => controller.Shop_BackBtn_Click());

            //ShopUiUpdate();
        }

        public void SetShopCosmetic(Cosmetic.Cosmetic cosmetic, Session session, MultipleCanvasController controller)
        {
            _session = session;
            _cosmetic = cosmetic;
            _controller = controller;

            itemImage.sprite = _cosmetic.Image;
            itemnameText.text = _cosmetic.Name;
            priceText.text = _cosmetic.Price.ToString();
            //explanationText.text = _cosmetic.Explanation;

            type = "Cosmetic";
            owned = _cosmetic.Owned;

            //BackButton.onClick.AddListener(() => controller.Shop_BackBtn_Click());

            //ShopUiUpdate();
        }

        public void Buy()
        {
            if(type == "Cat")
            {
                _generator.Build(_session);
                //-Geld
                //_generator.Owned++; //= 1 geht auch
            }
            else
            {
                _cosmetic.Build(_session);
            }

            //ShopUiUpdate();
        }

        public void LateUpdate()
        {
            if (type == "Cat")
            {
                if (_generator.Owned > 0) 
                {
                    owned = true;
                    buyButton.interactable = false;
                    priceText.color = Color.gray;
                }
                else
                {
                    owned = false;
                    if (_session.Money >= _generator.NextBuildingCostsForOne)
                    {
                        buyButton.interactable = true;
                        priceText.color = Color.white;
                    }
                    else
                    {
                        buyButton.interactable = false;
                        priceText.color = Color.red;
                    }
                }
            }
            else
            {
                if (_cosmetic.Owned)
                {
                    owned = true;
                    buyButton.interactable = false;
                    priceText.color = Color.gray;
                }
                else
                {
                    owned = false;
                    if (_session.Money >= _cosmetic.Price)
                    {
                        buyButton.interactable = true;
                        priceText.color = Color.white;
                    }
                    else
                    {
                        buyButton.interactable = false;
                        priceText.color = Color.red;
                    }
                }
            }
            
        }
    }
}
