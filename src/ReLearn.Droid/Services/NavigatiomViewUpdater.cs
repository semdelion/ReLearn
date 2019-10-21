using ReLearn.Core.Services;
using ReLearn.Droid.Views.Menu;

namespace ReLearn.Droid.Services
{
    public class NavigatiomViewUpdater: INavigatiomViewUpdater
    {
        public void UpdateNavigatiomView() => MenuFragment.UpdateNavigatiomView();
    }
}