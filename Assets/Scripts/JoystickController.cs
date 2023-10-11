using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public RectTransform joystickOutline;
    public RectTransform joystickButton;

    public float moveFactor;
    private Vector3 move;

    private bool canControlJoystick;
    private Vector3 tapPosition;

    public float moveSpeedy = 1f;
    // Start is called before the first frame update
    void Start()
    {
        HideJoystick();
    }

    public void TappedOnJoystick()
    {
        tapPosition = Input.mousePosition;
        joystickOutline.position = tapPosition;
        //ekrana dokunulduðunda algýlayacaðýmýz ve joystick ekranda belirecek
        ShowJoystick();
    }

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        canControlJoystick = true;
    }

    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControlJoystick = false;
        move = Vector3.zero;
    }

    public void ControlJoystick()
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - tapPosition;//butonu mouseu çektiðimiz yöne doðru çekicek

        float canvasYScale = GetComponentInParent<Canvas>().GetComponent<RectTransform>().localScale.y;
        float moveMagnitude = direction.magnitude * moveFactor * canvasYScale;

        float joystickOutlineHalfWidth = joystickOutline.rect.width / 2;
        float newWidth = joystickOutlineHalfWidth * canvasYScale;
        
        moveMagnitude = Mathf.Min(moveMagnitude, newWidth);
        
        move = direction.normalized * moveMagnitude;

        //float moveMagnitude = direction.magnitude * moveFactor / Screen.width;//hareket þiddeti belirledik
        //moveMagnitude = Mathf.Min(moveMagnitude, joystickOutline.rect.width / 4);//outlinenýn içine sýkýþtýrdýk


        Vector3 targetPos = tapPosition + move;
        joystickButton.position = targetPos;
        //joystick ile karakterimizi hareket ettireceðiz
        if(Input.GetMouseButtonUp(0))
        {
            HideJoystick();
        }
    }

    public Vector3 GetMovePosition()
    {
        return move * moveSpeedy;
    }

    // Update is called once per frame
    void Update()
    {
        if(canControlJoystick)
        {
            ControlJoystick();
        }
    }
}
