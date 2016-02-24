'use strict';
app.factory('resourceService', ['$http', 'ngAuthSettings', '$timeout', '$cacheFactory', 'authDataService', function ($http, ngAuthSettings, $timeout, $cacheFactory, authDataService) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;
    var service = {};
    var callbacks = [];
    var caches = {};
    var links = [
        {
            type: 'planningtasks',
            action: 'add',
            links: [
                'availabletasks',
                'planningtotals'
            ]
        },
        {
            type: 'planningtasks',
            action: 'delete',
            links: [
                'availabletasks',
                'planningtotals'
            ]
        },
        {
            type: 'tasks',
            action: 'add',
            links: [
                'availabletasks'
            ]
        },
        {
            type: 'tasks',
            action: 'delete',
            links: [
                'availabletasks'
            ]
        },
        {
            type: 'activetasks',
            action: 'edit',
            links: [
                'activetotals'
            ]
        }
    ];

    var getCache = function () {
        if (!caches[authDataService.authentication.userId]) {
            var cache = $cacheFactory(authDataService.authentication.userId);
            caches[authDataService.authentication.userId] = cache;
            return cache;
        }
        return caches[authDataService.authentication.userId];
    };

    var setCache = function (type, data) {
        getCache().put(type, data);
    };

    var callCallbacks = function (type, data) {
        angular.forEach(callbacks, function(callback) {
            if (callback.type === type) {
                callback.callback(data);
            }
        });
    };

    var retrieveWithRetry = function (type,retry) {
        $http.get(serviceBase + 'api/' + type).then(function (results) {
            if (angular.toJson(getCache().get(type)) === angular.toJson(results.data) && retry < 1000) {
                var interval = 100;
                $timeout(retrieveWithRetry(type, retry + interval), interval);
                return;
            }
            setCache(type, results.data);
            callCallbacks(type, results.data);
        });
    }

    var retrieve = function (type) {
        retrieveWithRetry(type, 0);
    };

    var updateLinks = function (type, action) {
        angular.forEach(links, function (link) {
            if (link.type === type && link.action === action) {
                angular.forEach(link.links, function(linked) {
                    retrieve(linked);
                });
            }
        });
    };

    service.subscribe = function (type, callback) {
        if (callback) {
            callbacks.push({ type: type, callback: callback });
        }
    };

    service.get = function (type, callback) {
        service.subscribe(type, callback);
        var cache = getCache().get(type);
        if (cache) {
            $timeout(callCallbacks(type, cache), 10);
        } else {
            retrieve(type);
        }
    };

    service.add = function (type,data) {
        return $http.post(serviceBase + 'api/' + type, data).then(function () {
            retrieve(type);
            updateLinks(type, 'add');
        });
    };

    service.edit = function (type,data) {
        return $http.put(serviceBase + 'api/' + type, data).then(function () {
            retrieve(type);
            updateLinks(type, 'edit');
        });
    };

    service.delete = function (type,id) {
        return $http.delete(serviceBase + 'api/' + type + '?id=' + id).then(function () {
            retrieve(type);
            updateLinks(type, 'delete');
        });
    };

    return service;
}]);