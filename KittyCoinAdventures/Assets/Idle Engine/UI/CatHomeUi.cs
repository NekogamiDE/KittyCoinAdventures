using IdleEngine.Sessions;
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
        public Button BackButton;
        //public Button VisitButton;

        private Generator.Generator _generator;
        private Session _session;
        private MultipleCanvasController _controller;

        public void SetCatHome(Generator.Generator generator, Session session, MultipleCanvasController controller)
        {
            _generator = generator;
            _session = session;
            _controller = controller;

            this.name = _generator.name;
            //CatNameText.text = catname;
            CatImage.sprite = _generator.Image;

            BackButton.onClick.AddListener(() => _controller.CatHome_BackBtn_Click());
        }
        public void Buy()
        {
            _generator.Build(_session);
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
