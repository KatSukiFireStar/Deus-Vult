using System;
using System.ComponentModel;
using EventSystem.SO;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
	[SerializeField] 
	private IntEventSO life;

	public IntEventSO Life
	{
		get => life;
		set => life = value;
	}

	[SerializeField]
	private float lifePoint;

	public float LifePoint
	{
		get => lifePoint;
		set => lifePoint = value;
	}
	
	private void Awake()
	{
		AddSuscribe();
	}

	public void AddSuscribe()
	{
		if (life != null)
			life.PropertyChanged += LifeOnPropertyChanged;
	}

	private void Start()
	{
		life.Value = life.Value;
	}

	private void LifeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;

		float percent = (float)s.Value / lifePoint ;

		transform.localScale = new Vector3(percent, transform.localScale.y, transform.localScale.z);
		transform.localPosition = new Vector3(-(1-percent)*0.5f, transform.localPosition.y, transform.localPosition.z);
	}
}