'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', 'authDataService', 'resourceService', function ($http, $q, localStorageService, ngAuthSettings, authDataService, resourceService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    authServiceFactory.saveRegistration = function (registration) {
        authServiceFactory.logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });
    };

    authServiceFactory.login = function (loginData) {
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        
        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            authDataService.authentication.isAuth = true;
            authDataService.authentication.userName = loginData.userName;
            authDataService.authentication.userId = response.userId;
            localStorageService.set('authorizationData', {
                token: response.access_token,
                userName: loginData.userName,
                userId: authDataService.authentication.userId
            });
            resourceService.initData();
            deferred.resolve(response);
        }).error(function (err, status) {
            authServiceFactory.logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    authServiceFactory.logOut = function () {
        localStorageService.remove('authorizationData');

        authDataService.authentication.isAuth = false;
        authDataService.authentication.userName = "";
        authDataService.authentication.userId = "";
    };

    authServiceFactory.fillAuthData = function () {
        var authData = localStorageService.get('authorizationData');
        if (authData) {
            authDataService.authentication.isAuth = true;
            authDataService.authentication.userName = authData.userName;
            authDataService.authentication.userId = authData.userId;
        }
    };

    authServiceFactory.obtainAccessToken = function (externalData) {
        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {
            authDataService.authentication.isAuth = true;
            authDataService.authentication.userName = response.userName;
            authDataService.authentication.userId = response.userId;
            localStorageService.set('authorizationData', {
                token: response.access_token,
                userName: response.userName,
                userId: authDataService.authentication.userId
            });
            resourceService.initData();
            deferred.resolve(response);
        }).error(function (err, status) {
            authServiceFactory.logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    authServiceFactory.registerExternal = function (registerExternalData) {
        var deferred = $q.defer();

        $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {
            authDataService.authentication.isAuth = true;
            authDataService.authentication.userName = response.userName;
            authDataService.authentication.userId = response.userId;
            localStorageService.set('authorizationData', {
                token: response.access_token,
                userName: response.userName,
                userId: authDataService.authentication.userId
            });
            resourceService.initData();
            deferred.resolve(response);
        }).error(function (err, status) {
            authServiceFactory.logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    authServiceFactory.authentication = authDataService.authentication;
    authServiceFactory.externalAuthData = authDataService.externalAuthData;

    return authServiceFactory;
}]);