﻿using System;
using System.Linq;
using Points.Data;
using Points.DataAccess.Readers;
using Points.DataAccess.Writers;
using Points.Scheduler.Processors;

namespace Points.Scheduler.Jobs
{
    public class StartWeekJob : IJob
    {
        private readonly ISingleSessionDataReader _dataReader;
        private readonly ISingleSessionDataWriter _dataWriter;
        private readonly IJobManager _jobManager;

        public StartWeekJob(ISingleSessionDataReader dataReader, ISingleSessionDataWriter dataWriter, IJobManager jobManager)
        {
            _dataReader = dataReader;
            _dataWriter = dataWriter;
            _jobManager = jobManager;
        }

        public void Process(Job context)
        {
            var tasks = _dataReader
                .GetAll<PlanningTask>()
                .Where(i => i.UserId.Equals(context.UserId, StringComparison.InvariantCultureIgnoreCase));
            foreach (var task in tasks)
            {
                var activeTask = new ActiveTask();
                activeTask.Copy(task);
                activeTask.Id = string.Empty;
                activeTask.DateStarted = DateTime.UtcNow;
                _dataWriter.Add(activeTask);
            }
            if (tasks.Any())
            {
                _jobManager.ScheduleEndJob(context.UserId);
                var user = _dataReader.Get<User>(context.UserId);
                user.ActiveTargetPoints = user.TargetPoints;
                _dataWriter.Edit(user);
            }
            _jobManager.ScheduleStartJob(context.UserId);
        }
    }
}