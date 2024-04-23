using IdleEngine.Generator;
using IdleEngine.Sessions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.UserInterface
{
    public class GeneratorUi : MonoBehaviour
    {
        public TextMeshProUGUI NextCostText;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI CatNameText;
        public TextMeshProUGUI IncomePerMinuteText;
        public Image ProgressionImage;
        public Image CatImage;
        public Button BuyButton;

        private Generator.Generator _generator;
        private Session _session;
        private MultipleCanvasController _controller;

        public void SetGenerator(Generator.Generator generator, Session session, MultipleCanvasController controller)
        {
            _session = session;
            _generator = generator;
            _controller = controller;

            CatImage.sprite = _generator.Image;
            //CatNameText.text = generator.GetInstanceID().ToString();
            CatNameText.text = _generator.Name;
        }

        public void CatVisit_BtnClick()
        {
            _controller.CatHome_GoToBtn_Click(_generator);
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