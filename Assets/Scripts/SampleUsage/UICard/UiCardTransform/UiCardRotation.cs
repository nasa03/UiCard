﻿using UnityEngine;

namespace Tools.UI.Card
{
    public class UiCardRotation : UiCardBaseTransform
    {
        public UiCardRotation(IUiCard handler) : base(handler)
        {
        }

        protected override float Threshold => 0.05f;

        //--------------------------------------------------------------------------------------------------------------

        public override void Execute(Vector3 euler, float speed, float delay = 0)
        {
            IsOperating = true;
            Target = euler;
            Speed = speed;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Finish()
        {
            Handler.transform.eulerAngles = Target;
            IsOperating = false;
            OnArrive?.Invoke();
        }

        protected override void KeepExecution()
        {
            var current = Handler.transform.rotation;
            var amount = Speed * Time.deltaTime;
            var rotation = Quaternion.Euler(Target);
            var newRotation = Quaternion.RotateTowards(current, rotation, amount);
            Handler.transform.rotation = newRotation;
        }

        protected override bool CheckFinalState()
        {
            var distance = Target - Handler.transform.eulerAngles;
            var smallerThanLimit = distance.magnitude <= Threshold;
            var equals360 = (int) distance.magnitude == 360;
            var isFinal = smallerThanLimit || equals360;
            return isFinal;
        }

        //--------------------------------------------------------------------------------------------------------------
    }
}