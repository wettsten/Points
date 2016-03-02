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
        tasksVm.loadTasks = loadTasks;

        activate();

        function activate() {
            filterService.subscribe($scope, 'taskFilter', function () {
                tasksVm.taskFilter = filterService.getTaskFilter();
            });

            resourceService.get('tasks', function (data) {
                tasksVm.tasks = data;
                if (data.length === 0) {
                    tasksVm.addWarning('No tasks found');
                    resourceService.get('categories', function (data2) {
                        if (data2.length === 0) {
                            tasksVm.noItems = true;
                        }
                    });
                }
            });
        }

        function loadTasks () {
            resourceService.get('tasks');
        }
    }
})();
