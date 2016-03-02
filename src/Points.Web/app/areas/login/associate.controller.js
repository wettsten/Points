﻿(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('associateController', associateController);

    associateController.$inject = ['$location', '$timeout', 'authService', 'authDataService'];

    function associateController($location, $timeout, authService, authDataService) {
        /* jshint validthis:true */
        var associateVm = this;

        associateVm.savedSuccessfully = false;
        associateVm.message = "";
        associateVm.registerData = {
            userName: authDataService.externalAuthData.userName,
            provider: authDataService.externalAuthData.provider,
            externalAccessToken: authDataService.externalAuthData.externalAccessToken
        };
        associateVm.registerExternal = registerExternal;

        activate();

        function activate() { }

        function registerExternal() {
            authService.registerExternal(associateVm.registerData).then(
                function (response) {
                    associateVm.savedSuccessfully = true;
                    associateVm.message = "User has been registered successfully, you will be redicted to User Options page in 2 seconds.";
                    $timeout(function () {
                        $location.path('/options');
                    }, 2000);
                },
                function (response) {
                    var errors = [];
                    for (var key in response.modelState) {
                        errors.push(response.modelState[key]);
                    }
                    associateVm.message = "Failed to register user due to:" + errors.join(' ');
                }
            );
        };
    }
})();