namespace Points.Data.Raven
{
    public class Category : RavenObject
    {
        public override void Copy(RavenObject obj)
        {
            base.Copy(obj);
            var cat = obj as Category;
            if (cat != null)
            {
                var objCat = cat;
            }
        }
    }
}
