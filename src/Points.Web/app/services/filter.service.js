(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('filterService', filterService);

    filterService.$inject = ['$rootScope'];

    function filterService($rootScope) {
        var filters = {
            cat: { name: '' },
            task: { name: '', category: { name: '' } },
            pTask: { name: '', task: { category: { name: '' } } },
            aTask: { name: '', task: { category: { name: '' } } }
        };

        var service = {
            getCatFilter: getCatFilter,
            setCatFilter: setCatFilter,
            getTaskFilter: getTaskFilter,
            setTaskFilter: setTaskFilter,
            getPTaskFilter: getPTaskFilter,
            setPTaskFilter: setPTaskFilter,
            getATaskFilter: getATaskFilter,
            setATaskFilter: setATaskFilter,
            subscribe: subscribe
        };

        return service;

        function setCatFilter (catFilter) {
            filters.cat = catFilter;
            $rootScope.$emit('catFilter');
        }
        function getCatFilter() {
            return filters.cat;
        }
        function setTaskFilter (taskFilter) {
            filters.task = taskFilter;
            $rootScope.$emit('taskFilter');
        }
        function getTaskFilter() {
            return filters.task;
        }
        function setPTaskFilter (taskFilter) {
            filters.pTask = taskFilter;
            $rootScope.$emit('ptaskFilter');
        }
        function getPTaskFilter() {
            return filters.pTask;
        }
        function setATaskFilter(taskFilter) {
            filters.aTask = taskFilter;
            $rootScope.$emit('ataskFilter');
        }
        function getATaskFilter () {
            return filters.aTask;
        }
        function subscribe (scope, event, callback) {
            var handler = $rootScope.$on(event, callback);
            scope.$on('$destroy', handler);
        }
    }
})();