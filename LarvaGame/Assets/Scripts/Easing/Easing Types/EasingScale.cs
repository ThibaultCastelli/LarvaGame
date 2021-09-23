using System.Collections;
using UnityEngine;

namespace EasingTC
{
    public class EasingScale : EasingBase
    {
        #region Variables
        public bool useAnotherStartValue;
        public bool addScale;

        public Vector3 startScale;
        public Vector3 endScale;
        public Vector3 addScl;

        Vector3 defaultStartScale;
        Vector3 newStartScale;

        Vector3 newEndScale;
        #endregion

        #region Animation Choice
        protected override void Awake()
        {
            base.Awake();

            // Select the animation and intialize default values
            animationToPlay = EaseScale;

            if (useAnotherStartValue)
                defaultStartScale = startScale;
            else
                defaultStartScale = transform.localScale;

            newStartScale = defaultStartScale;

            if (addScale)
                newEndScale = defaultStartScale + addScl;
            else
                newEndScale = endScale;

            // Select which special ease function will be used
            if (animationType == AnimationType.SpecialEase)
            {
                switch (specialEaseType)
                {
                    case SpecialEase.EaseInBack:
                        animationToPlay = EaseInBackScale;
                        break;

                    case SpecialEase.EaseOutBack:
                        animationToPlay = EaseOutBackScale;
                        break;

                    case SpecialEase.EaseOutBounce:
                        animationToPlay = EaseOutBounceScale;
                        break;
                }
            }
        }
        #endregion

        #region Functions
        public override void PlayAnimationInOut()
        {
            if (addScale)
                newEndScale = newEndScale == defaultStartScale + addScl ? defaultStartScale : defaultStartScale + addScl;
            else
                newEndScale = newEndScale == endScale ? defaultStartScale : endScale;
            newStartScale = transform.localScale;

            base.PlayAnimationInOut();
        }

        IEnumerator EaseScale()
        {
            while (true)
            {
                transform.localScale = Vector3.Lerp(newStartScale, newEndScale, easeFunc(elapsedTime / duration));

                if (elapsedTime == duration)
                {
                    _isInTransition = false;
                    yield break;
                }

                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime += Time.deltaTime, 0, duration);
            }
        }

        IEnumerator EaseInBackScale()
        {
            Vector3 endScale = newEndScale - newStartScale;

            while (true)
            {
                t = elapsedTime / duration;

                transform.localScale = endScale * t * t * ((s + 1) * t - s) + newStartScale;

                if (elapsedTime == duration)
                {
                    _isInTransition = false;
                    yield break;
                }

                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime += Time.deltaTime, 0, duration);
            }
        }

        IEnumerator EaseOutBackScale()
        {
            Vector3 endScale = newEndScale - newStartScale;

            while (true)
            {
                t = elapsedTime / duration - 1;

                transform.localScale = endScale * (t * t * ((s + 1) * t + s) + 1) + newStartScale;

                if (elapsedTime == duration)
                {
                    _isInTransition = false;
                    yield break;
                }

                yield return null;
                elapsedTime = Mathf.Clamp(elapsedTime += Time.deltaTime, 0, duration);
            }
        }

        IEnumerator EaseOutBounceScale()
        {
            Vector3 endScale = newEndScale - newStartScale;

            while (true)
            {
                t = elapsedTime / duration;

                if (t < (1 / 2.75f))
                    transform.localScale = endScale * (7.5625f * t * t) + newStartScale;

                else if (t < (2 / 2.75f))
                {
                    t -= (1.5f / 2.75f);

                    transform.localScale = endScale * (7.5625f * (t) * t + .75f) + newStartScale;
                }
                else if (t < (2.5 / 2.75))
                {
                    t -= (2.25f / 2.75f);

                    transform.localScale = endScale * (7.5625f * (t) * t + .9375f) + newStartScale;
                }
                else
                {
                    t -= (2.625f / 2.75f);

                    transform.localScale = endScale * (7.5625f * (t) * t + .984375f) + newStartScale;
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