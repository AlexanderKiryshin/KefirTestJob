using Assets._Scripts.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.UI
{
    public class UIController:MonoBehaviour
    {
        [SerializeField] private Text pointsText;
        [SerializeField] private Text priceText;
        [SerializeField] private Button _learningButton;
        [SerializeField] private Button _forgetButton;
        [SerializeField] private Button _forgetAllButton;
        [SerializeField] private Button _workButton;

        private const string pointsPhrase = "очки:";
        private const string pricePhrase = "стоимость изучения:";
        public Action workingClick;
        public Action<TreeNode<Skill>> learningClick;
        public Action<TreeNode<Skill>> forgetClick;
        public Action forgetAllButtonsClick;
        private SkillButton _lastClickedButton;

        private void Awake()
        {
            pointsText.text = pointsPhrase + 0;
            priceText.text = pricePhrase + 0;
            _workButton.onClick.AddListener(OnWorkButtonClick);
            _learningButton.onClick.AddListener(OnLearningButtonClick);
            _forgetButton.onClick.AddListener(OnForgetButtonClick);
            _forgetAllButton.onClick.AddListener(OnForgetAllButtonClick);
        }

        public void OnWorkButtonClick()
        {
            workingClick?.Invoke();
        }
       
        public void OnLearningButtonClick()
        {
           if (_lastClickedButton!=null)
            learningClick?.Invoke(_lastClickedButton.skill);
        }

        public void OnForgetButtonClick()
        {
            if (_lastClickedButton != null)
                forgetClick?.Invoke(_lastClickedButton.skill);
        }

        public void OnForgetAllButtonClick()
        {
            forgetAllButtonsClick?.Invoke();
        }

        public void UpdatePoints(int points)
        {
            pointsText.text = pointsPhrase + points;
        }

        public void Intialize(List<SkillButton> skillButtons)
        {
            foreach (var skill in skillButtons)
            {
                skill.SubscribeButton(OnClickSkillButton);
            }
        }

        private void OnClickSkillButton(SkillButton button)
        {
            priceText.text = pricePhrase + button.skill.Value.price;
            _lastClickedButton = button;
        }
    }
}
