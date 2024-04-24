using IdleEngine.Sessions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.Cosmetic
{

    [CreateAssetMenu(fileName = "Cosmetic", menuName = "Game/Cosmetic")]
    public class Cosmetic : ScriptableObject
    {
        public string Name;
        public Sprite Image;
        public double Price;
        public bool Owned;
        //public string type;
        public string Explanation;
        public CosmeticAttributes attributes;

        public class CosmeticAttributes
        {
            public bool additive;
            public float BaseProductionTimeInSeconds;
            public int BaseRevenue;
            public float CostFactor;
            public int ProductionCount;
            //multipliers
        }

        public bool CanBeBuild(Session session)
        {
            return session.Money >= Price;
        }

        public void Build(Session session)
        {
            if (!CanBeBuild(session))
            {
                return;
            }

            Owned = true;
            session.Money -= Price;
        }
    }
}
