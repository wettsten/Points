(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('loginController', loginController);

    loginController.$inject = ['$location', 'authService'];

    function loginController($location, authService) {
        /* jshint validthis:true */
        var loginVm = this;

        loginVm.loginData = {
            userName: "",
            password: ""
        };
        loginVm.message = "";
        loginVm.login = login;

        activate();

        function activate() { }

        function login () {
            authService.login(loginVm.loginData).then(
                function (response) {
                    $location.path('/active');
                },
                function (err) {
                    loginVm.message = err.error_description;
                }
            );
        };
    }
})();
