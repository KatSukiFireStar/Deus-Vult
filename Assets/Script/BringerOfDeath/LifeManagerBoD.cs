using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;
// ReSharper disable InconsistentNaming

public class LifeManagerBoD : MonoBehaviour
{
    [SerializeField] private int life = 200;
    
    [SerializeField] 
    private IntEventSO lifeEvent;
	
    public IntEventSO LifeEvent
    {
        get => lifeEvent;
        set => lifeEvent = value;
    }
	
    [SerializeField] 
    private BoolEventSO takeDamageEventSO;
	
    public BoolEventSO TakeDamageEventSO
    {
        get => takeDamageEventSO;
        set => takeDamageEventSO = value;
    }

    [SerializeField]
    private BoolEventSO deadEventSO;
    
    
    // Start is called before the first frame update
    void Start()
    {
        lifeEvent.Value = life;
        takeDamageEventSO.Value = false;
        
        // Subscribing
        lifeEvent.PropertyChanged += LifeEventOnPropertyChanged;
    }
    

    private void LifeEventOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        GenericEventSO<int> s = (GenericEventSO<int>)sender;
        takeDamageEventSO.Value = true;
        if (s.Value <= 0)
        {
            deadEventSO.Value = true; 
            s.Value = 0;
        }
    }
}
