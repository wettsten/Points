(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('tasksController', tasksController);

    tasksController.$inject = ['$scope', 'resourceService', 'filterService', '$timeout'];

    function tasksController($scope, resourceService, filterService, $timeout) {
        /* jshint validthis:true */
        var tasksVm = this;

        tasksVm.noItems = false;
        tasksVm.tasks = [];
        tasksVm.taskInEdit = { id: '' };
        tasksVm.taskFilter = filterService.getTaskFilter();
        tasksVm.refreshTasks = refreshTasks;

        activate();

        function activate() {
            filterService.subscribe($scope, 'taskFilter', getTaskFilter);

            resourceService.get('tasks', getTasks);
        }

        function getTaskFilter() {
            tasksVm.taskFilter = filterService.getTaskFilter();
        }

        function getTasks(data) {
            tasksVm.tasks = data;
            if (data.length === 0) {
                tasksVm.addWarning('No tasks found');
                resourceService.get('categories', checkCategories);
            }
        }

        function checkCategories(data) {
            if (data.length === 0) {
                tasksVm.noItems = true;
            }
        }

        function refreshTasks () {
            resourceService.get('tasks');
        }
    }
})();
