using System;
using System.Linq;
using IdleEngine.SaveSystem;
using IdleEngine.Sessions;
using UnityEngine;

namespace IdleEngine.Generator
{

    [CreateAssetMenu(fileName = "Generator", menuName = "Game/Generator")]
    public class Generator : ScriptableObject, IRestorable<Generator.RuntimeData>
    {
        [Serializable]
        public class RuntimeData
        {
            public string Id;
            public int Owned;
            public double Earnings;
            public float ProductionCycleInSeconds;
        }

        private RuntimeData _data = new RuntimeData();

        private double _multiplier;
        

        public int Owned
        {
            get => _data.Owned;
            set => _data.Owned = value;
        }

        public float ProductionCycleInSeconds
        {
            get => _data.ProductionCycleInSeconds;
            set => _data.ProductionCycleInSeconds = value;
        }

        public double Earnings
        {
            get => _data.Earnings;
            set => _data.Earnings = value;
        }

        public double BaseCost;
        public double BaseRevenue;
        public float BaseProductionTimeInSeconds;
        
        public double CostFactor;
        public Multiplier[] Multipliers;
        public string Name;
        public Sprite Image;

        //coins
        public int ProductionCount;
        private int ProductionsLeft;

        // 0..1
        public float ProductionCycleNormalized => ProductionCycleInSeconds / ProductionTimeInSeconds;

        [NonSerialized]
        public float ProductionTimeInSeconds;

        [NonSerialized]
        public double NextBuildingCostsForOne;

        [NonSerialized]
        public double MoneyPerMinute;

        private void OnEnable()
        {
            _data = new RuntimeData();
            ProductionsLeft = 5;//ProductionCount;
            Precalculate();
        }

        public bool CanBeBuild(Session session)
        {
            return session.Money >= NextBuildingCostsForOne;
        }

        public void Build(Session session)
        {
            if (!CanBeBuild(session))
            {
                return;
            }

            Owned++;
            session.Money -= NextBuildingCostsForOne;
            Precalculate();
        }

        public double Collect()
        {
            ProductionsLeft = 5; //ProductionCount;
            double temp = Earnings;
            Earnings = 0;
            return temp;
        }

        public int Produce(float deltaTimeInSeconds)
        {
            var productionCycleInSeconds = ProductionCycleInSeconds;
            int result = Produce(deltaTimeInSeconds, ref productionCycleInSeconds);
            ProductionCycleInSeconds = productionCycleInSeconds;
            return result;
        }

        private int Produce(float deltaTimeInSeconds, ref float productionCycleInSeconds)
        {
            if (Owned == 0)
            {
                return 0;
            }

            productionCycleInSeconds += deltaTimeInSeconds;

            if (ProductionsLeft == 0)
            {
                productionCycleInSeconds = 0;
                return 2;
            }

            if(productionCycleInSeconds < 0 && productionCycleInSeconds <= -ProductionTimeInSeconds && ProductionsLeft > 0)
            {
                Earnings += -BaseRevenue * Owned * _multiplier;
                productionCycleInSeconds += ProductionTimeInSeconds;
                ProductionsLeft--;
                return 1;
            }

            if(productionCycleInSeconds >= ProductionTimeInSeconds && ProductionsLeft > 0)
            {
                Earnings += BaseRevenue * Owned * _multiplier;
                productionCycleInSeconds -= ProductionTimeInSeconds;
                ProductionsLeft--;
                return 1;
            }

            return 0;
        }

        private void Precalculate()
        {
            
            UpdateModifiers();
            UpdateMultiplier();
            UpdateNextBuildingCosts();
            UpdateMoneyPerMinute();
        }

        private void UpdateMoneyPerMinute()
        {
            //var productionCycleInSeconds = 0f;
            //MoneyPerMinute = this.BaseRevenue / 60.0 * ProductionTimeInSeconds;
            float mpmcycle = 60f;
            double sum = 0;
            while(mpmcycle >= ProductionTimeInSeconds)
            {
                sum += BaseRevenue * Owned *_multiplier;
                mpmcycle -= ProductionTimeInSeconds;
            }
            MoneyPerMinute = sum; //(60, ref productionCycleInSeconds);
        }

        private void UpdateNextBuildingCosts()
        {
            var kOverR = Math.Pow(CostFactor, Owned);
            var kPlusNOverR = Math.Pow(CostFactor, Owned + 1);

            NextBuildingCostsForOne = BaseCost *
                                      (
                                        (kOverR - kPlusNOverR)
                                        /
                                        (1 - CostFactor)
                                      );
        }

        private void UpdateModifiers()
        {
            ProductionTimeInSeconds = BaseProductionTimeInSeconds;

            if (Owned > 10)
            {
                //ProductionTimeInSeconds /= 2;
            }
        }

        private void UpdateMultiplier()
        {
            if (Multipliers == null)
            {
                _multiplier = 1;
                return;
            }

            _multiplier = Multipliers.Aggregate(1d, (acc, multiplier) => acc * (multiplier.Level <= Owned ? multiplier.Value : 1));
        }

        private void OnValidate()
        {
            Precalculate();
        }

        public RuntimeData GetRestorableData()
        {
            return new RuntimeData()
            {
                Owned = Owned,
                ProductionCycleInSeconds = ProductionCycleInSeconds,
                Id = name
            };
        }

        public void SetRestorableData(RuntimeData data)
        {
            _data = data;
            Precalculate();
        }
    }
}