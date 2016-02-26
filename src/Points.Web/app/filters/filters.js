'use strict';
app.filter('complexTaskFilter', function () {
    return function (tasks,filterTasks) {
        var out = [];

        angular.forEach(tasks, function(task) {
            if (_.contains(filterTasks.name, task.name)) {
                out.push(task);
            }
        });

        return out;
    }
});