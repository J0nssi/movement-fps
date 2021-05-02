using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
	public string name { get; set; }
	public int frags { get; set; }
	public int deaths { get; set; }

	public Score(string name)
	{
		this.name = name;
		frags = 0;
		deaths = 0;
	}
}
