(function() {
    'use strict';

    angular
        .module('checkpoint')
        .controller('editPlanningTaskModal', editPlanningTaskModal);

    editPlanningTaskModal.$inject = ['$uibModalInstance', 'data', 'resourceService'];

    function editPlanningTaskModal($uibModalInstance, data, resourceService) {
        /* jshint validthis:true */
        var editPlanVm = this;

        editPlanVm.enums = {
            dTypes: [],
            dUnits: [],
            fTypes: [],
            fUnits: []
        };
        editPlanVm.task = data;
        editPlanVm.showAddDuration = showAddDuration;
        editPlanVm.showAddFrequency = showAddFrequency;
        editPlanVm.confirm = confirm;
        editPlanVm.cancel = cancel;

        activate();

        function activate() {
            resourceService.get('users/enums', getEnums);
        }

        function getEnums(data) {
            editPlanVm.enums = data;
        }

        function showAddDuration () {
            if (!editPlanVm.task.duration.type) {
                return false;
            }
            return editPlanVm.task.duration.type.id !== 'None';
        }

        function showAddFrequency () {
            if (!editPlanVm.task.frequency.type) {
                return false;
            }
            return editPlanVm.task.frequency.type.id !== 'Once';
        }

        function confirm() {
            resourceService
                .edit('planningtasks', editPlanVm.task)
                .then(editSuccess, editError);
        }

        function editSuccess(response) {
            $uibModalInstance.close(editPlanVm.task);
        }

        function editError(err) {
            editPlanVm.addError(err.data.message);
        }

        function cancel () {
            $uibModalInstance.dismiss('cancel');
        }
    }

})();