(function () {
    'use strict';

    angular
        .module('checkpoint')
        .factory('filterService', filterService);

    filterService.$inject = ['$rootScope'];

    function filterService($rootScope) {
        var filters = {
            cat: {
                name: ''
            },
            task: {
                name: '',
                category: {
                    name: ''
                }
            },
            pTask: {
                name: '',
                task: {
                    category: {
                        name: ''
                    }
                }
            },
            aTask: {
                name: '',
                task: {
                    category: {
                        name: ''
                    }
                }
            }
        };

        var service = {
            getCatFilter: filters.cat,
            setCatFilter: setCatFilter,
            getTaskFilter: filters.task,
            setTaskFilter: setTaskFilter,
            getPTaskFilter: filters.pTask,
            setPTaskFilter: setPTaskFilter,
            getATaskFilter: filters.aTask,
            setATaskFilter: setATaskFilter,
            subscribe: subscribe
        };

        return service;

        function setCatFilter (catFilter) {
            filters.cat = catFilter;
            $rootScope.$emit('catFilter');
        }
        function setTaskFilter (taskFilter) {
            filters.task = taskFilter;
            $rootScope.$emit('taskFilter');
        }
        function setPTaskFilter (taskFilter) {
            filters.pTask = taskFilter;
            $rootScope.$emit('ptaskFilter');
        }
        function setATaskFilter (taskFilter) {
            filters.aTask = taskFilter;
            $rootScope.$emit('ataskFilter');
        }
        function subscribe (scope, event, callback) {
            var handler = $rootScope.$on(event, callback);
            scope.$on('$destroy', handler);
        }
    }
})();