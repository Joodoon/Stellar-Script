using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelescopeTrigger : MonoBehaviour
{
    private CircleCollider2D cc;
    [SerializeField] private SkyView skyView;
    private GameObject interactObj;
    private Image interactIcon;

    private bool isTriggered = false;

    private Cinemachine.CinemachineVirtualCamera telescopeCam;
    private Cinemachine.CinemachineVirtualCamera playerCam;


    void Start(){
        telescopeCam = GameObject.Find("SkyVCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        playerCam = GameObject.Find("PlayerVCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();

        interactObj = GameObject.Find("InteractIcon");
        interactIcon = interactObj.GetComponent<Image>();

        interactObj.SetActive(false);

        cc = GetComponent<CircleCollider2D>();
    }

    void Update(){
        interactIcon.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 5, 0));
        telescopeCam.transform.position = new Vector3(playerCam.transform.position.x, telescopeCam.transform.position.y, telescopeCam.transform.position.z);

        if (cc.IsTouchingLayers(LayerMask.GetMask("Player"))){
            // show interact icon above object
            interactObj.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E) && !isTriggered){
                triggerTelescope();
            }
            else if (Input.GetKeyDown(KeyCode.E) && isTriggered){
                exitTelescope();
            }
        }
        else{
            interactObj.SetActive(false);
        }
    }

    void triggerTelescope(){
        isTriggered = true;
        telescopeCam.Priority = 3;
        foreach (constellationEnum constellation in skyView.constellations){
            Debug.Log(constellation);
        }
    }

    void exitTelescope(){
        isTriggered = false;
        telescopeCam.Priority = 1;
    }
}
