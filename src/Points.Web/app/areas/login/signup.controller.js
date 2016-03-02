(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('signupController', signupController);

    signupController.$inject = ['$location', '$timeout', 'authService'];

    function signupController($location, $timeout, authService) {
        /* jshint validthis:true */
        var signupVm = this;

        signupVm.savedSuccessfully = false;
        signupVm.message = "";
        signupVm.registration = {
            userName: "",
            password: "",
            confirmPassword: ""
        };
        signupVm.signUp = signUp;

        activate();

        function activate() { }

        function signUp() {
            authService.saveRegistration(signupVm.registration).then(
                function (response) {
                    if (response.status === 202) {
                        signupVm.savedSuccessfully = true;
                        signupVm.message = "User has been registered successfully, you will be redicted to Login page in 2 seconds.";
                        $timeout(function () {
                            $location.path('/login');
                        }, 2000);
                    } else {
                        signupVm.savedSuccessfully = true;
                        signupVm.message = "User has been registered successfully, you will be redicted to User Options page in 2 seconds.";
                        $timeout(function () {
                            $location.path('/options');
                        }, 2000);
                    }
                },
                function (response) {
                    var errors = [];
                    for (var key in response.data.modelState) {
                        for (var i = 0; i < response.data.modelState[key].length; i++) {
                            errors.push(response.data.modelState[key][i]);
                        }
                    }
                    signupVm.message = "Failed to register user due to: " + errors.join(' ');
                }
            );
        };
    }
})();
