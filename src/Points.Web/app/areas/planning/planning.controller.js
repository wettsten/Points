(function () {
    'use strict';

    angular
        .module('checkpoint')
        .controller('planningController', planningController);

    planningController.$inject = ['$scope', 'resourceService', '$timeout', 'modalService', 'filterService'];

    function planningController($scope, resourceService, $timeout, modalService, filterService) {
        /* jshint validthis:true */
        var planningVm = this;

        planningVm.noItems = false;
        planningVm.tasks = [];
        planningVm.availableTasks = false;
        planningVm.taskFilter = filterService.getPTaskFilter();
        planningVm.addTask = addTask;
        
        activate();

        function activate() {
            filterService.subscribe($scope, 'ptaskFilter', function () {
                planningVm.taskFilter = filterService.getPTaskFilter();
            });

            resourceService.get('planningtasks', function (data) {
                planningVm.tasks = data;
                if (data.length === 0) {
                    planningVm.addWarning('No planning tasks found');
                    resourceService.get('availabletasks', function (data2) {
                        if (data2.length === 0) {
                            planningVm.noItems = true;
                        }
                    });
                } else {
                    setupCats();
                }
            });
            resourceService.get('availabletasks', function (data) {
                planningVm.availableTasks = data.length > 0;
            });
        }

        function setupCats () {
            for (var i = 0; i < planningVm.tasks.length; i++) {
                planningVm.tasks[i].isOpen = i === 0;
            }
        }

        function addTask() {
            modalService.newModal('newPlanningTask', null, 'lg',
                function (result) {
                    planningVm.addSuccess("Task '{0}' successfully added".format(result.name));
                }
            );
        }
    }
})();
