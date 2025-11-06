using UniRx;
using System.Collections.Generic;

namespace UI
{
    public class UIEventBus
    {
        public readonly Subject<Unit> OnPopupShow = new();
        public readonly Subject<Unit> OnPopupHide = new();
        public readonly Subject<(Dictionary<string, int> resources, string changed)> OnResourceChanged = new();
    }
}