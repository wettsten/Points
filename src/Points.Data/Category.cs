using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Points.Data
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
