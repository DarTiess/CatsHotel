namespace Infrastructure.Input
{
    public class StandaloneInputService : InputService
    {
        public override float GetHorizontal
        {
            get
            {
               float hor = SimpleInput.GetAxis(HORIZONTAL);

                if (hor == 0)
                {
                    hor = UnityAxis(HORIZONTAL);

                }

                return hor;
            }
        }
        public override float GetVertical
        {
            get
            {
                float vert = SimpleInput.GetAxis(VERTICAL);

                if (vert == 0)
                {
                    vert = UnityAxis(VERTICAL);

                }

                return vert;
            }
        }

        private static float UnityAxis(string key)
        {
            return UnityEngine.Input.GetAxis(key);
        }
    }
}