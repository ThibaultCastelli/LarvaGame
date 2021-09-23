using System.Collections;
using UnityEngine;

namespace EasingTC
{
    public class EasingRotation : EasingBase
    {
        #region Variables
        public bool useAnotherStartValue;
        public bool addRotation;

        public Vector3 startRot;
        public Vector3 endRot;
        public Vector3 addRot;

        Vector3 defaultStartRot;
        Vector3 newStartRot;

        Vector3 newEndRot;
        #endregion

        #region Animation Choice
        protected override void Awake()
        {
            base.Awake();

            // Select the animation and intialize default values
            animationToPlay = EaseRot;

            if (useAnotherStartValue)
                defaultStartRot = startRot;
            else
                defaultStartRot = transform.rotation.eulerAngles;

            newStartRot = defaultStartRot;

            if (addRotation)
                newEndRot = defaultStartRot + addRot;
            else
                newEndRot = endRot;

            // Select which special ease function will be used
            if (animationType == AnimationType.SpecialEase)
            {
                switch (specialEaseType)
                {
                    case SpecialEase.EaseInBack:
                        animationToPlay = EaseInBackRot;
                        break;

                    case SpecialEase.EaseOutBack:
                        animationToPlay = EaseOutBackRot;
                        break;

                    case SpecialEase.EaseOutBounce:
                        animationToPlay = EaseOutBounceRot;
                        break;
                }
            }
        }
        #endregion

        #region Functions
        public override void PlayAnimationInOut()
        {
            if (addRotation)
                newEndRot = newEndRot == defaultStartRot + addRot ? defaultStartRot : defaultStartRot + addRot;
            else
                newEndRot = newEndRot == endRot ? defaultStartRot : endRot;
            newStartRot = transform.rotation.eulerAngles;

            base.PlayAnimationInOut();
        }

        IEnumerator EaseRot()
        {
            Quaternion startRot = Quaternion.Euler(newStartRot);
            Quaternion endRot = Quaternion.Euler(newEndRot);

            while (true)
            {
                transform.rotation = Quaternion.Lerp(startRot, endRot, easeFunc(elapsedTime / duration));

                if (elapsedTime == duration)
                {
                    _isInTransition = false;
                    yield break;
                }

                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime += Time.deltaTime, 0, duration);
            }
        }

        IEnumerator EaseInBackRot()
        {
            Vector3 endRot = newEndRot - newStartRot;

            while (true)
            {
                t = elapsedTime / duration;

                transform.rotation = Quaternion.Euler(endRot * t * t * ((s + 1) * t - s) + newStartRot);

                if (elapsedTime == duration)
                {
                    _isInTransition = false;
                    yield break;
                }

                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime += Time.deltaTime, 0, duration);
            }
        }

        IEnumerator EaseOutBackRot()
        {
            Vector3 endRot = newEndRot - newStartRot;

            while (true)
            {
                t = elapsedTime / duration - 1;

                transform.rotation = Quaternion.Euler(endRot * (t * t * ((s + 1) * t + s) + 1) + newStartRot);

                if (elapsedTime == duration)
                {
                    _isInTransition = false;
                    yield break;
                }

                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime += Time.deltaTime, 0, duration);
            }
        }

        IEnumerator EaseOutBounceRot()
        {
            Vector3 endRot = newEndRot - newStartRot;

            while (true)
            {
                t = elapsedTime / duration;

                if (t < (1 / 2.75f))
                    transform.rotation = Quaternion.Euler(endRot * (7.5625f * t * t) + newStartRot);

                else if (t < (2 / 2.75f))
                {
                    t -= (1.5f / 2.75f);

                    transform.rotation = Quaternion.Euler(endRot * (7.5625f * (t) * t + .75f) + newStartRot);
                }
                else if (t < (2.5 / 2.75))
                {
                    t -= (2.25f / 2.75f);

                    transform.rotation = Quaternion.Euler(endRot * (7.5625f * (t) * t + .9375f) + newStartRot);
                }
                else
                {
                    t -= (2.625f / 2.75f);

                    transform.rotation = Quaternion.Euler(endRot * (7.5625f * (t) * t + .984375f) + newStartRot);
                }

                if (elapsedTime == duration)
                {
                    _isInTransition = false;
                    yield break;
                }

                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime += Time.deltaTime, 0, duration);
            }
        }
        #endregion
    }
}
