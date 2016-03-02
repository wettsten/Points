(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('filterCat', filterCat);
    
    function filterCat() {
        var directive = {
            restrict: 'EA',
            scope: false,
            templateUrl: '/app/areas/categories/filterCat.html',
            controller: 'filterCatController',
            controllerAs: 'filterCatVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('filterCatController', filterCatController);

    filterCatController.$inject = ['filterService'];

    function filterCatController(filterService) {
        /* jshint validthis:true */
        var filterCatVm = this;

        filterCatVm.filter = { text: '' };
        filterCatVm.search = search;
        filterCatVm.clear = clear;

        activate();

        function activate() { }

        function search () {
            filterService.setCatFilter({
                name: filterCatVm.filter.text
            });
        }

        function clear() {
            filterCatVm.filter.text = '';
            search();
        }
    }

})();