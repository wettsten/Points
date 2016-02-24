'use strict';
app.factory('userService', ['$http', 'ngAuthSettings', '$cacheFactory', function ($http, ngAuthSettings, $cacheFactory) {

    var userServiceFactory = {};
    var serviceBase = ngAuthSettings.apiResourceBaseUri;
    var caches = {};

    var getCache = function (id) {
        if (!caches['user' + id]) {
            var cache = $cacheFactory('user' + id);
            caches['user' + id] = cache;
            return cache;
        }
        return caches['user' + id];
    };

    var setCache = function (id,data) {
        getCache(id).put('user', data);
    };

    userServiceFactory.getUser = function (id,callback) {
        var cache = getCache(id).get('user');
        if (!cache) {
            $http.get(serviceBase + 'api/users').then(function (results) {
                setCache(id, results.data[0]);
                if (callback) {
                    callback(results.data[0]);
                }
            });
        } else if (callback) {
            callback(cache);
        }
    };

    userServiceFactory.editUser = function (userData) {
        return $http.put(serviceBase + 'api/users', userData).then(function(results) {
            setCache(userData.id,results.data[0]);
        });
    };

    return userServiceFactory;
}]);