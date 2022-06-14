using Assets._Scripts.Skills;
using Assets._Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts
{
    public class PlayerSkills:MonoBehaviour
    {
        [SerializeField] UIController controller;
        [SerializeField] private SkillsTree _skillsTree;
        TreeNode<bool> learnedSkills;
        int points;
        int learnedPoints;

        private void Awake()
        {
            controller.workingClick += LearnPoint;
            controller.learningClick += TryLearnSkill;
            controller.forgetClick += TryForgetSkill;
            controller.forgetAllButtonsClick += ForgetAllSkills;
        }

        private void LearnPoint()
        {
            points++;
            controller.UpdatePoints(points);
        }

        private void TryLearnSkill(TreeNode<Skill> skill)
        {
            bool learnedSkillExist = false;
            foreach (var children in skill.Childrens)
            {
                if (children.Value.isLearned)
                {
                    learnedSkillExist = true;
                    break;
                }
            }

            foreach (var parent in skill.Parents)
            {
                if (parent.Value.isLearned)
                {
                    learnedSkillExist = true;
                    break;
                }
            }

            if (skill.Value.price<=points && !skill.Value.isLearned && learnedSkillExist)
            {
                skill.Value.isLearned = true;
                learnedPoints += skill.Value.price;
                points -= skill.Value.price;
                controller.UpdatePoints(points);
            }
        }

        private void TryForgetSkill(TreeNode<Skill> skill)
        {
            if (skill.Value.isLearned && _skillsTree.TryFindPathToMainNode(skill))
            {
                skill.Value.isLearned = false;
                points += skill.Value.price;
                learnedPoints -= skill.Value.price;
                controller.UpdatePoints(points);
            }
        }

        private void ForgetAllSkills()
        {
            _skillsTree.ForgetSkills();
            controller.UpdatePoints(points);
        }

        private void OnDestroy()
        {
            controller.workingClick -= LearnPoint;
            controller.learningClick -= TryLearnSkill;
            controller.forgetClick -= TryForgetSkill;
            controller.forgetAllButtonsClick -= ForgetAllSkills;
        }
    }
}
