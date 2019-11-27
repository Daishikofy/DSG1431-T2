using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterfaceInteractiveObject : MonoBehaviour
{
    // Start is called before the first frame update
    protected abstract void Start();

    // Update is called once per frame
    protected abstract void Update();

	public abstract void onInteraction(Player player);
}
