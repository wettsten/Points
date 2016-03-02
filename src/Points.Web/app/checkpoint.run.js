(function () {
    'use strict';

    angular
        .module('checkpoint')
        .run(runBlock);

    runBlock.$inject = ['authService'];

    function runBlock (authService) {
        authService.fillAuthData();
    }
})();
