using UnityEngine;

namespace EventSystem.SO
{
    [CreateAssetMenu(fileName = "IntTriggerSO", menuName = "Trigger/IntTriggerSO")]
    public class IntTriggerSO : GenericTriggerSO<int> 
    {
        public override void Trigger()
        {
            GenericEventSO<int> e =  (GenericEventSO<int>)EventSO;
            e.Value = modifier;
        }
    }
}