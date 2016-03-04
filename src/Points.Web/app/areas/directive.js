(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('directive', directive);
    
    function directive () {
        var directive = {
            restrict: 'EA',
            scope: false,
            templateUrl: '/app/areas/common/alertBox.html',
            controller: 'alertBoxController',
            controllerAs: 'abVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('alertBoxController', alertBoxController);

    alertBoxController.$inject = ['$timeout'];

    function alertBoxController($timeout) {
        /* jshint validthis:true */
        var abVm = this;



        activate();

        function activate() { }

    }

})();