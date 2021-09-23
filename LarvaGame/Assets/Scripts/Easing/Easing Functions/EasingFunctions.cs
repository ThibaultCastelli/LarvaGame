using UnityEngine;

namespace EasingTC
{
    public static class EasingFunctions
    {
        #region Standard Eases
        public static float Linear(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            return Mathf.Lerp(start, end, animTime);
        }

        #region Quad
        public static float EaseInQuad(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return end * animTime * animTime + start;
        }
        public static float EaseOutQuad(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return -end * animTime * (animTime - 2) + start;
        }
        public static float EaseInOutQuad(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp(elapsedTime / duration / 0.5f, 0, 2);
            end -= start;
            if (animTime < 1) return end * 0.5f * animTime * animTime + start;
            animTime--;
            return -end * 0.5f * (animTime * (animTime - 2) - 1) + start;
        }
        #endregion

        #region Cubic
        public static float EaseInCubic(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return end * animTime * animTime * animTime + start;
        }
        public static float EaseOutCubic(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            animTime--;
            end -= start;
            return end * (animTime * animTime * animTime + 1) + start;
        }
        public static float EaseInOutCubic(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp(elapsedTime / duration / 0.5f, 0, 2);
            end -= start;
            if (animTime < 1) return end * 0.5f * animTime * animTime * animTime + start;
            animTime -= 2;
            return end * 0.5f * (animTime * animTime * animTime + 2) + start;
        }
        #endregion

        #region Quart
        public static float EaseInQuart(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return end * animTime * animTime * animTime * animTime + start;
        }
        public static float EaseOutQuart(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            animTime--;
            end -= start;
            return -end * (animTime * animTime * animTime * animTime - 1) + start;
        }
        public static float EaseInOutQuart(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp(elapsedTime / duration / 0.5f, 0, 2);
            end -= start;
            if (animTime < 1) return end * 0.5f * animTime * animTime * animTime * animTime + start;
            animTime -= 2;
            return -end * 0.5f * (animTime * animTime * animTime * animTime - 2) + start;
        }
        #endregion

        #region Quint
        public static float EaseInQuint(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return end * animTime * animTime * animTime * animTime * animTime + start;
        }
        public static float EaseOutQuint(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            animTime--;
            end -= start;
            return end * (animTime * animTime * animTime * animTime * animTime + 1) + start;
        }
        public static float EaseInOutQuint(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp(elapsedTime / duration / 0.5f, 0, 2);
            end -= start;
            if (animTime < 1) return end * 0.5f * animTime * animTime * animTime * animTime * animTime + start;
            animTime -= 2;
            return end * 0.5f * (animTime * animTime * animTime * animTime * animTime + 2) + start;
        }
        #endregion

        #region Sine
        public static float EaseInSine(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return -end * Mathf.Cos(animTime * (Mathf.PI * 0.5f)) + end + start;
        }
        public static float EaseOutSine(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return end * Mathf.Sin(animTime * (Mathf.PI * 0.5f)) + start;
        }
        public static float EaseInOutSine(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return -end * 0.5f * (Mathf.Cos(Mathf.PI * animTime) - 1) + start;
        }
        #endregion

        #region Expo
        public static float EaseInExpo(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return end * Mathf.Pow(2, 10 * (animTime - 1)) + start;
        }
        public static float EaseOutExpo(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return end * (-Mathf.Pow(2, -10 * animTime) + 1) + start;
        }
        public static float EaseInOutExpo(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp(elapsedTime / duration / 0.5f, 0, 2);
            end -= start;
            if (animTime < 1) return end * 0.5f * Mathf.Pow(2, 10 * (animTime - 1)) + start;
            animTime--;
            return end * 0.5f * (-Mathf.Pow(2, -10 * animTime) + 2) + start;
        }
        #endregion

        #region Circ
        public static float EaseInCirc(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            return -end * (Mathf.Sqrt(1 - animTime * animTime) - 1) + start;
        }
        public static float EaseOutCirc(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            animTime--;
            end -= start;
            return end * Mathf.Sqrt(1 - animTime * animTime) + start;
        }
        public static float EaseInOutCirc(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp(elapsedTime / duration / 0.5f, 0, 2);
            end -= start;
            if (animTime < 1) return -end * 0.5f * (Mathf.Sqrt(1 - animTime * animTime) - 1) + start;
            animTime -= 2;
            return end * 0.5f * (Mathf.Sqrt(1 - animTime * animTime) + 1) + start;
        }
        #endregion

        #endregion

        #region Special Eases
        public static float Spring(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            animTime = (Mathf.Sin(animTime * Mathf.PI * (0.2f + 2.5f * animTime * animTime * animTime)) * Mathf.Pow(1f - animTime, 2.2f) + animTime) * (1f + (1.2f * (1f - animTime)));
            return start + (end - start) * animTime;
        }

        #region Bounce
        public static float EaseOutBounce(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            if (animTime < (1 / 2.75f))
            {
                return end * (7.5625f * animTime * animTime) + start;
            }
            else if (animTime < (2 / 2.75f))
            {
                animTime -= (1.5f / 2.75f);
                return end * (7.5625f * (animTime) * animTime + .75f) + start;
            }
            else if (animTime < (2.5 / 2.75))
            {
                animTime -= (2.25f / 2.75f);
                return end * (7.5625f * (animTime) * animTime + .9375f) + start;
            }
            else
            {
                animTime -= (2.625f / 2.75f);
                return end * (7.5625f * (animTime) * animTime + .984375f) + start;
            }
        }
        #endregion

        #region Back
        public static float EaseInBack(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;
            float s = 1.70158f;
            return end * (animTime) * animTime * ((s + 1) * animTime - s) + start;
        }
        public static float EaseOutBack(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            float s = 1.70158f;
            end -= start;
            animTime = animTime - 1;
            return end * (animTime * animTime * ((s + 1) * animTime + s) + 1) + start;
        }
        public static float EaseInOutBack(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp(elapsedTime / duration / 0.5f, 0, 2);
            float s = 1.70158f;
            end -= start;
            if ((animTime) < 1)
            {
                s *= (1.525f);
                return end * 0.5f * (animTime * animTime * (((s) + 1) * animTime - s)) + start;
            }
            animTime -= 2;
            s *= (1.525f);
            return end * 0.5f * (animTime * animTime * (((s) + 1) * animTime + s) + 2) + start;
        }
        #endregion

        #region Elastic
        public static float EaseInElastic(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (animTime == 0) return start;

            if ((animTime /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (animTime -= 1)) * Mathf.Sin((animTime * d - s) * (2 * Mathf.PI) / p)) + start;
        }
        public static float EaseOutElastic(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (animTime == 0) return start;

            if ((animTime /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * animTime) * Mathf.Sin((animTime * d - s) * (2 * Mathf.PI) / p) + end + start);
        }
        public static float EaseInOutElastic(float start, float end, float elapsedTime, float duration)
        {
            float animTime = Mathf.Clamp01(elapsedTime / duration);
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (animTime == 0) return start;

            if ((animTime /= d * 0.5f) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (animTime < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (animTime -= 1)) * Mathf.Sin((animTime * d - s) * (2 * Mathf.PI) / p)) + start;
            return a * Mathf.Pow(2, -10 * (animTime -= 1)) * Mathf.Sin((animTime * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }
        #endregion

        #endregion
    }
}
