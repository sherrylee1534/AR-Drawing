﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScreenshotFlash : MonoBehaviour
{
	void Start ()
    {
		Destroy(gameObject, 0.15f);
		Debug.Log("destroy");
	}
}
