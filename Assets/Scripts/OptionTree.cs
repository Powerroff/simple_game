using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionTree 
{
    public OptionNode[][] nodeTree;

    public OptionTree() {

    }

    public struct OptionNode //class vs struct?? idk
    {
        public int layer;
        public int[] children;
        public Option option;

        public OptionNode(int layer, int[] children, Option option) {
            this.layer = layer;
            this.children = children;
            this.option = option;

        }
        
    }

    public List<OptionNode> getChildren(OptionNode n) {
        
        List<OptionNode> childrenList = new List<OptionNode>();
        if (n.children != null)
            foreach (int child in n.children) {
                childrenList.Add(nodeTree[n.layer + 1][child]);
            }
        return childrenList;
        
    }

    public List<OptionNode> getFirstLayer() {
        List<OptionNode> l = new List<OptionNode>();
        l.AddRange(nodeTree[0]);
        return l;
    }

    public static OptionTree defaultTree() {
        OptionTree ot = new OptionTree();
        ot.nodeTree = new OptionNode[][] { new OptionNode[2], new OptionNode[3], new OptionNode[5] };
        ot.nodeTree[0][0] = new OptionNode(0, new int[] { 0, 1 }, Option.hackSlash());
        ot.nodeTree[0][1] = new OptionNode(0, new int[] { 1, 2 }, Option.harvest());
        ot.nodeTree[1][0] = new OptionNode(1, new int[] { 0, 1 }, Option.savageSlash());
        ot.nodeTree[1][1] = new OptionNode(1, new int[] { 1, 2, 3 }, Option.clearPath());
        ot.nodeTree[1][2] = new OptionNode(1, new int[] { 3, 4 }, Option.skilledExploration());
        ot.nodeTree[2][0] = new OptionNode(2, null, Option.recklessAssault());
        ot.nodeTree[2][1] = new OptionNode(2, null, Option.swiftKill());
        ot.nodeTree[2][2] = new OptionNode(2, null, Option.layLand());
        ot.nodeTree[2][3] = new OptionNode(2, null, Option.rangerTactics());
        ot.nodeTree[2][4] = new OptionNode(2, null, Option.conquerWilderness());
        return ot;
    }


}
