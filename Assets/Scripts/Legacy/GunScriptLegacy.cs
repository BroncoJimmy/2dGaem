using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScriptLegacy : MonoBehaviour
{

    [SerializeField] Camera mainCam;
    SpriteRenderer playerRenderer;


    Vector2 mousePos;

    [SerializeField] float offset;
    float xOffset;
    float yOffset;


    Vector3 up = new Vector3(0,0,1);

     
    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = Globals.player.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        

        

    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        
        // Updates controller's position so that they are aligned.
        transform.position = Globals.player.transform.position;


        // Converts mouse position to Vector2 coordinates on the screen
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Creates a Vector that points from gun to mouse. Limits arc with limitVector function.
        Globals.lookDirection = limitVector(mousePos - new Vector2(Globals.player.transform.position.x, Globals.player.transform.position.y));

        // Calculates rot angle to point to mouse using arctangent and converting to degrees.
        float pointAngle = Mathf.Atan2(Globals.lookDirection.y, Globals.lookDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(pointAngle, up);


        xOffset = ((int)(Globals.lookDirection.x * (offset / Globals.lookDirection.magnitude) * 100) ) / 100f;
        yOffset = ((int)(Globals.lookDirection.y * (offset / Globals.lookDirection.magnitude) * 100) ) / 100f;

        //transform.position = Globals.player.transform.position + new Vector3(0.14343f, 0.152542f, -2);
        
        if (Globals.currentPlayerAnim.Equals("Player_WalkD") || Globals.currentPlayerAnim.Equals("Player_IdleD"))
        {
            transform.position = Globals.player.transform.position + new Vector3(xOffset, yOffset, -2);
        }
        else
        {
            transform.position = Globals.player.transform.position + new Vector3(xOffset, yOffset, 1);
        }

    }

    // This method limits the gun's angle based on where the player is facing.
    public Vector2 limitVector(Vector2 input)
    {
        Globals.mouseRatio = input.y / input.x;

        if (Globals.currentPlayerAnim.Equals("Player_WalkD") || Globals.currentPlayerAnim.Equals("Player_IdleD"))
        {
            if (Globals.mouseRatio < 1 && input.x < 0)
            {
                return new Vector2(-1f, -1f);
            }
            if (Globals.mouseRatio > -1 && input.x > 0)
            {
                return new Vector2(1f, -1f);
            }
        }
        else if (Globals.currentPlayerAnim.Equals("Player_WalkU") || Globals.currentPlayerAnim.Equals("Player_IdleU"))
        {
            if (Globals.mouseRatio < 1 && input.x > 0)
            {
                return new Vector2(1f, 1f);
            }
            if (Globals.mouseRatio > -1 && input.x < 0)
            {
                return new Vector2(-1f, 1f);
            }
        }
        else if ((Globals.currentPlayerAnim.Equals("Player_WalkX") || Globals.currentPlayerAnim.Equals("Player_IdleX")) && playerRenderer.flipX)
        {
            if (Mathf.Abs(Globals.mouseRatio) > 1 || input.x > 0)
            {
                if (input.y < 0)
                {
                    return new Vector2(-1f, -1f);
                }
                else
                {
                    return new Vector2(-1f, 1f);
                }

            }
        }
        else if ((Globals.currentPlayerAnim.Equals("Player_WalkX") || Globals.currentPlayerAnim.Equals("Player_IdleX")))
        {
            if (Mathf.Abs(Globals.mouseRatio) > 1 || input.x < 0)
            {
                if (input.y < 0)
                {
                    return new Vector2(1f, -1f);
                }
                else
                {
                    return new Vector2(1f, 1f);
                }

            }

        }
        return input;
    }
}
