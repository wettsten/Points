'use strict';
app.factory('catsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var catsServiceFactory = {};

    catsServiceFactory.getCats = function () {

        return $http.get(serviceBase + 'api/categories').then(function (results) {
            return results;
        });
    };

    catsServiceFactory.addCat = function(catData) {
        return $http.post(serviceBase + 'api/categories', catData).then(function (results) {
            return results;
        });
    }

    return catsServiceFactory;

}]);