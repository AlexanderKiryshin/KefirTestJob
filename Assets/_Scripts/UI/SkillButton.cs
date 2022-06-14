using Assets._Scripts.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets._Scripts.UI
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Color _learnedColor;
        [SerializeField] private Color _unlearnedColor;
        [SerializeField] private Graphic _buttonGraphic;
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _selection;
        private TreeNode<Skill> _skill;
        private Action<SkillButton> onClick;
        public TreeNode<Skill> skill => _skill;

        private void Awake()
        {
            _button.onClick.AddListener(Select);
        }
        public void Initialize(string name,bool isLearned,TreeNode<Skill> skill)
        {
            _skill = skill;
            _skill.Value.OnIsLearned += SetStatus;
            SetName(name);
            SetStatus(isLearned);
        }
        public void SetName(string name)
        {
            _name.text = name;
        }

        public void SetStatus(bool isLearned)
        {
            _buttonGraphic.color = isLearned ? _learnedColor : _unlearnedColor;
        }

        public RectTransform GetRectTransform()
        {
            return _rectTransform;
        }

        public void Select(bool isSelected)
        {
            _selection.enabled = isSelected;
        }

        public bool IsSelected()
        {
            return _selection.enabled;
        }

        public void SubscribeButton(Action<SkillButton> callback)
        {
            onClick += callback;
        }

        public void Unsubscribe(Action<SkillButton> callback)
        {
            onClick -= callback;
        }

        private void Select()
        {
            onClick?.Invoke(this);
            Select(true);
        }

        private void OnDestroy()
        {
            _skill.Value.OnIsLearned -= SetStatus;
        }
    }
}
