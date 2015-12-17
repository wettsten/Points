'use strict';
app.factory('usersService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var usersServiceFactory = {};

    usersServiceFactory.getUserByName = function (name) {
        return $http.get(serviceBase + 'api/users?name=' + name).then(function(results) {
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