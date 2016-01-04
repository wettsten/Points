﻿
using System;

namespace Points.Data.View
{
    public class ArchivedTask : ActiveTask
    {
        public DateTime DateEnded { get; set; }

        public override void Copy(ViewObject obj)
        {
            base.Copy(obj);
            var task = obj as ArchivedTask;
            if (task != null)
            {
                DateEnded = task.DateEnded;
            }
        }
    }
}
