'use strict';
app.factory('catsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;

    var catsServiceFactory = {};

    catsServiceFactory.getCatsByUser = function (userId) {

        return $http.get(serviceBase + 'api/categories?userid=' + userId).then(function (results) {
            return results;
        });
    };

    catsServiceFactory.addCat = function(catData) {
        return $http.post(serviceBase + 'api/categories', catData).then(function (results) {
            return results;
        });
    }

    catsServiceFactory.editCat = function (catData) {
        return $http.put(serviceBase + 'api/categories', catData).then(function (results) {
            return results;
        });
    }

    catsServiceFactory.deleteCat = function (catId) {
        return $http.delete(serviceBase + 'api/categories?id=' + catId).then(function (results) {
            return results;
        });
    }

    return catsServiceFactory;

}]);