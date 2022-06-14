using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Skills
{
    public class SkillsTree:MonoBehaviour
    {
        TreeNode<Skill> skills;

        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            skills = new TreeNode<Skill>(new Skill("база", 0, new Vector3(0,-200), true,true));
            var node2=skills.AddChild(new Skill("2", 2,new Vector3(100,-300)));
            var node1 = skills.AddChild(new Skill("1", 1, new Vector3(-100, -100)));
            var node8 = skills.AddChild(new Skill("8", 2, new Vector3(0, -100)));
            var node4 = skills.AddChild(new Skill("4", 3, new Vector3(-100, -300)));
            var node9 = skills.AddChild(new Skill("9", 2, new Vector3(100, -200)));

            var node5 = node4.AddChild(new Skill("5", 1, new Vector3(-200,-300)));
            var node6 = node4.AddChild(new Skill("6", 2, new Vector3(-100,-400)));
            var skill10 = new Skill("10", 3, new Vector3(100, -100));
           var skill10Node= node8.AddChild(skill10);
            node9.AddExistingNode(skill10Node);
            
            node2.AddChild(new Skill("3", 2, new Vector3(200,-400)));
            var skill7 = new Skill("7", 2, new Vector3(-200,-400));
            var skill7Node = node5.AddChild(skill7);
            node6.AddExistingNode(skill7Node);
        }     

        public TreeNode<Skill> GetSkills()
        {
            return skills;
        }

        public bool TryFindPathToMainNode(TreeNode<Skill> skill)
        {
            skill.Value.marked = true;
            List<TreeNode<Skill>> skillsForCheck = new List<TreeNode<Skill>>();
            skillsForCheck.AddRange(skill.Childrens);
            skillsForCheck.AddRange(skill.Parents);
            bool found = true;
            foreach (var sk in skillsForCheck)
            {
                if (!sk.Value.isLearned)
                {
                    continue;
                }
                if (!FindMainNode(sk))
                {
                    found = false;
                    break;
                }
            }
            RemoveMarks(skill);
            if (found)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void RemoveMarks(TreeNode<Skill> skill)
        {
            foreach (var children in skill.Childrens)
            {
                if (children.Value.marked)
                {
                    children.Value.marked = false;
                    RemoveMarks(children);
                }
            }
            foreach (var parent in skill.Parents)
            {
                if (parent.Value.marked)
                {
                    parent.Value.marked = false;
                    RemoveMarks(parent);
                }
            }
        }

        private bool FindMainNode(TreeNode<Skill> skill)
        {
            if (skill.Value.mainNode)
            {
                return true;
            }
            if (FindMainNodeInList(skill.Childrens))
            {
                return true;
            }
            if (FindMainNodeInList(skill.Parents))
            {
                return true;
            }
            return false;
        }

        private bool FindMainNodeInList(List<TreeNode<Skill>> list)
        {
            foreach (var children in list)
            {
                if (children.Value.marked)
                {
                    continue;
                }
                if (children.Value.mainNode == true)
                {
                    return true;
                }
                else
                {
                    if (children.Value.isLearned)
                    {
                        children.Value.marked = true;
                        if (FindMainNode(children))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public int ForgetSkills()
        {
            return ForgetSkills(skills);
        }
        private int ForgetSkills(TreeNode<Skill> node)
        {
            int points = 0;
            foreach (var children in node.Childrens)
            {
                if (children.Value.isLearned)
                {
                    if (!children.Value.mainNode)
                    {
                        children.Value.isLearned = false;
                    }
                    points += children.Value.price;
                    points+=ForgetSkills(children);
                }
            }
            foreach (var parent in node.Parents)
            {
                if (parent.Value.isLearned)
                {
                    if (!parent.Value.mainNode)
                    {
                        parent.Value.isLearned = false;
                    }
                    points += parent.Value.price;
                    points += ForgetSkills(parent);
                }
            }
            return points;
        }
    }
}
