using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorShaderTween : MonoBehaviour
{

    public Renderer floorRenderer;
    public float currentFloorHologramPower;
    public float hologramPowerDelta;
    public float speed;

    public void ShowFloor()
    {
        StartCoroutine(ShowFloorLerp());
    }

    public IEnumerator ShowFloorLerp()
    {
        while (currentFloorHologramPower < 1.1f)
        {
            yield return new WaitForSeconds(1/speed);
            floorRenderer.material.SetFloat("_HologramPower", currentFloorHologramPower);
            currentFloorHologramPower += hologramPowerDelta;
        }

    }
    public IEnumerator HideFloorLerp()
    {
        while (currentFloorHologramPower > -0.1f)
        {
            yield return new WaitForSeconds(1/speed);
            floorRenderer.material.SetFloat("_HologramPower", currentFloorHologramPower);
            currentFloorHologramPower -= hologramPowerDelta;
        }

    }
}
