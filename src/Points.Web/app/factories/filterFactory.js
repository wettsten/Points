'use strict';
app.factory('filterFactory', function ($rootScope) {
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
            name: [],
            category: {
                name: []
            }
        },
        aTask: {
            name: [],
            category: {
                name: []
            }
        }
    };

    return {
        getTaskFilter: function () {
            return filters.task;
        },
        setTaskFilter: function (taskFilter) {
            filters.task = taskFilter;
            $rootScope.$emit('taskFilter');
        },
        getCatFilter: function () {
            return filters.cat;
        },
        setCatFilter: function (catFilter) {
            filters.cat = catFilter;
            $rootScope.$emit('catFilter');
        },
        getPTaskFilter: function () {
            return filters.pTask;
        },
        setPTaskFilter: function (taskFilter) {
            filters.pTask = taskFilter;
            $rootScope.$emit('ptaskFilter');
        },
        getATaskFilter: function () {
            return filters.aTask;
        },
        setATaskFilter: function (taskFilter) {
            filters.aTask = taskFilter;
            $rootScope.$emit('ataskFilter');
        },
        subscribe: function (scope, event, callback) {
            var handler = $rootScope.$on(event, callback);
            scope.$on('$destroy', handler);
        }
    };
});

