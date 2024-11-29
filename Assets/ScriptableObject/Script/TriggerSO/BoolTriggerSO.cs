using UnityEngine;

namespace EventSystem.SO
{
    [CreateAssetMenu(fileName = "BoolTriggerSO ", menuName = "Trigger/BoolTriggerSO ")]
    public class BoolTriggerSO : GenericTriggerSO<bool> 
    {
        public override void Trigger()
        {
            GenericEventSO<bool> e =  (GenericEventSO<bool>)EventSO;
            e.Value = modifier;
        }
    }
}