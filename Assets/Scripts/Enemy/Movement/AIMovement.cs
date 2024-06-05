using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGG.Move
{
    public class AIMovement : CharacterMovementBase
    {
        protected override void Update()
        {
            base.Update();

            UpdateGrvity();
        }


        private void UpdateGrvity()
        {
            verticalDirection.Set(0f,verticalSpeed,0f);
            control.Move(Time.deltaTime * verticalDirection);
        }
    }

}