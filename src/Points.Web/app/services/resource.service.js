(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('resourceService', resourceService);

    resourceService.$inject = ['$http', 'cpSettings', '$timeout', 'resourceCacheService', 'resourceSubscriptionService'];

    function resourceService($http, cpSettings, $timeout, resourceCacheService, resourceSubscriptionService) {
        var service = {
            subscribe: subscribe,
            get: get,
            add: add,
            edit: edit,
            remove: remove,
            initData: initData
        };

        var serviceBase = cpSettings.apiResourceBaseUri;
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

        return service;

        function subscribe(type, callback) {
            resourceSubscriptionService.subscribe(type, callback);
        }

        function retrieveWithRetry(type, retry) {
            $http.get(serviceBase + 'api/' + type).then(function (results) {
                if (angular.toJson(resourceCacheService.getCache().get(type)) === angular.toJson(results.data) && retry < 1000) {
                    var interval = 100;
                    $timeout(retrieveWithRetry(type, retry + interval), interval);
                    return;
                }
                resourceCacheService.setCache(type, results.data);
                resourceSubscriptionService.callCallbacks(type, results.data);
            });
        }

        function retrieve(type) {
            retrieveWithRetry(type, 0);
        }

        function updateLinks(type, action) {
            var filteredLinks = _.where(links, { type: type, action: action });
            for (var link in filteredLinks) {
                retrieve(link);
            }
        }

        function get (type, callback) {
            resourceSubscriptionService.subscribe(type, callback);
            var cache = resourceCacheService.getCache().get(type);
            if (cache) {
                $timeout(resourceSubscriptionService.callCallbacks(type, cache), 10);
            } else {
                retrieve(type);
            }
        }

        function add (type,data) {
            return $http.post(serviceBase + 'api/' + type, data).then(function () {
                retrieve(type);
                updateLinks(type, 'add');
            });
        }

        function edit (type,data) {
            return $http.put(serviceBase + 'api/' + type, data).then(function () {
                retrieve(type);
                updateLinks(type, 'edit');
            });
        }

        function remove (type,id) {
            return $http.delete(serviceBase + 'api/' + type + '?id=' + id).then(function () {
                retrieve(type);
                updateLinks(type, 'delete');
            });
        }

        function initData () {
            retrieve('users');
            retrieve('categories');
            retrieve('tasks');
            retrieve('availabletasks');
            retrieve('planningtasks');
            retrieve('activetasks');
        }
    }
})();