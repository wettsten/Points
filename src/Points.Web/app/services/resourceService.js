'use strict';
app.factory('resourceService', ['$http', 'ngAuthSettings', '$timeout', function ($http, ngAuthSettings, $timeout) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;
    var service = {};
    var callbacks = [];

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

    service.get = function(type) {
        $http.get(serviceBase + 'api/' + type).then(function(results) {
            callCallbacks(type, results.data);
        });
    };

    service.add = function (type,data) {
        return $http.post(serviceBase + 'api/' + type, data).then(function () {
            $timeout(service.get(type), 1000);
            if (type === 'planningtasks') {
                $timeout(service.get('availabletasks'), 500);
            }
        });
    };

    service.edit = function (type,data) {
        return $http.put(serviceBase + 'api/' + type, data).then(function () {
            $timeout(service.get(type), 250);
        });
    };

    service.delete = function (type,id) {
        return $http.delete(serviceBase + 'api/' + type + '?id=' + id).then(function () {
            $timeout(service.get(type), 250);
            if (type === 'planningtasks') {
                $timeout(service.get('availabletasks'), 250);
            }
        });
    };

    return service;
}]);