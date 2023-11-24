using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System.Text;

/// <summary>
/// Specific functionality for spawned anchors
/// </summary>
[RequireComponent(typeof(OVRSpatialAnchor))]
public class SpaceAnchor : MonoBehaviour
{
	public const string NumUuidsPlayerPref = "numUuids";

	private OVRSpatialAnchor _spatialAnchor;

	#region Monobehaviour Methods

	private void Awake()
	{
		_spatialAnchor = GetComponent<OVRSpatialAnchor>();
	}

	private IEnumerator Start()
	{
		while (_spatialAnchor && _spatialAnchor.PendingCreation)
		{
			yield return null;
		}

		if (!_spatialAnchor)
		{
			Destroy(this.gameObject);
		}
	}

	#endregion // MonoBehaviour Methods

	#region UI Event Listeners

	/// <summary>
	/// UI callback for the anchor menu's Save button
	/// </summary>
	public void OnSaveLocalButtonPressed()
	{
		if (!_spatialAnchor) return;

		Debug.Log("Umin On");
		_spatialAnchor.Save((anchor, success) =>
		{
			Debug.Log("Umin : " + success);
			if (!success) return;

			SaveUuidToPlayerPrefs(anchor.Uuid);
		});

		Debug.Log("Umin Done");
	}

	void SaveUuidToPlayerPrefs(Guid uuid)
	{
		// Write uuid of saved anchor to file
		if (!PlayerPrefs.HasKey(NumUuidsPlayerPref))
		{
			PlayerPrefs.SetInt(NumUuidsPlayerPref, 0);
		}

		int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPref);
		PlayerPrefs.SetString("uuid" + playerNumUuids, uuid.ToString());
		PlayerPrefs.SetInt(NumUuidsPlayerPref, ++playerNumUuids);
	}

	#endregion // UI Event Listeners
}
