(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('resourceService', resourceService);

    resourceService.$inject = ['$http', 'cpSettings', '$timeout', '$cacheFactory', 'authDataService'];

    function resourceService($http, cpSettings, $timeout, $cacheFactory, authDataService) {
        var service = {
            subscribe: subscribe,
            get: get,
            add: add,
            edit: edit,
            remove: remove,
            initData: initData
        };

        var serviceBase = cpSettings.apiResourceBaseUri;
        var callbacks = new Array();
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
            var typeHashes = callbacks[type];
            for (var key in typeHashes) {
                typeHashes[key](data);
            };
        };

        var retrieveWithRetry = function (type, retry) {
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
                    angular.forEach(link.links, function (linked) {
                        retrieve(linked);
                    });
                }
            });
        };

        return service;

        function subscribe (type, callback) {
            if (callback) {
                var hash = Sha1.hash(callback.toString());
                var types = callbacks[type];
                if (!types) {
                    types = new Array();
                    types[hash] = callback;
                    callbacks[type] = types;
                } else {
                    types[hash] = callback;
                }
            }
        };

        function get (type, callback) {
            service.subscribe(type, callback);
            var cache = getCache().get(type);
            if (cache) {
                $timeout(callCallbacks(type, cache), 10);
            } else {
                retrieve(type);
            }
        };

        function add (type,data) {
            return $http.post(serviceBase + 'api/' + type, data).then(function () {
                retrieve(type);
                updateLinks(type, 'add');
            });
        };

        function edit (type,data) {
            return $http.put(serviceBase + 'api/' + type, data).then(function () {
                retrieve(type);
                updateLinks(type, 'edit');
            });
        };

        function remove (type,id) {
            return $http.delete(serviceBase + 'api/' + type + '?id=' + id).then(function () {
                retrieve(type);
                updateLinks(type, 'delete');
            });
        };

        function initData () {
            retrieve('users');
            retrieve('categories');
            retrieve('tasks');
            retrieve('availabletasks');
            retrieve('planningtasks');
            retrieve('activetasks');
        };
    }
})();