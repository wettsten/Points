﻿'use strict';
app.factory('resourceService', ['$http', 'ngAuthSettings', '$timeout', '$cacheFactory', 'authService', function ($http, ngAuthSettings, $timeout, $cacheFactory, authService) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;
    var service = {};
    var callbacks = [];
    var caches = {};

    var getCache = function () {
        if (!caches[authService.authentication.userId]) {
            var cache = $cacheFactory(authService.authentication.userId);
            caches[authService.authentication.userId] = cache;
            return cache;
        }
        return caches[authService.authentication.userId];
    };

    var setCache = function (type, data) {
        getCache().put(type, data);
    };

    var callCallbacks = function(type,data) {
        for (var i = 0; i < callbacks.length; i++) {
            if (callbacks[i].type === type) {
                callbacks[i].callback(data);
            }
        }
    };

    service.registerForUpdates = function(type,callback) {
        callbacks.push({ type: type, callback: callback });
    };

    var retrieveWithRetry = function (type,retry) {
        $http.get(serviceBase + 'api/' + type).then(function (results) {
            if (angular.toJson(getCache().get(type)) === angular.toJson(results.data) && retry < 5) {
                $timeout(retrieveWithRetry(type, ++retry), 250);
                return;
            }
            setCache(type, results.data);
            callCallbacks(type, results.data);
        });
    }

    var retrieve = function (type) {
        retrieveWithRetry(type, 0);
    };

    service.get = function (type) {
        var cache = getCache().get(type);
        if (!cache) {
            retrieve(type);
        } else {
            $timeout(callCallbacks(type, cache), 10);
        }
    };

    service.add = function (type,data) {
        return $http.post(serviceBase + 'api/' + type, data).then(function () {
            $timeout(retrieve(type), 1000);
            if (type === 'planningtasks') {
                $timeout(retrieve('availabletasks'), 500);
            }
        });
    };

    service.edit = function (type,data) {
        return $http.put(serviceBase + 'api/' + type, data).then(function () {
            $timeout(retrieve(type), 250);
        });
    };

    service.delete = function (type,id) {
        return $http.delete(serviceBase + 'api/' + type + '?id=' + id).then(function () {
            $timeout(retrieve(type), 250);
            if (type === 'planningtasks') {
                $timeout(retrieve('availabletasks'), 250);
            }
        });
    };

    return service;
}]);