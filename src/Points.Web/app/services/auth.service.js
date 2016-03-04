(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('authService', authService);

    authService.$inject = ['$http', '$q', 'localStorageService', 'cpSettings', 'authDataService', 'resourceService'];

    function authService($http, $q, localStorageService, cpSettings, authDataService, resourceService) {
        var service = {
            saveRegistration: saveRegistration,
            login: login,
            logOut: logOut,
            fillAuthData: fillAuthData,
            obtainAccessToken: obtainAccessToken,
            registerExternal: registerExternal
        };

        var serviceBase = cpSettings.apiServiceBaseUri;

        return service;

        function saveRegistration (registration) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
                if (response.status !== 202) {
                    authDataService.authentication.isAuth = true;
                    authDataService.authentication.userName = response.data.userName;
                    authDataService.authentication.userId = response.data.userId;
                    localStorageService.set('authorizationData', {
                        token: response.data.access_token,
                        userName: response.data.userName,
                        userId: response.data.userId
                    });
                    resourceService.initData();
                } else {
                    logOut();
                }
                deferred.resolve(response);
            }, function (err) {
                logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        function login (loginData) {
            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

            var deferred = $q.defer();

            $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
                authDataService.authentication.isAuth = true;
                authDataService.authentication.userName = loginData.userName;
                authDataService.authentication.userId = response.userId;
                localStorageService.set('authorizationData', {
                    token: response.access_token,
                    userName: loginData.userName,
                    userId: response.userId
                });
                resourceService.initData();
                deferred.resolve(response);
            }, function (err) {
                logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        function logOut () {
            localStorageService.remove('authorizationData');

            authDataService.authentication.isAuth = false;
            authDataService.authentication.userName = "";
            authDataService.authentication.userId = "";
        };

        function fillAuthData () {
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                authDataService.authentication.isAuth = true;
                authDataService.authentication.userName = authData.userName;
                authDataService.authentication.userId = authData.userId;
            }
        };

        function obtainAccessToken (externalData) {
            var deferred = $q.defer();

            $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {
                authDataService.authentication.isAuth = true;
                authDataService.authentication.userName = response.userName;
                authDataService.authentication.userId = response.userId;
                localStorageService.set('authorizationData', {
                    token: response.access_token,
                    userName: response.userName,
                    userId: response.userId
                });
                resourceService.initData();
                deferred.resolve(response);
            }, function (err) {
                logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        function registerExternal (registerExternalData) {
            var deferred = $q.defer();

            $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {
                authDataService.authentication.isAuth = true;
                authDataService.authentication.userName = response.userName;
                authDataService.authentication.userId = response.userId;
                localStorageService.set('authorizationData', {
                    token: response.access_token,
                    userName: response.userName,
                    userId: response.userId
                });
                resourceService.initData();
                deferred.resolve(response);
            }, function (err) {
                logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        }
    }
})();