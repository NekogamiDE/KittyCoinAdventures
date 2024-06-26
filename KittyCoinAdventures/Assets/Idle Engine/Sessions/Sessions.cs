using System;
using System.Linq;
using IdleEngine.Generator;
using IdleEngine.Cosmetic;
using IdleEngine.SaveSystem;
using UnityEngine;

namespace IdleEngine.Sessions
{
    [CreateAssetMenu(fileName = "Session", menuName = "Game/Session")]
    public class Session : ScriptableObject, IRestorable<Session.SaveData>
    {
        [Serializable]
        public class RuntimeData
        {
            public double GeneratedMoney;
            public double Money;
            public long LastTicks;
        }

        [Serializable]
        public class SaveData : RuntimeData
        {
            public Generator.Generator.RuntimeData[] Generator;
            public Cosmetic.Cosmetic.RuntimeData[] Cosmetic;
        }

        private RuntimeData _data = new();

        public Generator.Generator[] Generator;
        public Cosmetic.Cosmetic[] Cosmetic;

        public double GeneratedMoney
        {
            get => _data.GeneratedMoney;
            set => _data.GeneratedMoney = value;
        }

        public double Money
        {
            get => _data.Money;
            set => _data.Money = value;
        }

        public long LastTicks
        {
            get => _data.LastTicks;
            set => _data.LastTicks = value;
        }

        private void OnEnable()
        {
            _data = new RuntimeData();
        }

        public void Tick(float deltaTimeInSeconds)
        {
            CalculateProgress(deltaTimeInSeconds);
        }

        public void GetCoins(UnityEngine.Object obj)
        {
            foreach (var item in Generator)
            {
                if(item.Name == obj.name)
                {
                    if (item.Earnings > 0)
                    {
                        Money += item.Collect(); //nicht produce, sondern extra funktion im generator f�rs einsammeln der coins
                    }
                }
            }

            return;
        }

        private void CalculateProgress(float deltaTimeInSeconds)
        {
            foreach (var item in Generator)
            {
                int temp = item.Produce(deltaTimeInSeconds);

                if (temp == 1)
                {
                    GeneratedMoney += item.Earnings;
                }
                else if(temp == 2)
                {
                    //Debug.Log(item.Name + " Ist voll!");
                }
            }

            return;
        }

        public void CalculateOfflineProgression()
        {
            if (LastTicks <= 0)
            {
                return;
            }

            var deltaTime = (DateTime.UtcNow.Ticks - LastTicks) / TimeSpan.TicksPerSecond;

            var moneyBefore = Money;

            Tick(deltaTime);

            Debug.Log($"Calculated offline progression: {Money - moneyBefore}");
        }

        public SaveData GetRestorableData()
        {
            return new SaveData()
            {
                Money = Money,
                LastTicks = LastTicks,
                Generator = Generator.Select(generator => generator.GetRestorableData()).ToArray(),
                Cosmetic = Cosmetic.Select(cosmetic => cosmetic.GetRestorableData()).ToArray()
            };
        }

        public void SetRestorableData(SaveData data)
        {
            Money = data.Money;
            LastTicks = data.LastTicks;

            foreach (var generator in Generator)
            {
                var savedGenerator = data.Generator.SingleOrDefault(g => g.Id == generator.Name);

                if (savedGenerator is null)
                {
                    Debug.LogWarning($"Did not find generator {generator.Name} in save game");
                    continue;
                }

                generator.SetRestorableData(savedGenerator);
            }

            foreach (var cosmetic in Cosmetic)
            {
                var savedCosmetic = data.Cosmetic.SingleOrDefault(c => c.Id == cosmetic.name);

                if (savedCosmetic is null)
                {
                    Debug.LogWarning($"Did not find cosmetic {cosmetic.name} in save game");
                    continue;
                }

                cosmetic.SetRestorableData(savedCosmetic);
            }
        }
    }
}