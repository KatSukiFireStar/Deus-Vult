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
	
	[SerializeField] 
	private IntEventSO maxLifeEvent;

	public float LifePoint
	{
		get => lifePoint;
		set => lifePoint = value;
	}
	
	private void Awake()
	{
		AddSuscribe();
		life.Value = (int)lifePoint;
	}

	public void AddSuscribe()
	{
		if (life != null)
			life.PropertyChanged += LifeOnPropertyChanged;
	}

	private void OnDestroy()
	{
		if (life != null)
			life.PropertyChanged -= LifeOnPropertyChanged;
	}

	private void Start()
	{
	}

	private void LifeOnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		GenericEventSO<int> s = (GenericEventSO<int>)sender;

		float percent = (float)s.Value / lifePoint ;

		if (gameObject.CompareTag("PlayerLife"))
		{
			percent = (float)s.Value / maxLifeEvent.Value;
			transform.parent.localScale = new Vector3(maxLifeEvent.Value * 2f, transform.parent.localScale.y, transform.parent.localScale.z);
			transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector3(60+ (transform.parent.localScale.x / 2f), transform.parent.GetComponent<RectTransform>().anchoredPosition.y, 0);
		}

		transform.localScale = new Vector3(percent, transform.localScale.y, transform.localScale.z);
		transform.localPosition = new Vector3(-(1-percent)*0.5f, transform.localPosition.y, transform.localPosition.z);
	}
}