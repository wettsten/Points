'use strict';
app.factory('catsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var catsServiceFactory = {};

    var _getCats = function () {

        return $http.get(serviceBase + 'api/categories').then(function (results) {
            return results;
        });
    };

    catsServiceFactory.getCats = _getCats;

    return catsServiceFactory;

}]);