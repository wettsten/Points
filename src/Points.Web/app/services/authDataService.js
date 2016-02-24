'use strict';
app.factory('authDataService', function () {

    var authDataService = {};

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

    authDataService.authentication = _authentication;
    authDataService.externalAuthData = _externalAuthData;

    return authDataService;
});