using System;
using IdleEngine.Generator;
using IdleEngine.SaveSystem;
using IdleEngine.Sessions;
using IdleEngine.UserInterface;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace IdleEngine
{
    public class IdleEngine : MonoBehaviour
    {
        public Session Session;
        public GeneratorUi GeneratorUiPrefab;
        public Transform GeneratorContainer;
<<<<<<< Updated upstream
=======
        public CatHomeUi CatHomeUiPrefab;
        public Transform CatHomeContainer;
        public ShopUi ShopUiPrefab;
        public Transform ShopItemContainer;
        //public Transform ShopCatContainer;
>>>>>>> Stashed changes
        public TextMeshProUGUI CoinsText;

        private void Awake()
        {
            ClearGenerators();
            CreateGeneratorUis();
        }

        private void ClearGenerators()
        {
            for (var i = GeneratorContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(GeneratorContainer.GetChild(i).gameObject);
            }
        }

<<<<<<< Updated upstream
=======
        public void CreateShopCosmeticUi()
        {
            if (!Session)
            {
                return;
            }

            for (var i = ShopItemContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(ShopItemContainer.GetChild(i).gameObject);
            }

            /*foreach (var generator in Session.Generator)
            {
                var shopUi = Instantiate(ShopUiPrefab, ShopCatContainer, false);
                shopUi.SetShop(canvas);
            }*/
            foreach (var cosmetic in Session.Cosmetic)
            {
                var shopUi = Instantiate(ShopUiPrefab, ShopItemContainer, false);
                //shopUi.SetShop(canvas);
            }
        }

        public void ShopCosmeticBtn_Click()
        {
            CreateShopCosmeticUi();
        }


        public void CreateShopCatUi()
        {
            if (!Session)
            {
                return;
            }

            for (var i = ShopItemContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(ShopItemContainer.GetChild(i).gameObject);
            }

            foreach (var generator in Session.Generator)
            {
                var shopUi = Instantiate(ShopUiPrefab, ShopItemContainer, false);
                //shopUi.SetShop(canvas);
            }
            /*foreach (var cosmetic in Session.Cosmetic)
            {
                var shopUi = Instantiate(ShopUiPrefab, ShopItemContainer, false);
                shopUi.SetShop(canvas);
            }*/
        }
        public void ShopCatBtn_Click()
        {
            CreateShopCatUi();
        }

        

        public void CreateCatHomeUi(Generator.Generator generator)
        {
            for (var i = CatHomeContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(CatHomeContainer.GetChild(i).gameObject);
            }

            var cathomeUi = Instantiate(CatHomeUiPrefab, CatHomeContainer, false);
            cathomeUi.SetCatHome(generator, canvas);
        }

>>>>>>> Stashed changes
        private void CreateGeneratorUis()
        {
            if (!Session)
            {
                return;
            }

            foreach (var generator in Session.Generator)
            {
                var generatorUi = Instantiate(GeneratorUiPrefab, GeneratorContainer, false);
                generatorUi.SetGenerator(generator, Session);
            }
        }

        private void Update()
        {
            if (!Session)
            {
                return;
            }

            Session.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (!Session)
            {
                return;
            }

            CoinsText.text = Session.Money.Normal();
        }

        private void OnEnable()
        {
            if (!Session)
            {
                return;
            }

            Load();

            Session.CalculateOfflineProgression();
        }

        private void OnDisable()
        {
            if (!Session)
            {
                return;
            }

            Session.LastTicks = DateTime.UtcNow.Ticks;

            Save();
        }

        private void Save()
        {
            var data = Session.GetRestorableData();
            var json = JsonUtility.ToJson(data);

            SaveFileManager.Write(Session.name, json);
        }

        private void Load()
        {
            if (!SaveFileManager.TryLoad(Session.name, out var content))
            {
                // Neues Spiel wurde angefangen
                Session.Money = Session.Generator[0].NextBuildingCostsForOne;
                return;
            }

            // Spiel laden
            var data = JsonUtility.FromJson<Session.SaveData>(content);
            Session.SetRestorableData(data);
        }
    }
}