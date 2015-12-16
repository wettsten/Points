
using Points.Data.Raven;

namespace Points.Data.View
{
    public class Category : ViewObject
    {
        public override void Copy(ViewObject obj)
        {
            base.Copy(obj);
            var cat = obj as Category;
            if (cat != null)
            {

            }
        }
    }
}
