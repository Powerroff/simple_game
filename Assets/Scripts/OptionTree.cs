using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionTree 
{
    public OptionNode[][] nodeTree;

    public OptionTree() {

    }


    public class OptionNode //class vs struct?? idk
    {
        public Option option;
        public OptionTree tree;

        OptionNode[] children;

        public OptionNode(Option option, OptionTree tree) {
            this.option = option;
            this.option.node = this;
            this.tree = tree;
            this.children = new OptionNode[3];
        }

        enum child { left, center, right };

        public Option[] getChildren() {
            Option[] childrenOpts = new Option[3];
            for (int i = 0; i < 3; i++)
                if (children[i] != null)
                    childrenOpts[i] = children[i].option;
            return childrenOpts;        
        }

        public List<Option> getChildrenList() {
            List<Option> childrenOpts = new List<Option>();
            foreach (OptionNode n in children)
                if (n != null)
                    childrenOpts.Add(n.option);
            return childrenOpts;
        }

            public void setChildren(OptionNode[] children) {
            if (children.Length != 3)
                Debug.Log("Setting children to incorrect length: " + children.Length);
            this.children = children;
        }
    }

    //This will be refactored
    public List<Option> getFirstLayer() { 
        List<Option> l = new List<Option>();
        l.AddRange(new Option[] { nodeTree[0][0].option, nodeTree[0][1].option });
        return l;
    }

    public static OptionTree defaultTree() {
        OptionTree ot = new OptionTree();
        ot.nodeTree = new OptionNode[][] { new OptionNode[2], new OptionNode[3], new OptionNode[5] };
        ot.nodeTree[0][0] = new OptionNode(Option.hackSlash(), ot);
        ot.nodeTree[0][1] = new OptionNode(Option.harvest(), ot);
        ot.nodeTree[1][0] = new OptionNode(Option.savageSlash(), ot);
        ot.nodeTree[1][1] = new OptionNode(Option.clearPath(), ot);
        ot.nodeTree[1][2] = new OptionNode(Option.skilledExploration(), ot);
        ot.nodeTree[2][0] = new OptionNode(Option.recklessAssault(), ot);
        ot.nodeTree[2][1] = new OptionNode(Option.swiftKill(), ot);
        ot.nodeTree[2][2] = new OptionNode(Option.layLand(), ot);
        ot.nodeTree[2][3] = new OptionNode(Option.rangerTactics(), ot);
        ot.nodeTree[2][4] = new OptionNode(Option.conquerWilderness(), ot);
        //Setup Children
        ot.nodeTree[0][0].setChildren(new OptionNode[] { ot.nodeTree[1][0], null, ot.nodeTree[1][1] });
        ot.nodeTree[0][1].setChildren(new OptionNode[] { ot.nodeTree[1][1], null, ot.nodeTree[1][2] });
        ot.nodeTree[1][0].setChildren(new OptionNode[] { ot.nodeTree[2][0], null, ot.nodeTree[2][1] });
        ot.nodeTree[1][1].setChildren(new OptionNode[] { ot.nodeTree[2][1], ot.nodeTree[2][2], ot.nodeTree[2][3] });
        ot.nodeTree[1][2].setChildren(new OptionNode[] { ot.nodeTree[2][3], null, ot.nodeTree[2][4] });


        return ot;
    }


}
