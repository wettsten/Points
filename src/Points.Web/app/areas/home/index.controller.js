(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('indexController', indexController);

    indexController.$inject = ['authDataService', '$location', 'authService'];

    function indexController(authDataService, $location, authService) {
        /* jshint validthis:true */
        var indexVm = this;

        indexVm.logOut = logOut;
        indexVm.authentication = authDataService.authentication;

        activate();

        function activate() { }

        function logOut () {
            authService.logOut();
            $location.path('/home');
        }
    }
})();
