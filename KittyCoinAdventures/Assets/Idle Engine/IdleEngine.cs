using System;
using IdleEngine.Generator;
using IdleEngine.SaveSystem;
using IdleEngine.Sessions;
using IdleEngine.UserInterface;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine
{
    public class IdleEngine : MonoBehaviour
    {
        public MultipleCanvasController canvas;
        public Session Session;
        public GeneratorUi GeneratorUiPrefab;
        public Transform GeneratorContainer;
        public ShopUi ShopUiPrefab;
        public Transform ShopItemContainer;
        public CatHomeUi CatHomeUiPrefab;
        public Transform CatHomeContainer;
        public TextMeshProUGUI CoinsText;
        public TextMeshProUGUI LevelText;



        private void Awake()
        {
            //canvas.Idle_GoToBtn_Click();
        }

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

            foreach (var cosmetic in Session.Cosmetic)
            {
                var shopUi = Instantiate(ShopUiPrefab, ShopItemContainer, false);
                shopUi.SetShopCosmetic(cosmetic, Session, canvas);
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
                shopUi.SetShopCat(generator, Session, canvas);
            }
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
            cathomeUi.SetCatHome(generator, Session, canvas);
        }

        private void ClearGeneratorUis()
        {
            for (var i = GeneratorContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(GeneratorContainer.GetChild(i).gameObject);
            }
        }

        public void CreateGeneratorUis()
        {
            ClearGeneratorUis();

            if (!Session)
            {
                return;
            }

            foreach (var generator in Session.Generator)
            {
                if (generator.Owned > 0)
                {
                    var generatorUi = Instantiate(GeneratorUiPrefab, GeneratorContainer, false);
                    generatorUi.SetGenerator(generator, Session, canvas);
                }
            }
        }

        private void Update()
        {
            if (!Session)
            {
                return;
            }

            //Debug.Log(Session.Generator[0].Owned.ToString());

            //CoinsText.text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //CoinsText.text = System.IO.Path.Combine(Application.persistentDataPath, "Saves");

            Session.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (!Session)
            {
                return;
            }

            int temp = 0;

            foreach (var generator in Session.Generator)
            {
                temp += generator.Owned;
            }

            LevelText.text = temp.ToString();//SaveFileManager.HomeRoot;
            CoinsText.text = Math.Floor(Session.Money).ToString();
        }

        private void OnEnable()
        {
            Enable();
        }

        private void Enable()
        {
            if (!Session)
            {
                return;
            }

            Load();

            Session.CalculateOfflineProgression();

            canvas.Idle_GoToBtn_Click(); //muss nach "Session.CalculateOfflineProgression();" kommen, da sonst die Generatorendaten noch nicht geladen werden.
        }

        //private void OnApplicationFocus(bool focus)
        //{
        //    if(focus)
        //    {
        //        Enable();
        //        return;
        //    }

        //    Disable();
        //}

        private void OnDisable()
        {
            Disable();
        }

        private void Disable()
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
                //Tut
                Session.Money = Session.Generator[0].NextBuildingCostsForOne;
                Session.Generator[0].Owned = 1;
                return;
            }

            // Spiel laden
            var data = JsonUtility.FromJson<Session.SaveData>(content);
            Session.SetRestorableData(data);
        }
    }
}