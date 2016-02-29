'use strict';
app.factory('authDataService', function () {

    var authDataService = {};

    authDataService.authentication = {
        isAuth: false,
        userName: "",
        userId: ""
    };

    authDataService.externalAuthData = {
        provider: "",
        userName: "",
        userId: "",
        externalAccessToken: ""
    };

    return authDataService;
});