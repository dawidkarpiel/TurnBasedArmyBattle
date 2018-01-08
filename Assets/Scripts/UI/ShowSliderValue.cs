using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSliderValue : MonoBehaviour {

	Slider parent;
	Text text;
	void Start ()
	{
		parent = gameObject.GetComponentInParent<Slider>();
		text = gameObject.GetComponent<Text>();

		parent.onValueChanged.AddListener(delegate {OnValueChange(); });
	}
	
	public void OnValueChange()
	{
		text.text = parent.value.ToString();
	}
}
