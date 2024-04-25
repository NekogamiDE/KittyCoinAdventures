using IdleEngine.SaveSystem;
using IdleEngine.Sessions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace IdleEngine.Cosmetic
{

    [CreateAssetMenu(fileName = "Cosmetic", menuName = "Game/Cosmetic")]
    public class Cosmetic : ScriptableObject, IRestorable<Cosmetic.RuntimeData>
    {
        [Serializable]
        public class RuntimeData
        {
            public string Id;
            public bool Owned;
        }

        private RuntimeData _data = new RuntimeData();

        public string Id
        {
            get => _data.Id;
            set => _data.Id = value;
        }
        public bool Owned
        {
            get => _data.Owned;
            set => _data.Owned = value;
        }


        public string Name;
        public Sprite Image;
        public double Price;
        //public string type;
        public string Explanation;

        //Attributes (Prozent)
        public bool additive;
        public float BaseProductionTimeInSeconds;
        public float BaseRevenue;
        public float CostFactor;
        public int ProductionCount;
        //multipliers

        private void OnEnable()
        {
            _data = new RuntimeData();
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

        public RuntimeData GetRestorableData()
        {
            return new RuntimeData()
            {
                Id = name,
                Owned = Owned
            };
        }

        public void SetRestorableData(RuntimeData data)
        {
            _data = data;
        }
    }
}
