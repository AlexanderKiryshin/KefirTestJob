using System;
using UnityEngine;

namespace Assets._Scripts.Skills
{
    public class Skill
    {
        public int price {get;private set; }
        public string name { get; private set; }
        public bool mainNode { get; private set; }
        public bool marked { get; set; }
        private bool _isLearned;
        public bool isLearned 
        {
            get 
            {
                return _isLearned;
            }
            set
            {
                _isLearned = value;
                OnIsLearned?.Invoke(_isLearned);
            }
               
        }
        public Vector3 position { get; private set; }
        public bool isInitialized { get; set; }
        public RectTransform rectTransform { get; set; }

        public Action<bool> OnIsLearned;
        public Skill(string name, int price,Vector3 position , bool isLearned=false,bool mainNode= false)
        {
            this.name = name;
            this.price = price;
            this.isLearned = isLearned;
            this.position = position;
            this.mainNode = mainNode;
        }
    }
}
