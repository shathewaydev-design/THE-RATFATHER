using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PokerBossAnimationController : MonoBehaviour
{
    //This script has one job: control which sprite the boss displays.
    //We should keep animation logic separate from the QTE manager. 
    //The QTE manager determines what attack is happening, 
    // and the animation controller determines what the player sees.

    [SerializeField] private SpriteRenderer pokerBossSpriteRenderer;//poker boss sprite
    [SerializeField] private Sprite idleSprite;//Default boss sprite
    [SerializeField] private Sprite currentTellSprite;//Current attack's tell sprite

    private void PlayTell(QTEData data)
    {
        //Changes boss sprite to the selected QTE's tell 
    }
    private void ReturnToIdle()
    {
        //Restores default sprite 
    }
    private void PlayBossDamagedAnimation()
    {
        //Optional damaged boss sprite
    }
    private void PlayBossDefeatAnimation() 
    {
        //Optional final defeated sprite
    }
    private void PlayVictoryAnimation() 
    {
        //Optional animation if the boss wins 
    }



}
