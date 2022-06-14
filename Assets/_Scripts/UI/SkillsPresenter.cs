using Assets._Scripts.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.UI
{
    public class SkillsPresenter : MonoBehaviour
    {
        [SerializeField] private SkillsTree _skillsTree;
        [SerializeField] private SkillButton _skillPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private Transform _linesTransform;
        [SerializeField] private RectTransform _linePrefab;
        [SerializeField] private UIController _uIController;
        private List<SkillButton> _skills;
        private SkillButton selected;
        private void Start()
        {
            _skills = new List<SkillButton>();
            var skillButton = Instantiate(_skillPrefab, _content);
            selected = skillButton;
            _skills.Add(skillButton);
            skillButton.SubscribeButton(OnSelectSkill);
            var treeNode = _skillsTree.GetSkills();
            skillButton.Initialize(treeNode.Value.name, treeNode.Value.isLearned, treeNode);
            skillButton.GetRectTransform().localPosition = treeNode.Value.position;
            skillButton.Select(true);
            DrawSkills(treeNode, skillButton.GetRectTransform());
            _uIController.Intialize(_skills);
        }

        private void DrawSkills(TreeNode<Skill> node, RectTransform parentTransform)
        {
            for (int index = 0; index < node.Childrens.Count; index++)
            {
                SkillButton skill;
                if (!node.Childrens[index].Value.isInitialized)
                {
                    skill = Instantiate(_skillPrefab, _content);
                    _skills.Add(skill);
                    skill.SubscribeButton(OnSelectSkill);
                    skill.gameObject.name = node.Childrens[index].Value.name;
                    node.Childrens[index].Value.rectTransform = skill.GetRectTransform();
                    skill.Initialize(node.Childrens[index].Value.name, node.Childrens[index].Value.isLearned, node.Childrens[index]);
                    skill.GetRectTransform().localPosition = node.Childrens[index].Value.position;
                    node.Childrens[index].Value.isInitialized = true;
                }
                var lineTransform = Instantiate(_linePrefab, _linesTransform);
                float y = Vector3.Distance(node.Childrens[index].Value.rectTransform.localPosition, parentTransform.localPosition);
                Vector2 position = Vector3.Lerp(node.Childrens[index].Value.rectTransform.localPosition, parentTransform.localPosition, 0.5f);
                lineTransform.sizeDelta = new Vector2(lineTransform.sizeDelta.x, y);
                lineTransform.localPosition = position;
                lineTransform.gameObject.name = "line " + node.Value.name + "  " + node.Childrens[index].Value.name;
                float a = node.Childrens[index].Value.rectTransform.localPosition.y - parentTransform.localPosition.y;
                float b = node.Childrens[index].Value.rectTransform.localPosition.x - parentTransform.localPosition.x;
                float angle = Mathf.Atan(a / b);
                lineTransform.Rotate(Vector3.back, 90 - angle * 180 / Mathf.PI);
                DrawSkills(node.Childrens[index], node.Childrens[index].Value.rectTransform);
            }
        }

        private void OnSelectSkill(SkillButton button)
        {
            if (selected!=null)
            {
                selected.Select(false);
            }
            selected = button;
        }

        private void OnDestroy()
        {
            if (_skills!=null)
            {
                foreach (var skill in _skills)
                {
                    skill.Unsubscribe(OnSelectSkill);
                }
            }
        }
    }
}
