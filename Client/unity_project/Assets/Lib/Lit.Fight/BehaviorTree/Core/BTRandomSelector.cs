using Lit.Unity;
namespace Lit.BT
{
    public class BTRandomSelector : BTNode {
        private BTNode runingNode = null;


        public override BTResult Tick()
        {
            if (children == null || children.Count == 0) return BTResult.Failure;
            if(runingNode != null)
            {
                var tmpRst = runingNode.Tick();
                if(tmpRst == BTResult.Running)
                {
                    return BTResult.Running;
                }
                if(tmpRst == BTResult.Success)
                {
                    runingNode = null;
                    return BTResult.Success;
                }
            }

            runingNode = null;

            children._Shuffle();
            BTResult result = BTResult.Failure;
            for (int i = 0; i < children.Count; i++)
            {
                BTResult tmpRst = children[i].Tick();
                if (tmpRst == BTResult.Running)
                {
                    runingNode = children[i];
                    result = tmpRst;
                    break;
                }
                if(tmpRst == BTResult.Success)
                {
                    result = tmpRst;
                    break;
                }
            }
            return result;


        }
    }
}

