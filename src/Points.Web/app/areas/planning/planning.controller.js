(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('planningController', planningController);

    planningController.$inject = ['$scope', 'resourceService', '$timeout', '$uibModal', 'filterService'];

    function planningController($scope, resourceService, $timeout, $uibModal, filterService) {
        /* jshint validthis:true */
        var planningVm = this;

        planningVm.noItems = false;
        planningVm.tasks = [];
        planningVm.availableTasks = false;
        planningVm.taskFilter = filterService.getPTaskFilter();
        planningVm.addTask = addTask;
        
        activate();

        function activate() {
            filterService.subscribe($scope, 'ptaskFilter', getPTaskFilter);

            resourceService.get('planningtasks', getPlanningTasks);
            resourceService.get('tasks/available', getAvailableTasks);
        }

        function getPTaskFilter() {
            planningVm.taskFilter = filterService.getPTaskFilter();
        }

        function getPlanningTasks(data) {
            planningVm.tasks = data;
            if (data.length === 0) {
                planningVm.addWarning('No planning tasks found');
                resourceService.get('tasks/available', checkAvailableTasks);
            } else {
                setupCats();
            }
        }

        function checkAvailableTasks(data) {
            if (data.length === 0) {
                planningVm.noItems = true;
            }
        }

        function getAvailableTasks(data) {
            planningVm.availableTasks = data.length > 0;
        }

        function setupCats () {
            for (var i = 0; i < planningVm.tasks.length; i++) {
                planningVm.tasks[i].isOpen = i === 0;
            }
        }

        function addTask() {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: '/app/areas/planning/newPlanningTask.html',
                controller: 'newPlanningTaskModal',
                controllerAs: 'newPlanVm',
                size: 'lg',
                resolve: {
                    data: null
                }
            });
            modalInstance.result.then(modalResult);
        }

        function modalResult(result) {
            if (result) {
                planningVm.addSuccess("Task '{0}' successfully added".format(result.name));
            }
        }
    }
})();
