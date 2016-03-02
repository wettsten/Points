(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('resourceCacheService', resourceCacheService);

    resourceCacheService.$inject = ['$cacheFactory', 'authDataService'];

    function resourceCacheService($cacheFactory, authDataService) {
        var service = {
            getCache: getCache,
            setCache: setCache
        };

        var caches = {};

        return service;

        function getCache() {
            if (!caches[authDataService.authentication.userId]) {
                var cache = $cacheFactory(authDataService.authentication.userId);
                caches[authDataService.authentication.userId] = cache;
                return cache;
            }
            return caches[authDataService.authentication.userId];
        }

        function setCache(type, data) {
            getCache().put(type, data);
        }
    }
})();