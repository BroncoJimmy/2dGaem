using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunController : MonoBehaviour
{
    // [SerializeField] GameObject player;
    [SerializeField] Camera mainCam;
    [SerializeField] SpriteRenderer playerRenderer;
    
    Vector2 mousePos;
    public Vector3 up = new Vector3(0, 0, 1);
    
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
                return  new Vector2(-1f, 1f);
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

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    private void FixedUpdate()
    {
        // Converts mouse position to Vector2 coordinates on the screen
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Creates a Vector that points from gun to mouse. Limits arc with limitVector function.
        //Globals.lookDirection = limitVector(mousePos - new Vector2(transform.position.x, transform.position.y));

    }

    private void LateUpdate()
    {
        // Gun tracks player location
        
        

        // Calculates rot angle to point to mouse using arctangent and converting to degrees.
        float pointAngle = Mathf.Atan2(Globals.lookDirection.y, Globals.lookDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(pointAngle, up);


        // Didn't work 
        /*Set the Quaternion rotation from the GameObject's position to the mouse position
        rotAngle.SetFromToRotation(transform.position, mousePos);

        //Rotate the GameObject towards the mouse position
        transform.rotation = rotAngle * transform.rotation;
        */
    }

    
}
