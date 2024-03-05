using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSpellscry : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    void Start(){
        Vector2 origin = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);

        //Cursor.SetCursor(cursorTexture, origin, CursorMode.Auto);
    }

    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
            transform.position = hit.point;
        }
    }
}
