(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('filterTask', filterTask);
    
    function filterTask() {
        var directive = {
            restrict: 'EA',
            scope: false,
            templateUrl: '/app/areas/tasks/filterTask.html',
            controller: 'filterTaskController',
            controllerAs: 'filterTaskVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('filterTaskController', filterTaskController);

    filterTaskController.$inject = ['filterService', 'resourceService'];

    function filterTaskController(filterService, resourceService) {
        /* jshint validthis:true */
        var filterTaskVm = this;

        filterTaskVm.cats = [];
        filterTaskVm.filter = { text: '', cat: {} };
        filterTaskVm.search = search;
        filterTaskVm.clear = clear;


        activate();

        function activate() {
            resourceService.get('categories', function (data) {
                filterTaskVm.cats = data;
            });
        }

        function search () {
            filterService.setTaskFilter({
                name: filterTaskVm.filter.text,
                category: {
                    name: filterTaskVm.filter.cat ? filterTaskVm.filter.cat.name : ''
                }
            });
        }

        function clear () {
            filterTaskVm.filter.text = '';
            filterTaskVm.filter.cat = {};
            search();
        }
    }

})();