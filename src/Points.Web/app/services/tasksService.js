'use strict';
app.factory('tasksService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var tasksServiceFactory = {};

    tasksServiceFactory.getTasks = function () {
        return $http.get(serviceBase + 'api/tasks').then(function (results) {
            return results;
        });
    };

    tasksServiceFactory.addTask = function(taskData) {
        return $http.post(serviceBase + 'api/tasks', taskData).then(function(results) {
            return results;
        });
    };

    tasksServiceFactory.editTask = function(taskData) {
        return $http.put(serviceBase + 'api/tasks', taskData).then(function(results) {
            return results;
        });
    };

    tasksServiceFactory.deleteTask = function(taskId) {
        return $http.delete(serviceBase + 'api/tasks?id=' + taskId).then(function(results) {
            return results;
        });
    };

    return tasksServiceFactory;

}]);