using UnityEngine;

namespace EventSystem.SO
{
    [CreateAssetMenu(fileName = "RespawnTriggerSO", menuName = "Trigger/RespawnTriggerSO")]
    public class RespawnTriggerSO : GenericTriggerSO<Respawn> 
    {
        public override void Trigger()
        {
            GenericEventSO<Respawn> e =  (GenericEventSO<Respawn>)EventSO;
            e.Value = modifier;
        }
    }
}