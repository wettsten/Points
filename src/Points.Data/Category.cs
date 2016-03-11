namespace Points.Data
{
    public class Category : DataBase
    {
        public override void Copy(DataBase obj)
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
