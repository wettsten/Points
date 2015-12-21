'use strict';
app.factory('jobsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var jobsServiceFactory = {};

    jobsServiceFactory.getStartJob = function () {
        return $http.get(serviceBase + 'api/jobs/start').then(function (results) {
            return results;
        });
    };

    jobsServiceFactory.getEndJob = function () {
        return $http.get(serviceBase + 'api/jobs/end').then(function (results) {
            return results;
        });
    };

    return jobsServiceFactory;

}]);