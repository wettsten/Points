(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('authInterceptorService', authInterceptorService);

    authInterceptorService.$inject = ['$q', '$injector', '$location', 'localStorageService'];

    function authInterceptorService($q, $injector, $location, localStorageService) {
        var service = {
            request: request,
            responseError: responseError
        };

        return service;

        function request (config) {
            config.headers = config.headers || {};
       
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }
            return config;
        }

        function responseError (rejection) {
            if (rejection.status === 401) {
                var authService = $injector.get('authService');
                authService.logOut();
                $location.path('/login');
            }
            return $q.reject(rejection);
        }
    }
})();