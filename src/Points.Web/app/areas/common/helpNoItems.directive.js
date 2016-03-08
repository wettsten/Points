(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('helpNoItems', helpNoItems);
    
    function helpNoItems() {
        var directive = {
            restrict: 'EA',
            scope: {
                showCategories: '@',
                showTasks: '@',
                showPlanning: '@'
            },
            templateUrl: '/app/areas/common/helpNoItems.html',
            controller: 'helpNoItemsController',
            controllerAs: 'hniVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('helpNoItemsController', helpNoItemsController);

    helpNoItemsController.$inject = [];

    function helpNoItemsController() {
        /* jshint validthis:true */
        var hniVm = this;

        hniVm.show = {
            categories: (typeof hniVm.showCategories === 'undefined') ? false : (hniVm.showCategories === false) ? false : true,
            tasks: (typeof hniVm.showTasks === 'undefined') ? false : (hniVm.showTasks === false) ? false : true,
            planning: (typeof hniVm.showPlanning === 'undefined') ? false : (hniVm.showPlanning === false) ? false : true
        };

        activate();

        function activate() { }

    }

})();