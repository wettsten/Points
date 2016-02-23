'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', 'usersService', function ($http, $q, localStorageService, ngAuthSettings, usersService) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        userId: ""
    };

    var _externalAuthData = {
        provider: "",
        userName: "",
        userId: "",
        externalAccessToken: ""
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        
        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            
            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;
            _authentication.userId = response.userId;
            localStorageService.set('authorizationData', {
                token: response.access_token,
                userName: loginData.userName,
                userId: _authentication.userId
            });
            usersService.getUser();
            deferred.resolve(response);
        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.userId = "";
    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.userId = authData.userId;
        }

    };

    var _obtainAccessToken = function (externalData) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).success(function (response) {

            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.userId = response.userId;
            localStorageService.set('authorizationData', {
                token: response.access_token,
                userName: response.userName,
                userId: _authentication.userId
            });
            usersService.getUser();
            deferred.resolve(response);
        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _registerExternal = function (registerExternalData) {

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).success(function (response) {
            
            _authentication.isAuth = true;
            _authentication.userName = response.userName;
            _authentication.userId = response.userId;
            localStorageService.set('authorizationData', {
                token: response.access_token,
                userName: response.userName,
                userId: _authentication.userId
            });
            usersService.getUser();
            deferred.resolve(response);
        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;

    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;

    return authServiceFactory;
}]);