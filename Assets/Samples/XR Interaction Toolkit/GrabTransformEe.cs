using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class GrabTransformEe : XRGeneralGrabTransformer
{
	public override void Process(XRGrabInteractable grabInteractable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
	{
		Debug.Log("umin pro");
		base.Process(grabInteractable, updatePhase, ref targetPose, ref localScale);
	}

	public override void OnGrab(XRGrabInteractable grabInteractable)
	{
		Debug.Log("umin on");
		base.OnGrab(grabInteractable);
	}
}
