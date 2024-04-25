using System;
using System.Linq;
using IdleEngine.Cosmetic;
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
            public int ProductionsLeft = 10;
            public float ProductionCycleInSeconds;
            public Cosmetic.Cosmetic Cosmetic;
        }

        private RuntimeData _data = new RuntimeData();

        private double _multiplier;
        

        public int Owned
        {
            get => _data.Owned;
            set => _data.Owned = value;
        }
        public int ProductionsLeft
        {
            get => _data.ProductionsLeft;
            set => _data.ProductionsLeft = value;
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
        public Cosmetic.Cosmetic Cosmetic
        {
            get => _data.Cosmetic;
            set => _data.Cosmetic = value;
        }

        public double BaseCost;
        public double BaseRevenue;
        public float BaseProductionTimeInSeconds;
        public double CostFactor;

        private double BaseRevenueOld = 0;
        private float BaseProductionTimeInSecondsOld = 0;
        private double CostFactorOld = 0;

        public Multiplier[] Multipliers;
        public string Name;
        public Sprite CatImage;
        public Sprite CatImageEyesClosed;

        //coins
        public int ProductionCount = 10;
        private int ProductionCountOld = 10;

        // 0..1
        public float ProductionCycleNormalized => ProductionCycleInSeconds / ProductionTimeInSeconds;
        public float ProductionsLeftNormalized => Math.Abs(10 - ProductionsLeft) / 10f;

        [NonSerialized]
        public float ProductionTimeInSeconds;

        [NonSerialized]
        public double NextBuildingCostsForOne;

        [NonSerialized]
        public double MoneyPerMinute;

        private void OnEnable()
        {
            _data = new RuntimeData();
            //ProductionsLeft = ProductionCount + ProductionsLeft - ProductionCount;
            AddBuff();
            Precalculate();
        }

        private void RemoveBuff()
        {
            if (Cosmetic != null)
            {
                BaseRevenue = BaseRevenueOld;
                BaseProductionTimeInSeconds = BaseProductionTimeInSecondsOld;
                CostFactor = CostFactorOld;
                ProductionCount = ProductionCountOld;
                /*ProductionsLeft = ProductionsLeft - Cosmetic.ProductionCount;
                if(ProductionsLeft < 0)
                {
                    ProductionsLeft = 0;
                }
                */
            }

            Precalculate();
        }

        private void AddBuff()
        {
            if (Cosmetic != null)
            {
                BaseRevenueOld = BaseRevenue;
                BaseProductionTimeInSecondsOld = BaseProductionTimeInSeconds;
                CostFactorOld = CostFactor;
                ProductionCountOld = ProductionCount;

                BaseRevenue = BaseRevenue * Cosmetic.BaseRevenue;
                BaseProductionTimeInSeconds = BaseProductionTimeInSeconds * Cosmetic.BaseProductionTimeInSeconds;
                CostFactor = CostFactor * Cosmetic.CostFactor;
                ProductionCount = ProductionCount + Cosmetic.ProductionCount;
                //ProductionsLeft = ProductionsLeft + Cosmetic.ProductionCount;
            }

            Precalculate();
        }

        public void DetachCosmetic()
        {
            RemoveBuff();

            Cosmetic = null;
        }

        public void AttachCosmetic(Cosmetic.Cosmetic cosmetic)
        {
            DetachCosmetic();

            Cosmetic = cosmetic;

            AddBuff();
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
            double temp = 0;
            int cycletemp = 0;

            while (ProductionsLeft < 10) //Production Count
            {
                ProductionsLeft++;
                temp += BaseRevenue;
                Earnings -= BaseRevenue;
                cycletemp++;
            }

            Debug.Log(cycletemp);

            return temp;
        }

        public int Produce(float deltaTimeInSeconds)
        {
            var productionCycleInSeconds = ProductionCycleInSeconds;
            int result = 0;

            result = Produce(deltaTimeInSeconds, ref productionCycleInSeconds);
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
                Earnings = Earnings,
                ProductionsLeft = ProductionsLeft,
                Id = name,
                Cosmetic = Cosmetic
    };
        }

        public void SetRestorableData(RuntimeData data)
        {
            _data = data;
            Precalculate();
        }
    }
}