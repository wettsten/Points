﻿using Points.Data;

namespace Points.Scheduler
{
    public interface IJob
    {
        void Process(Job context);
    }
}