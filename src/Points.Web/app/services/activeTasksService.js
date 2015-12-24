'use strict';
app.factory('activeTasksService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var tasksServiceFactory = {};

    tasksServiceFactory.getTasks = function () {
        return $http.get(serviceBase + 'api/activetasks').then(function (results) {
            return results;
        });
    };

    tasksServiceFactory.editTask = function(taskData) {
        return $http.put(serviceBase + 'api/activetasks', taskData).then(function (results) {
            return results;
        });
    };

    return tasksServiceFactory;

}]);