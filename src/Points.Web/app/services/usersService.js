'use strict';
app.factory('usersService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var usersServiceFactory = {};

    usersServiceFactory.getUser = function () {
        return $http.get(serviceBase + 'api/users').then(function(results) {
            return results;
        });
    };

    usersServiceFactory.editUser = function(userData) {
        return $http.put(serviceBase + 'api/users', userData).then(function(results) {
            return results;
        });
    };

    return usersServiceFactory;

}]);