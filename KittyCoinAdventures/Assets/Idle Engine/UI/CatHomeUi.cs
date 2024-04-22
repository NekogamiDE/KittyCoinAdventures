using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.UserInterface
{
    public class CatHomeUi : MonoBehaviour
    {
        //public TextMeshProUGUI NextCostText;
        //public TextMeshProUGUI LevelText;
                                //public TextMeshProUGUI CatNameText;
        //public TextMeshProUGUI IncomePerMinuteText;
        //public Image ProgressionImage;
        public Image CatImage;
        public Button BackButton;
        //public Button VisitButton;

        public void SetCatHome(Generator.Generator generator, MultipleCanvasController controller)
        {
            //CatNameText.text = catname;
            CatImage.sprite = generator.Image;

            BackButton.onClick.AddListener(() => controller.CatHome_BackBtn_Click());
        }
    }
}
