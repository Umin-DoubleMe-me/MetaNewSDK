using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCode : MonoBehaviour
{
	public static TestCode instance;

	[SerializeField] private InputActionProperty select;
	[SerializeField] private OVRSpatialAnchor _targetObj;

	private SpatialAnchorLoader spatialAnchorLoader;

	public void Awake()
	{
		instance = this;
		spatialAnchorLoader = new SpatialAnchorLoader();

		ttt();
	}

	public async Task ttt()
	{
		Debug.Log("Umin 시작");

		await Task.Delay(3000);

		Debug.Log("Umin 생성");
		OVRSpatialAnchor gg = Instantiate(_targetObj.gameObject).GetComponent<OVRSpatialAnchor>();
		spatialAnchorLoader.Init(gg);

		await Task.Delay(3000);
		Debug.Log("Umin 저장");
		gg.gameObject.GetComponent<SpaceAnchor>().OnSaveLocalButtonPressed();
	}


	public void Update()
	{
		var ss = select.action.ReadValue<float>();

		if (ss != 0)
		{
			//Debug.Log("Click");
		}
	}

	public void LoadButton()
	{
		spatialAnchorLoader.LoadAnchorsByUuid();
	}

	public void ZeroGenButton()
	{
		GameObject zeroPosObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		zeroPosObj.transform.position = Vector3.zero;
		zeroPosObj.transform.rotation = Quaternion.identity;
		zeroPosObj.transform.localScale = Vector3.one * 0.1f;
	}
}



public class SpatialAnchorLoader
{
	private OVRSpatialAnchor _targetObj;

	public void Init(OVRSpatialAnchor targetObj)
	{
		_targetObj = targetObj;
	}

	public void LoadAnchorsByUuid()
	{
		// Get number of saved anchor uuids
		if (!PlayerPrefs.HasKey(SpaceAnchor.NumUuidsPlayerPref))
		{
			PlayerPrefs.SetInt(SpaceAnchor.NumUuidsPlayerPref, 0);
		}

		var playerUuidCount = PlayerPrefs.GetInt(SpaceAnchor.NumUuidsPlayerPref);
		if (playerUuidCount == 0)
			return;

		var uuids = new Guid[playerUuidCount];
		for (int i = 0; i < playerUuidCount; ++i)
		{
			var uuidKey = "uuid" + i;
			var currentUuid = PlayerPrefs.GetString(uuidKey);

			uuids[i] = new Guid(currentUuid);
		}

		Load(new OVRSpatialAnchor.LoadOptions
		{
			Timeout = 0,
			StorageLocation = OVRSpace.StorageLocation.Local,
			Uuids = uuids
		});
	}

	private void Load(OVRSpatialAnchor.LoadOptions options) => OVRSpatialAnchor.LoadUnboundAnchors(options, anchors =>
	{
		if (anchors == null)
		{
			return;
		}

		foreach (var anchor in anchors)
		{
			if (anchor.Localized)
			{
				OnLocalized(anchor, true);
			}
			else if (!anchor.Localizing)
			{
				anchor.Localize(OnLocalized);
			}
		}
	});

	private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
	{
		if (!success)
		{
			Debug.LogError($"{unboundAnchor} Localization failed!");
			return;
		}

		var pose = unboundAnchor.Pose;
		_targetObj.gameObject.transform.position = pose.position;
		_targetObj.gameObject.transform.rotation = pose.rotation;
		var spatialAnchor = _targetObj;
		unboundAnchor.BindTo(spatialAnchor);
	}

}
