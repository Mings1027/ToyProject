// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerAnimationController : MonoBehaviour
// {
//     [SerializeField] PlayerController playerController;

//     public Animator anim;
//     public SpriteRenderer spriteRenderer;
//     //walk animation list
//     public List<Sprite> nSprites;
//     public List<Sprite> neSprites;
//     public List<Sprite> eSprites;
//     public List<Sprite> seSprites;
//     public List<Sprite> sSprites;
//     //idle animation list
//     public List<Sprite> inSprites;
//     public List<Sprite> ineSprites;
//     public List<Sprite> ieSprites;
//     public List<Sprite> iseSprites;
//     public List<Sprite> isSprites;
//     //roll animation list
//     public List<Sprite> rnSprites;
//     public List<Sprite> rneSprites;
//     public List<Sprite> reSprites;
//     public List<Sprite> rseSprites;
//     public List<Sprite> rsSprites;

//     public float frameRate;
//     float idleTime;
//     public List<Sprite> moveSprites = null;
//     public List<Sprite> idleSprites = null;
//     public List<Sprite> rollSprites = null;
//     List<Sprite> selectedSprites = null;

//     Vector2 move;

//     public float posX, posY;

//     //Flash
//     private Material normalMaterial;
//     [SerializeField] Material flashMaterial;
//     [SerializeField] float flashDuration;
//     private Coroutine flashRoutine;

//     private void Awake()
//     {
//         moveSprites = sSprites;
//         idleSprites = isSprites;
//         rollSprites = rsSprites;
//         normalMaterial = spriteRenderer.material;
//     }
//     private void Update()
//     {
//         posX = playerController.mousePos.x - transform.position.x;
//         posY = playerController.mousePos.y - transform.position.y;
//         //move = playerController.move;
//         SpriteFlip();
//         // SetSprites();
//         // GetIdleSprites();
//         // GetRollSprites();
//         LookAtTheMouse();
//         RollAnimation();
//     }

//     private void SpriteFlip()
//     {
//         if (move.x < 0 || posX < 0) spriteRenderer.flipX = true;
//         else if (move.x > 0 || posX > 0) spriteRenderer.flipX = false;
//     }

//     private void SetSprites()
//     {
//         selectedSprites = GetMoveSprites();
//         PlayAnimation(selectedSprites);
//     }

//     private List<Sprite> GetMoveSprites()
//     {
//         if (move.y > 0)
//         {
//             if (Mathf.Abs(move.x) > 0)
//                 moveSprites = neSprites;
//             else
//                 moveSprites = nSprites;
//         }
//         else if (move.y < 0)
//         {
//             if (Mathf.Abs(move.x) > 0)
//                 moveSprites = seSprites;
//             else
//                 moveSprites = sSprites;
//         }

//         return moveSprites;
//     }

//     private void GetIdleSprites()
//     {
//         if (playerController.isDodge) return;
//         if (move.sqrMagnitude == 0 && selectedSprites != null)
//         {
//             if (selectedSprites == neSprites) idleSprites = ineSprites;
//             else if (selectedSprites == nSprites) idleSprites = inSprites;
//             else if (selectedSprites == eSprites) idleSprites = ieSprites;
//             else if (selectedSprites == seSprites) idleSprites = iseSprites;
//             else if (selectedSprites == sSprites) idleSprites = isSprites;

//             //PlayAnimation(idleSprites);
//         }
//     }

//     private void GetRollSprites()
//     {
//         if (move.sqrMagnitude != 0 && selectedSprites != null)
//         {
//             if (selectedSprites == neSprites) rollSprites = rneSprites;
//             else if (selectedSprites == nSprites) rollSprites = rnSprites;
//             else if (selectedSprites == eSprites) rollSprites = reSprites;
//             else if (selectedSprites == seSprites) rollSprites = rseSprites;
//             else if (selectedSprites == sSprites) rollSprites = rsSprites;
//         }
//         if (playerController.isDodge == true)
//         {
//             PlayAnimation(rollSprites);
//         }
//     }

//     private void LookAtTheMouse()
//     {
//         if (playerController.isDodge) return;
//         if (move.sqrMagnitude != 0)
//         {
//             if (posY > 0)
//             {
//                 if (posX > -1 && posX < 1) moveSprites = nSprites;
//                 else moveSprites = neSprites;
//             }
//             else
//             {
//                 if (posX > -1 && posX < 1) moveSprites = sSprites;
//                 else moveSprites = seSprites;
//             }
//             PlayAnimation(moveSprites);
//         }
//         else
//         {
//             if (posY > 0)
//             {
//                 if (posX > -1 && posX < 1) idleSprites = inSprites;
//                 else idleSprites = ineSprites;
//             }
//             else
//             {
//                 if (posX > -1 && posX < 1) idleSprites = isSprites;
//                 else idleSprites = iseSprites;
//             }
//             PlayAnimation(idleSprites);
//         }
//     }
//     private void RollAnimation()
//     {
//         if (move.y > 0)
//         {
//             if (Mathf.Abs(move.x) > 0) rollSprites = rneSprites;
//             else rollSprites = rnSprites;
//         }
//         else if (move.y < 0)
//         {
//             if (Mathf.Abs(move.x) > 0) rollSprites = rseSprites;
//             else rollSprites = rsSprites;
//         }
//         else rollSprites = reSprites;

//         if (playerController.isDodge)
//         {
//             PlayAnimation(rollSprites);
//         }
//     }

//     private void PlayAnimation(List<Sprite> animationList)
//     {
//         if (animationList != null && animationList.Count != 0)
//         {
//             float playTime = Time.time - idleTime;
//             int totalFrames = (int)(playTime * frameRate);
//             int frame = totalFrames % animationList.Count;
//             spriteRenderer.sprite = animationList[frame];
//         }
//         else idleTime = Time.time;

//     }

//     public void Flash()
//     {
//         if (flashRoutine != null) StopCoroutine(flashRoutine);
//         flashRoutine = StartCoroutine(FlashRoutine());
//     }
//     private IEnumerator FlashRoutine()
//     {
//         var wait = new WaitForSeconds(flashDuration);
//         spriteRenderer.material = flashMaterial;
//         yield return wait;
//         spriteRenderer.material = normalMaterial;
//         yield return wait;
//         spriteRenderer.material = flashMaterial;
//         yield return wait;
//         spriteRenderer.material = normalMaterial;
//         flashRoutine = null;
//     }






// }
