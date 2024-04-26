using IdleEngine.Generator;
using IdleEngine.Sessions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.UserInterface
{
    public class CatHomeUi : MonoBehaviour
    {
        public TextMeshProUGUI NextCostText;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI CatNameText;
        public TextMeshProUGUI IncomePerMinuteText;
        public Image ProgressionImage;
        public Button BuyButton;
        public Image CatImage;
        public Button CatImageButton;
        private Sprite Cat;
        private Sprite CatEyesClosed;
        public Image CosmeticImage;
        public Button BackButton;
        //public Button VisitButton;

        //Accessoires:
        public Canvas AccessoiresCanvas;
        public SelectAccessoiresUi AccessoirePrefab;
        private int selection;
        //public Button AccessoiresButton;
        public Transform AccessoiresContainer;

        //CatMove
        public GameObject CatContainer;

        private Generator.Generator _generator;
        private Session _session;
        private MultipleCanvasController _controller;
        private List<SelectAccessoiresUi> _accessoires;

        /*
        public void CatPressed()
        {
            CatImage.sprite = CatEyesClosed;
        }
        public void CatReleased()
        {
            CatImage.sprite = Cat;
        }
        */

        public void OnAcessoireSelect(int index)
        {
            selection = index;

            if (index != -1)
            {
                for (int i = 0; i < _accessoires.Count; i++)
                {
                    if (index != i)
                    {
                        _accessoires[i].ResetSelection();
                    }
                }
            }
        }
        public void CloseAccessoires()
        {
            if (selection != -1)
            {
                CosmeticChange(_session.Cosmetic[selection]);
                CosmeticImage.sprite = _session.Cosmetic[selection].Image;
                CosmeticImage.enabled = true;
            }
            else
            {
                CosmeticChange(null);
                CosmeticImage.sprite = null;
                CosmeticImage.enabled = false;
            }

            _accessoires.Clear();

            for (var i = AccessoiresContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(AccessoiresContainer.GetChild(i).gameObject);
            }
            AccessoiresCanvas.enabled = false;
        }
        public void OpenAccessoires()
        {
            selection = -1;
            _accessoires = new List<SelectAccessoiresUi>();
            int temp = 0;

            foreach(var item in _session.Cosmetic)
            {
                if (item.Owned)
                {
                    var accessoireUi = Instantiate(AccessoirePrefab, AccessoiresContainer, false);
                    accessoireUi.CreateAccessoire(this.OnAcessoireSelect, item.Image, temp);
                    _accessoires.Add(accessoireUi);
                }

                temp++;
            }
            AccessoiresCanvas.enabled = true;
        }

        public void SetCatHome(Generator.Generator generator, Session session, MultipleCanvasController controller)
        {
            _generator = generator;
            _session = session;
            _controller = controller;
            //CosmeticImage.maskable = false;

            _generator.UpdateMoneyPerMinute();

            this.name = _generator.name;
            CatNameText.text = _generator.Name;
            //CatNameText.text = catname;
            CatImage.sprite = _generator.CatImage;
            Cat = _generator.CatImage;
            //CatEyesClosed = _generator.CatImageEyesClosed;

            if (_generator.Cosmetic != null)
            {
                CosmeticImage.sprite = _generator.Cosmetic.Image;
                CosmeticImage.enabled = true;
            }
            else
            {
                CosmeticImage.enabled = false;
            }
            
            BackButton.onClick.AddListener(() => _controller.CatHome_BackBtn_Click());
        }

        public void CosmeticChange(Cosmetic.Cosmetic cosmetic)
        {
            _generator.AttachCosmetic(cosmetic);
        }

        public void Buy()
        {
            _generator.Build(_session);
        }

        private void Update()
        {
            //move cat after specific time
        }

        private void LateUpdate()
        {
            UpdateUi();
        }

        private void UpdateUi()
        {
            if (!_generator)
            {
                return;
            }

            NextCostText.text = _generator.NextBuildingCostsForOne.Normal();
            LevelText.text = _generator.Owned.ToString();
            ProgressionImage.fillAmount = _generator.ProductionCycleNormalized;
            IncomePerMinuteText.text = $"{_generator.MoneyPerMinute.Normal()}/m";
            BuyButton.interactable = _generator.CanBeBuild(_session);
        }
    }
}
