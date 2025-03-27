using System.Collections.Generic;
using DefaultNamespace;

namespace GamePlayObjects.Fabrics
{
    public class KitchenFabric
    {
        public KitchenFabric(List<Kitchen> kitchens)
        {
            foreach (Kitchen kitchen in kitchens)
            {
                kitchen.KitchenSpawner.Initialize(kitchen.KitchenSettings);
            }
        }
    }
}