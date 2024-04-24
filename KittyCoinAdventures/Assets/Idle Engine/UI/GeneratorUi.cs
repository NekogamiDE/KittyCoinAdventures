using IdleEngine.Generator;
using IdleEngine.Sessions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.UserInterface
{
    public class GeneratorUi : MonoBehaviour
    {
        //public TextMeshProUGUI NextCostText;
        public TextMeshProUGUI LevelText;
        public TextMeshProUGUI CatNameText;
        public TextMeshProUGUI NextUpgradeCost;
        public Image ProgressionImage;
        public TextMeshProUGUI EarningsLeftText;
        public Image EarningsLeftImage;
        public Image CatImage;
        //public Button BuyButton;

        private Generator.Generator _generator;
        private Session _session;
        private MultipleCanvasController _controller;

        public void SetGenerator(Generator.Generator generator, Session session, MultipleCanvasController controller)
        {
            _session = session;
            _generator = generator;
            _controller = controller;

            CatImage.sprite = _generator.CatImage;
            //CatNameText.text = generator.GetInstanceID().ToString();
            CatNameText.text = _generator.Name;
        }

        public void CatVisit_BtnClick()
        {
            _controller.CatHome_GoToBtn_Click(_generator);
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

            //NextCostText.text = _generator.NextBuildingCostsForOne.Normal();
            LevelText.text = _generator.Owned.ToString();
            ProgressionImage.fillAmount = _generator.ProductionCycleNormalized;
            NextUpgradeCost.text = $"{_generator.NextBuildingCostsForOne.Normal()}";
            if(_session.Money >= _generator.NextBuildingCostsForOne)
            {
                NextUpgradeCost.color = Color.white;
            }
            else
            {
                NextUpgradeCost.color = Color.red;
            }
            EarningsLeftImage.fillAmount = _generator.ProductionsLeftNormalized;
            EarningsLeftText.text = $"{_generator.Earnings}";
            //BuyButton.interactable = _generator.CanBeBuild(_session);
        }
    }
}