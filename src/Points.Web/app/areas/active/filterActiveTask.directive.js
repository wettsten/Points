(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('filterActiveTask', filterActiveTask);
    
    function filterActiveTask() {
        var directive = {
            restrict: 'EA',
            scope: {},
            templateUrl: '/app/areas/tasks/filterTask.html',
            controller: 'filterActiveTaskController',
            controllerAs: 'filterVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('filterActiveTaskController', filterActiveTaskController);

    filterActiveTaskController.$inject = ['filterService', 'resourceService'];

    function filterActiveTaskController(filterService, resourceService) {
        /* jshint validthis:true */
        var filterVm = this;

        filterVm.cats = [];
        filterVm.filter = { text: '', cat: {} };
        filterVm.search = search;
        filterVm.clear = clear;

        activate();

        function activate() {
            resourceService.get('activetotals', function (data) {
                filterVm.cats = data.categories;
            });
        }

        function search () {
            filterService.setATaskFilter({
                name: filterVm.filter.text,
                task: {
                    category: {
                        name: filterVm.filter.cat ? filterVm.filter.cat.name : ''
                    }
                }
            });
        }

        function clear() {
            filterVm.filter = { text: '', cat: {} };
            search();
        }
    }

})();