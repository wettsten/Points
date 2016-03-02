(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('filterPlanningTask', filterPlanningTask);
    
    function filterPlanningTask() {
        var directive = {
            restrict: 'EA',
            scope: {},
            templateUrl: '/app/areas/tasks/filterTask.html',
            controller: 'filterPlanningTaskController',
            controllerAs: 'filterVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('filterPlanningTaskController', filterPlanningTaskController);

    filterPlanningTaskController.$inject = ['filterService', 'resourceService'];

    function filterPlanningTaskController(filterService, resourceService) {
        /* jshint validthis:true */
        var filterVm = this;

        filterVm.cats = [];
        filterVm.filter = { text: '', cat: {} };
        filterVm.search = search;
        filterVm.clear = clear;

        activate();

        function activate() {
            resourceService.get('planningtotals', function (data) {
                filterVm.cats = data.categories;
            });
        }

        function search () {
            filterService.setPTaskFilter({
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