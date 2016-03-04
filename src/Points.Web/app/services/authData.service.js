(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('authDataService', authDataService);
    
    function authDataService() {
        var authentication = {
            isAuth: false,
            userName: '',
            userId: ''
        };
        var externalAuthData = {
            provider: '',
            userName: '',
            userId: '',
            externalAccessToken: ''
        };
        var service = {
            authentication: authentication,
            externalAuthData: externalAuthData
        };

        return service;
    }
})();