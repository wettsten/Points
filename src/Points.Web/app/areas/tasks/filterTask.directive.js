(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('filterTask', filterTask);
    
    function filterTask() {
        var directive = {
            restrict: 'EA',
            scope: {},
            templateUrl: '/app/areas/tasks/filterTask.html',
            controller: 'filterTaskController',
            controllerAs: 'filterVm',
            bindToController: true
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('filterTaskController', filterTaskController);

    filterTaskController.$inject = ['filterService', 'resourceService'];

    function filterTaskController(filterService, resourceService) {
        /* jshint validthis:true */
        var filterVm = this;

        filterVm.cats = [];
        filterVm.filter = { text: '', cat: {} };
        filterVm.search = search;
        filterVm.clear = clear;

        activate();

        function activate() {
            resourceService.get('categories', function (data) {
                filterVm.cats = data;
            });
        }

        function search () {
            filterService.setTaskFilter({
                name: filterVm.filter.text,
                category: {
                    name: filterVm.filter.cat ? filterVm.filter.cat.name : ''
                }
            });
        }

        function clear () {
            filterVm.filter = { text: '', cat: {} };
            search();
        }
    }

})();