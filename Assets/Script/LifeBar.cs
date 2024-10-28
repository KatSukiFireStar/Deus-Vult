using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
	[SerializeField] 
	private IntEventSO life;

	
	private void Awake()
	{
		life.PropertyChanged += LifeOnPropertyChanged;
	}

	private void LifeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;

		float percent = (s.Value * 0.005f) ;

		transform.localScale = new Vector3(percent, transform.localScale.y, transform.localScale.z);
		transform.localPosition = new Vector3(-(1-percent)*0.5f, transform.localPosition.y, transform.localPosition.z);
	}
}