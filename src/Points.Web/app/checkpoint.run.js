(function () {
    'use strict';

    angular
        .module('cp.core')
        .run(runBlock);

    runBlock.$inject = ['authService'];

    function runBlock (authService) {
        authService.fillAuthData();
    }
})();
