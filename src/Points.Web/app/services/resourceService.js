'use strict';
app.factory('resourceService', ['$http', 'ngAuthSettings', '$timeout', function ($http, ngAuthSettings, $timeout) {

    var serviceBase = ngAuthSettings.apiResourceBaseUri;
    var service = {};
    var callbacks = [];
    var cats = [];
    var tasks = [];
    var planningtasks = [];
    var activetasks = [];

    var callCallbacks = function(type,data) {
        for (var i = 0; i < callbacks.length; i++) {
            if (callbacks[i].type === type) {
                callbacks[i].callback(data);
            }
        }
    };

    var retrieveWithAttempt = function (type,attempt) {
        $http.get(serviceBase + 'api/' + type).then(function (results) {
            var objList = [];
            switch (type) {
                case 'categories':
                    objList = cats;
                    cats = results.data;
                    break;
                case 'tasks':
                    objList = tasks;
                    tasks = results.data;
                    break;
                case 'planningtasks':
                    objList = planningtasks;
                    planningtasks = results.data;
                    break;
                case 'activetasks':
                    objList = activetasks;
                    activetasks = results.data;
                    break;
            }
            if (angular.toJson(objList) === angular.toJson(results.data) && attempt < 5) {
                $timeout(retrieveWithAttempt(type, ++attempt), 250);
                return;
            }
            callCallbacks(type, results.data);
        });
    };

    var retrieve = function (type) {
        retrieveWithAttempt(type, 0);
    };

    service.registerForUpdates = function(type,callback) {
        callbacks.push({ type: type, callback: callback });
    };
    
    service.get = function (type) {
        switch (type) {
            case 'categories':
                return cats;
            case 'tasks':
                return tasks;
            case 'planningtasks':
                return planningtasks;
            case 'activetasks':
                return activetasks;
        }
        return [];
    };

    service.add = function (type,data) {
        return $http.post(serviceBase + 'api/' + type, data).then(function () {
            retrieve(type);
        });
    };

    service.edit = function (type,data) {
        return $http.put(serviceBase + 'api/' + type, data).then(function () {
            retrieve(type);
        });
    };

    service.delete = function (type,id) {
        return $http.delete(serviceBase + 'api/' + type + '?id=' + id).then(function () {
            retrieve(type);
        });
    };

    retrieve('categories');
    retrieve('tasks');
    retrieve('planningtasks');
    retrieve('activetasks');

    return service;
}]);