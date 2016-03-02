(function() {
    'use strict';

    angular
        .module('checkpoint')
        .controller('newPlanningTaskModal', newPlanningTaskModal);

    newPlanningTaskModal.$inject = ['$uibModalInstance', 'data', 'resourceService'];

    function newPlanningTaskModal($uibModalInstance, data, resourceService) {
        /* jshint validthis:true */
        var newPlanVm = this;

        newPlanVm.addTaskData = {
            duration: {
                value: 0,
                type: {},
                unit: {}
            },
            frequency: {
                value: 1,
                type: {},
                unit: {}
            },
            task: {},
            cat: {}
        };
        newPlanVm.cats = [];
        newPlanVm.enums = {
            dTypes: [],
            dUnits: [],
            fTypes: [],
            fUnits: []
        };
        newPlanVm.showAddDuration = showAddDuration;
        newPlanVm.showAddFrequency = showAddFrequency;
        newPlanVm.confirm = confirm;
        newPlanVm.cancel = cancel;
        
        activate();

        function activate() {
            resourceService.get('availabletasks', function (data) {
                newPlanVm.cats = data;
                resetDropdowns();
            });
            resourceService.get('enums', function (data) {
                newPlanVm.enums = data;
                resetDropdowns();
            });
        }

        function resetDropdowns () {
            newPlanVm.addTaskData.cat = newPlanVm.cats[0];
            if (newPlanVm.cats.length > 0) {
                newPlanVm.addTaskData.task = newPlanVm.cats[0].tasks[0];
            }
            newPlanVm.addTaskData.duration.type = newPlanVm.enums.dTypes[0];
            newPlanVm.addTaskData.duration.unit = newPlanVm.enums.dUnits[0];
            newPlanVm.addTaskData.frequency.type = newPlanVm.enums.fTypes[0];
            newPlanVm.addTaskData.frequency.unit = newPlanVm.enums.fUnits[0];
        }

        function showAddDuration () {
            if (!newPlanVm.addTaskData.duration.type) {
                return false;
            }
            return newPlanVm.addTaskData.duration.type.id !== 'None';
        }

        function showAddFrequency () {
            if (!newPlanVm.addTaskData.frequency.type) {
                return false;
            }
            return newPlanVm.addTaskData.frequency.type.id !== 'Once';
        }

        function confirm () {
            newPlanVm.addTaskData.name = newPlanVm.addTaskData.task.name;
            resourceService.add('planningtasks', newPlanVm.addTaskData).then(
                function (response) {
                    $uibModalInstance.close(newPlanVm.addTaskData);
                },
                function (err) {
                    newPlanVm.addError(err.data.message);
                }
            );
        }

        function cancel () {
            $uibModalInstance.dismiss('cancel');
        }
    }

})();