'use strict';
app.factory('tasksService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var tasksServiceFactory = {};

    var _getTasksByUser = function (userId) {

        return $http.get(serviceBase + 'api/tasks?userid=' + userId).then(function (results) {
            return results;
        });
    };

    tasksServiceFactory.getTasksByUser = _getTasksByUser;

    return tasksServiceFactory;

}]);