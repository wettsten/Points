'use strict';
app.factory('planningTasksService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var tasksServiceFactory = {};

    tasksServiceFactory.getTasksByUser = function (userId) {
        return $http.get(serviceBase + 'api/planningtasks?userid=' + userId).then(function (results) {
            return results;
        });
    };

    tasksServiceFactory.getEnums = function () {
        return $http.get(serviceBase + 'api/planningtasks/enums').then(function (results) {
            return results;
        });
    };

    tasksServiceFactory.addTask = function(taskData) {
        return $http.post(serviceBase + 'api/planningtasks', taskData).then(function (results) {
            return results;
        });
    };

    tasksServiceFactory.editTask = function(taskData) {
        return $http.put(serviceBase + 'api/planningtasks', taskData).then(function (results) {
            return results;
        });
    };

    tasksServiceFactory.deleteTask = function(taskId) {
        return $http.delete(serviceBase + 'api/planningtasks?id=' + taskId).then(function (results) {
            return results;
        });
    };

    return tasksServiceFactory;

}]);