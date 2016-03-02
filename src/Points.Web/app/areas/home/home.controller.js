(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('homeController', homeController);

    homeController.$inject = [];

    function homeController() {
        /* jshint validthis:true */
        var homeVm = this;


        activate();

        function activate() { }
    }
})();
