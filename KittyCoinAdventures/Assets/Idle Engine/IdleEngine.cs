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
        public MultipleCanvasController canvas;
        public Session Session;
        public GeneratorUi GeneratorUiPrefab;
        public Transform GeneratorContainer;
        public CatHomeUi CatHomeUiPrefab;
        public Transform CatHomeContainer;
        public TextMeshProUGUI CoinsText;


        private void Awake()
        {
            ClearGeneratorUis();
            CreateGeneratorUis();

            canvas.Idle_GoToBtn_Click();
        }

        private void ClearGeneratorUis()
        {
            for (var i = GeneratorContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(GeneratorContainer.GetChild(i).gameObject);
            }
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

            private void CreateGeneratorUis()
        {
            if (!Session)
            {
                return;
            }

            foreach (var generator in Session.Generator)
            {
                var generatorUi = Instantiate(GeneratorUiPrefab, GeneratorContainer, false);
                generatorUi.SetGenerator(generator, Session, canvas);
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